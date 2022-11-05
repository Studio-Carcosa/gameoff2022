Shader "PSFX/Standard" {
    Properties
    {
        _BlendMode("Blend Mode", Int) = 0
        _SrcBlend("Src Blend Mode", Int) = 0
        _DstBlend("Dst Blend Mode", Int) = 0
        _ZWrite("ZWrite", Int) = 0
        _LightingMode("Lighting Mode", Int) = 0
        _Cull("Culling Mode", Int) = 2

        [Slider] _SpecularStrength("Shininess", Range(0.01,1)) = 0.75
        [Slider] _SpecularGlossiness("Glossiness", Range(0,2)) = 0.46

        _Color("Shade", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Cutoff("Cutoff", Range(0,1)) = 0.5
        _ReflectionBlendMode("Reflection Blend Mode", Float) = 0
        _Reflection2DRotation("Reflection Map 2D Rotation", Float) = 0
        _Reflection2D("Reflection Map 2D", 2D) = "" {}
        _Reflection3D("Reflection Map 3D", Cube) = "" {}
        _ReflectionMask("Reflection Mask", 2D) = "white" {}

        _OverrideCamera("Override PSFX Camera Distortions", Int) = 0

        // Reflections
        [ToggleOff] _ReflectionMode("Reflections Mode", Int) = 0
        [PowerSlider(2)] _AffineDistortion("Affine Distortion", Range(0,1)) = 1
        [PowerSlider(5.0)] _VertexPrecision("Vertex Precision", Range(0.0001, 0.04)) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull[_Cull]
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]

        CGPROGRAM

        #pragma surface surf BlinnPhong keepalpha addshadow vertex:verts
        #pragma multi_compile __ PSFX_REFLECTIONS_3D PSFX_REFLECTIONS_2D
        #pragma target 3.5

        #include "UnityCG.cginc"

        uniform float _PSFX_TriangleCullDistance;
        uniform int _PSFX_TriangleNearClipping;

        int    _BlendMode;
        int    _LightingMode;
        half   _SpecularStrength;
        fixed  _SpecularGlossiness;
        float4 _Color;
        half   _Cutoff;
        int    _ReflectionBlendMode;
        float  _Reflection2DRotation;
        int    _ReflectionMode;
        float  _AffineDistortion;
        float  _VertexPrecision;

        sampler2D _MainTex;
        sampler2D _ReflectionMask;
#ifdef PSFX_REFLECTIONS_2D
        sampler2D _Reflection2D;
        float4    _Reflection2D_ST;
#endif
#ifdef PSFX_REFLECTIONS_3D
        samplerCUBE _Reflection3D;
#endif

        struct vertexInOut
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 texcoord : TEXCOORD0;
            float3 normal : NORMAL;
            float3 texcoord1 : TEXCOORD1;
            float3 texcoord2 : TEXCOORD2;
        };

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
            float3 worldRefl;
            float4 screenPos;
            float4 vertex : SV_POSITION;
            float warpAmount;
            float clip;
        };

        void verts(inout vertexInOut v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            float3 worldScale = float3(
                length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)),
                length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)),
                length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))
                );

            float3 localPrecision = _VertexPrecision / worldScale;

            // ======================
            // Vertex precision
            // ======================
            float4 wpos = UnityObjectToClipPos(v.vertex);
            float4 vx = v.vertex + wpos;
            vx.xyz /= vx.w;
            vx.x = round(vx.x / localPrecision.x) * localPrecision.x;
            vx.y = round(vx.y / localPrecision.y) * localPrecision.y;
            vx.z = round(vx.z / localPrecision.z) * localPrecision.z;
            vx.xyz *= vx.w;

            v.vertex.xyz = vx - wpos;

            // ======================
            // Affine distortion
            // ======================

            float distortion = abs(UnityObjectToClipPos(v.vertex).w);
            float lerpDist = lerp(1, distortion, _AffineDistortion);

            v.texcoord.xy *= lerpDist;
            o.warpAmount = lerpDist;

            // ======================
            // Triangle clipping
            // ======================
            float viewDepth = -UnityObjectToViewPos(v.vertex).z;

            o.clip = 1;

            if (viewDepth < _ProjectionParams.y && _PSFX_TriangleNearClipping > 0)
                o.clip = 0;

            // ======================
            // Triangle distance culling
            // ======================

            bool isNull = _PSFX_TriangleCullDistance < _PSFX_TriangleNearClipping || _PSFX_TriangleCullDistance == 0;
            float cullDistance = isNull ? 10000 : _PSFX_TriangleCullDistance;

            if (viewDepth > cullDistance)
                o.clip = 0;
        }

        void surf (Input i, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, i.uv_MainTex / i.warpAmount) * _Color * i.color;

            // ======================
            // Triangle clipping
            // ======================
            if (i.clip < 0.999999)
            {
                clip(-1);

                return;
            }

            // ======================
            // Calculate reflections
            // ======================
            #if defined(PSFX_REFLECTIONS_2D) || defined(PSFX_REFLECTIONS_3D)
                fixed3 reflectIntensityRGB = tex2D(_ReflectionMask, i.uv_MainTex / i.color.w);
                fixed reflectIntensity = reflectIntensityRGB.r;
                fixed3 reflect = fixed3(1, 1, 1);
            #endif

            #ifdef PSFX_REFLECTIONS_2D
                float sinx = sin(3.14159 * -_Reflection2DRotation / 180); // sin
                float cosx = cos(3.14159 * -_Reflection2DRotation / 180); // cos
                float2x2 rotationMatrix = float2x2(cosx, -sinx, sinx, cosx);
                float2 screenPos = i.screenPos.xy / max(i.screenPos.w, 0.00001);
                screenPos.x *= (_ScreenParams.x / _ScreenParams.y);
                screenPos.xy -= _Reflection2D_ST.zw;
                screenPos.xy *= _Reflection2D_ST.xy;

                float2 uv = mul(screenPos, rotationMatrix);

                reflect = tex2D(_Reflection2D, uv);
            #endif

            #ifdef PSFX_REFLECTIONS_3D
                reflect = texCUBE(_Reflection3D, i.worldRefl).rgba;
            #endif

            #if defined(PSFX_REFLECTIONS_2D) || defined(PSFX_REFLECTIONS_3D)
                switch (_ReflectionBlendMode)
                {
                case 0: // Multiply
                    o.Albedo = lerp(c, c * reflect.rgb, reflectIntensity);
                    break;
                case 1: // Add
                    reflect.rgb *= reflectIntensity;

                    o.Albedo = saturate(c + reflect.rgb);
                    break;
                case 2: // Replace
                    o.Albedo = lerp(c, reflect.rgb, reflectIntensity);
                    break;
                }
            #else
                o.Albedo = c.rgb * i.color.rgb;
            #endif


            // ======================
            // Miscellanneous 
            // ======================
            o.Alpha = _BlendMode == 0 ? 1 : c.a;
            o.Specular = 0.00001;
            o.Gloss = 0;
            _SpecColor = half4(1, 1, 1, 1);

            // ======================
            // Apply lighting
            // ======================
            switch (_LightingMode)
            {
                case 0: // Unlit
                    o.Emission = o.Albedo;
                    o.Albedo = fixed3(0, 0, 0);
                    break;
                case 1: // Diffuse
                    break;
                case 2: // Specular
                    if(_SpecularStrength < 0.01)
                        _SpecularStrength = 0.01;

                    o.Specular = _SpecularStrength;
                    o.Gloss = _SpecularGlossiness;
                    break;
            }

            // ======================
            // Cutout
            // ======================
            if(_BlendMode == 2)
                clip(c.a - min(_Cutoff, 0.9999));
        }
        ENDCG
    }
    CustomEditor "PSXShaderGUI"
}
