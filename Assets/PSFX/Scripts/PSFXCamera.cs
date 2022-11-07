using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PSFXFadeMethod
{
    Subtractive = 0,
    ChromaticCascade = 1,
    Normal = 2
}

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class PSFXCamera : MonoBehaviour {
    [SerializeField] int resolutionScale = 2;
    [SerializeField] int colorDepth = 12;
    [SerializeField] int vertexPrecision = 5;
    [SerializeField] int cameraPositionPrecision = 2;
    [SerializeField] float triangleCullDistance = 150;
    [SerializeField] bool triangleNearClipping = false;
    [SerializeField] float fadeOut = 0;
    [SerializeField] PSFXFadeMethod fadeMethod = PSFXFadeMethod.Subtractive;
    [SerializeField] float ditheringStrength = 0.05f;
    [SerializeField] float affineDistortion = 1;
    [SerializeField] bool enableColorDepth = true;
    [SerializeField] bool enableLetterboxing = false;
    [SerializeField] Color letterboxColor = Color.black;
    [SerializeField] Vector2 xyAspectRatio = Vector2.one;

    public int Resolution
    {
        get
        {
            return resolutionScale;
        }
        set
        {
            resolutionScale = Mathf.Clamp(value, 1, int.MaxValue);
        }
    }

    public bool EnableColorDepth
    {
        get
        {
            return enableColorDepth;
        }
        set
        {
            enableColorDepth = value;
        }
    }
    
    public int ColorDepth
    {
        get
        {
            return colorDepth;
        }
        set
        {
            colorDepth = Mathf.Clamp(value, 1, 32);
        }
    }
    
    public int VertexPrecision
    {
        get
        {
            return vertexPrecision;
        }
        set
        {
            vertexPrecision = Mathf.Clamp(value, 1, int.MaxValue);
        }
    }
    
    public int CameraPositionPrecision
    {
        get
        {
            return cameraPositionPrecision;
        }
        set
        {
            cameraPositionPrecision = Mathf.Clamp(value, 1, int.MaxValue);
        }
    }
    
    public float TriangleCullDistance
    {
        get
        {
            return triangleCullDistance;
        }
        set
        {
            triangleCullDistance = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
    
    public bool TriangleNearClipping
    {
        get
        {
            return triangleNearClipping;
        }
        set
        {
            triangleNearClipping = value;
        }
    }
    
    public float Fade
    {
        get
        {
            return fadeOut;
        }
        set
        {
            fadeOut = Mathf.Clamp01(value);
        }
    }
    
    public PSFXFadeMethod FadeMethod
    {
        get
        {
            return fadeMethod;
        }
        set
        {
            fadeMethod = value;
        }
    }
    
    public float DitheringStrength {
        get
        {
            return ditheringStrength;
        }
        set
        {
            ditheringStrength = Mathf.Clamp01(value);
        }
    }
    
    public float AffineStrength
    {
        get
        {
            return affineDistortion;
        }
        set
        {
            affineDistortion = Mathf.Clamp01(value);
        }
    }
    
    public bool EnableLetterboxing
    {
        get
        {
            return enableLetterboxing;
        }
        set
        {
            enableLetterboxing = value;
        }
    }
    
    public Vector2 LetterboxAspectRatio
    {
        get
        {
            return xyAspectRatio;
        }
        set
        {
            float x = Mathf.Max(1, value.x);
            float y = Mathf.Max(1, value.y);

            xyAspectRatio = new Vector2(x, y);
        }
    }
    
    public Color BorderColor {
        get
        {
            return letterboxColor;
        }
        set
        {
            letterboxColor = value;
        }
    }

    Material blitMaterial;

    void ReloadShader()
    {
        blitMaterial = new Material(Shader.Find("Hidden/PSFXCameraShader"));
    }

    private void Awake()
    {
        ReloadShader();
    }

    Vector3 previousPosition;

    private void OnPreRender()
    {
        float vertexPrecision = Mathf.Lerp(0.01f, 0.0001f, (VertexPrecision - 1f) / 7);
        float cameraPrecision = Mathf.Lerp(0.01f, 0.0001f, (CameraPositionPrecision - 1f) / 7);

        // Calculate camera position according to the camera precision
        Vector3 lV = transform.position;
        float x = Mathf.Floor(lV.x / cameraPrecision) * cameraPrecision;
        float y = Mathf.Floor(lV.y / cameraPrecision) * cameraPrecision;
        float z = Mathf.Floor(lV.z / cameraPrecision) * cameraPrecision;

        previousPosition = transform.position;

        transform.position = new Vector3(x, y, z);
        
        // Apply properties to all PSFX materials
        foreach(Material m in Resources.FindObjectsOfTypeAll<Material>())
        {
            if(m.shader.name == "PSFX/Standard")
            {
                if(m.GetInt("_OverrideCamera") == 1)
                    continue;

                m.SetFloat("_AffineDistortion", affineDistortion);
                m.SetFloat("_VertexPrecision", vertexPrecision);
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(blitMaterial == null)
            ReloadShader();
        
        Shader.SetGlobalFloat("_PSFX_TriangleCullDistance", triangleCullDistance);
        Shader.SetGlobalInt("_PSFX_TriangleNearClipping", triangleNearClipping ? 1 : 0);

        blitMaterial.SetFloat("_FadeOut", Fade);
        blitMaterial.SetInt("_FadeMethod", (int)FadeMethod);
        blitMaterial.SetFloat("_ResolutionScale", Resolution);
        blitMaterial.SetInt("_ColorDepth", EnableColorDepth ? ColorDepth : 256);
        blitMaterial.SetFloat("_DitheringStrength", DitheringStrength);

        Vector3 aspectRatio = new Vector3(xyAspectRatio.x, xyAspectRatio.y, enableLetterboxing ? 1 : 0);
        float xyMaxAspectRatio = Mathf.Max(aspectRatio.x, aspectRatio.y);

        if(aspectRatio.x < 1)
            aspectRatio.x = 1;

        if(aspectRatio.y < 1)
            aspectRatio.y = 1;

        aspectRatio.x /= xyMaxAspectRatio;
        aspectRatio.y /= xyMaxAspectRatio;

        blitMaterial.SetVector("_AspectRatio", aspectRatio);
        blitMaterial.SetColor("_LetterboxingColor", letterboxColor);

        Graphics.Blit(source, destination, blitMaterial);
    }

    private void OnPostRender()
    {
        transform.position = previousPosition;
    }
}
