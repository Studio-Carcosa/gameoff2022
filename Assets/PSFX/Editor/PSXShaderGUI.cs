using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PSXShaderGUI : ShaderGUI
{
    public enum BlendMode
    {
        Opaque,
        Transparent,
        Cutout
    }

    public enum LightingMode
    {
        Unlit,
        Diffuse,
        Specular
    }

    public enum ReflectionMode
    {
        Off, TwoD, ThreeD
    }

    public enum ReflectionBlendMode
    {
        Multiply, Add, Screen, Replace,
    }

    string[] blendNames = new string[]
    {
        "Opaque",
        "Transparent",
        "Cutout"
    };

    string[] lightingModes = new string[]
    {
        "Unlit",
        "Diffuse",
        "Specular"
    };

    string[] cullingModes = new string[]
    {
        "Off",
        "Front",
        "Back"
    };

    string[] reflectionBlendModes = new string[]
    {
        "Multiply",
        "Add",
        "Replace"
    };

    string[] reflectionNames = new string[]
    {
        "Off", "2D", "3D"
    };

    BlendMode materialBlendMode
    {
        get {
            return (BlendMode)(int)blendMode.floatValue;
        }
    }

    ReflectionMode materialReflectionMode {
        get {
            return (ReflectionMode)((int)reflections.floatValue);
        }
        set {
            reflections.floatValue = (float)value;
        }
    }

    LightingMode materialLightingMode {
        get {
            return (LightingMode)((int)lightingMode.floatValue);
        }
        set {
            lightingMode.floatValue = (float)value;
        }
    }

    ReflectionBlendMode materialReflectionBlendMode {
        get {
            return (ReflectionBlendMode)((int)reflectionBlendMode.floatValue);
        }
        set {
            reflectionBlendMode.floatValue = (float)value;
        }
    }

    UnityEngine.Rendering.CullMode materialCullingMode {
        get {
            return (UnityEngine.Rendering.CullMode)((int)cullingMode.floatValue);
        }
        set {
            cullingMode.floatValue = (float)value;
        }
    }
    
    int materialVertexPrecision {
        get {
            return Mathf.RoundToInt(Mathf.Lerp(1, 8, (vertexPrecision.floatValue - 0.01f)/(0.0001f - 0.01f)));
        }
        set {
            vertexPrecision.floatValue = Mathf.Lerp(0.01f, 0.0001f, (value-1f)/7);
        }
    }
    
    bool materialOverrideCamera {
        get {
            return overridePSFXCamera.floatValue == 1;
        }
        set {
            overridePSFXCamera.floatValue = value ? 1 : 0;
        }
    }

    #region "Styles"
    GUIContent mainTextureText = new GUIContent("Main Texture");
    GUIContent reflectionMaskText = new GUIContent("Reflection Mask");
    #endregion

    #region "Material Properties"
    MaterialProperty blendMode;
    MaterialProperty lightingMode;
    MaterialProperty cullingMode;
    MaterialProperty specularStrength;
    MaterialProperty specularGlossiness;
    MaterialProperty mainTexture;
    MaterialProperty mainTextureColor;
    MaterialProperty reflectionMask;
    MaterialProperty alphaCutoff;
    MaterialProperty reflections;
    MaterialProperty reflectionBlendMode;
    MaterialProperty reflectionMap2D;
    MaterialProperty reflectionMap2DRotation;
    MaterialProperty reflectionMap3D;
    MaterialProperty overridePSFXCamera;
    MaterialProperty affineDistortion;
    MaterialProperty vertexPrecision;
    #endregion

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        InitializeProperties(properties);

        SetBlendMode((Material)materialEditor.target, (BlendMode)EditorGUILayout.Popup("Rendering Mode", (int)materialBlendMode, blendNames));
        materialLightingMode = (LightingMode)EditorGUILayout.Popup("Lighting Mode", (int)materialLightingMode, lightingModes);
        materialCullingMode =  (UnityEngine.Rendering.CullMode)EditorGUILayout.Popup("Culling Mode", (int)materialCullingMode, cullingModes);

        if(materialLightingMode == LightingMode.Specular) {
            materialEditor.ShaderProperty(specularStrength, "Shininess", MaterialEditor.kMiniTextureFieldLabelIndentLevel);
            materialEditor.ShaderProperty(specularGlossiness, "Glossiness", MaterialEditor.kMiniTextureFieldLabelIndentLevel);
        }

        // Maps
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Main Maps", EditorStyles.boldLabel);
        DrawMainTextureMap(materialEditor);
        EditorGUILayout.Space();
        DrawReflectionMap(materialEditor);
        EditorGUILayout.Space();
        materialEditor.TextureScaleOffsetProperty(mainTexture);
        EditorGUILayout.Space();
        DrawPSXDistortions(materialEditor);
    }

    void DrawMainTextureMap(MaterialEditor materialEditor)
    {
        materialEditor.TexturePropertySingleLine(mainTextureText, mainTexture, mainTextureColor);

        if(mainTexture.textureValue!=null)
            if(mainTexture.textureValue.filterMode != FilterMode.Point)
                EditorGUILayout.HelpBox("It is recommended to use Point filtering for textures for an authentic PSX experience", MessageType.Info);

        if(materialBlendMode == BlendMode.Cutout)
            materialEditor.ShaderProperty(alphaCutoff, "Alpha Cutoff", MaterialEditor.kMiniTextureFieldLabelIndentLevel);
    }

    void DrawReflectionMap(MaterialEditor materialEditor)
    {
        materialReflectionMode = (ReflectionMode)EditorGUILayout.Popup("Reflection Mode", (int)materialReflectionMode, reflectionNames);

        SetReflectionMode((Material)materialEditor.target, materialReflectionMode);

        if(materialReflectionMode != ReflectionMode.Off) {
            DrawReflectionMask(materialEditor);
            materialReflectionBlendMode = (ReflectionBlendMode)EditorGUILayout.Popup("Blending Mode", (int)materialReflectionBlendMode, reflectionBlendModes);

            switch(materialReflectionMode) {
                case ReflectionMode.TwoD:
                    materialEditor.ShaderProperty(reflectionMap2DRotation, "Rotation Offset (Degrees)", MaterialEditor.kMiniTextureFieldLabelIndentLevel);
                    materialEditor.ShaderProperty(reflectionMap2D, "Reflection Map", MaterialEditor.kMiniTextureFieldLabelIndentLevel); break;
                case ReflectionMode.ThreeD:
                    materialEditor.ShaderProperty(reflectionMap3D, "Reflection Map", MaterialEditor.kMiniTextureFieldLabelIndentLevel); break;
            }
        }
    }

    void DrawReflectionMask(MaterialEditor materialEditor)
    {
        materialEditor.TexturePropertySingleLine(reflectionMaskText, reflectionMask);

        if(reflectionMask.textureValue!=null)
            if(reflectionMask.textureValue.filterMode != FilterMode.Point)
                EditorGUILayout.HelpBox("It is recommended to use Point filtering for textures for an authentic PSX experience", MessageType.Info);
    }

    void DrawPSXDistortions(MaterialEditor materialEditor)
    {
        materialOverrideCamera = EditorGUILayout.BeginToggleGroup("Override PSFXCamera Distortions", materialOverrideCamera);
        
        materialVertexPrecision = EditorGUILayout.IntSlider("Vertex Precision", materialVertexPrecision, 1, 8);
        materialEditor.ShaderProperty(affineDistortion, "Affine Distortion");
        EditorGUILayout.EndToggleGroup();
    }

    void SetBlendMode(Material material, BlendMode blendMode)
    {
        switch(blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetOverrideTag("ForceNoShadowCasting", "False");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

                break;
            case BlendMode.Cutout:
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetOverrideTag("ForceNoShadowCasting", "False");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;

                break;
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetOverrideTag("ForceNoShadowCasting", "True");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                break;
        }

        material.SetInt("_BlendMode", (int)blendMode);
    }

    void SetReflectionMode(Material material, ReflectionMode reflectionMode)
    {
        switch(reflectionMode)
        {
            case ReflectionMode.Off:
                material.DisableKeyword("PSFX_REFLECTIONS_2D");
                material.DisableKeyword("PSFX_REFLECTIONS_3D");
                break;
            case ReflectionMode.TwoD:
                material.EnableKeyword("PSFX_REFLECTIONS_2D");
                material.DisableKeyword("PSFX_REFLECTIONS_3D");
                break;
            case ReflectionMode.ThreeD:
                material.DisableKeyword("PSFX_REFLECTIONS_2D");
                material.EnableKeyword("PSFX_REFLECTIONS_3D");
                break;
        }
    }

    void InitializeProperties(MaterialProperty[] properties)
    {
        blendMode = FindProperty("_BlendMode", properties);
        lightingMode = FindProperty("_LightingMode", properties);
        cullingMode = FindProperty("_Cull", properties);

        specularStrength = FindProperty("_SpecularStrength", properties);
        specularGlossiness = FindProperty("_SpecularGlossiness", properties);

        mainTexture = FindProperty("_MainTex", properties);
        mainTextureColor = FindProperty("_Color", properties);
        reflectionMask = FindProperty("_ReflectionMask", properties);
        alphaCutoff = FindProperty("_Cutoff", properties);
        reflections = FindProperty("_ReflectionMode", properties, false);
        reflectionBlendMode = FindProperty("_ReflectionBlendMode", properties, false);
        reflectionMap2D = FindProperty("_Reflection2D", properties);
        reflectionMap2DRotation = FindProperty("_Reflection2DRotation", properties);
        reflectionMap3D = FindProperty("_Reflection3D", properties);
        
        overridePSFXCamera = FindProperty("_OverrideCamera", properties);
        affineDistortion = FindProperty("_AffineDistortion", properties);
        vertexPrecision = FindProperty("_VertexPrecision", properties);
    }
}