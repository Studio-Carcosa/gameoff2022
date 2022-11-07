Shader "Hidden/PSFXCameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeMethod ("Fade Method", Int) = 0
        _FadeOut ("Fade Out", Range(0,1)) = 0
        _ResolutionScale ("Resolution Scale", Range(1,100)) = 1
        _ColorDepth("Color Depth", Range(1,32)) = 24
        _DitheringStrength("Dithering Strength", Range(0,1)) = 0.5
        _AspectRatio("Aspect Ratio", Vector) = (0, 0, 0, 0) // When Z is 1, letterboxing is enabled
        _LetterboxingColor("Letterboxing Color", Color) = (0, 0, 0, 0)
    }
    SubShader
        {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertexIn
        {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _FadeMethod;
            fixed _FadeOut;
            fixed _DitheringStrength;
            uint _ResolutionScale;
            uint _ColorDepth;
            fixed3 _LetterboxingColor;
            half3 _AspectRatio;

            vertexOut vert(vertexIn v)
            {
                vertexOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed dither(uint x, uint y)
            {
                float4x4 tab = float4x4 (
                    -4.0, 0.0, -3.0, 1.0,
                    2.0, -2.0, 3.0, -1.0,
                    -3.0, 1.0, -4.0, 0.0,
                    3.0, -1.0, 2.0, -2.0
                );

                return tab[x%4][y%4];
            }

            fixed4 frag(vertexOut i) : SV_Target
            {
                // Calculate UV with the resolution scale
                fixed texelSize = _ResolutionScale % 2 == 0 ? 0.25 : 0.5;
                float2 screenUV = _ResolutionScale*(floor((_ScreenParams.xy / _ResolutionScale) * i.uv) + texelSize) / _ScreenParams.xy;
                
                fixed4 c = tex2D(_MainTex, screenUV);

                // Fade out
                switch (_FadeMethod)
                {
                case 0:
                    c = saturate(saturate(c) - _FadeOut);

                    break;
                case 1:
                    c.r = saturate(c.r) - _FadeOut;
                    c.g = saturate(c.g) - _FadeOut * _FadeOut;
                    c.b = saturate(c.b) - _FadeOut * _FadeOut * _FadeOut;

                    c = saturate(c);

                    break;
                case 2:
                    c = lerp(saturate(c), 0, _FadeOut);

                    break;
                }

                // Dithering
                if(_ColorDepth <= 255) // If the value is over 255 we can assume this is disabled
                { 
                    int2 puv = _ScreenParams.xy/_ResolutionScale * i.uv;
                    c += dither(puv.x, puv.y) / 4 *lerp(0, 0.35, _DitheringStrength);
                    c = floor(c*_ColorDepth)/_ColorDepth;
                }

                // Letterboxing
                if (_AspectRatio.z == 1)
                {
                    float widthRatio = _AspectRatio.x;
                    float heightRatio = _AspectRatio.y;

                    float2 centeredUV = abs(i.uv - 0.5) * 2;
                    float2 screenRatio = _ScreenParams.xy;
                    screenRatio /= max(screenRatio.x, screenRatio.y);

                    float2 uvAdjusted = centeredUV * screenRatio;

                    if (uvAdjusted.x > 1 / heightRatio * screenRatio.y)
                        c = fixed4(_LetterboxingColor.rgb, 1);

                    if (uvAdjusted.y > 1 / widthRatio * screenRatio.x * _AspectRatio.x / _AspectRatio.y)
                        c = fixed4(_LetterboxingColor.rgb, 1);
                }

                return c;
            }
            ENDCG
        }
    }
}
