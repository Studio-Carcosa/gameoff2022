using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PSFXCamera))]
public class PSFXCameraGUI : Editor {
    GUIContent resolutionLabel = new GUIContent() { text = "Resolution Scale" };
    GUIContent cameraPrecisionLabel = new GUIContent() { text = "Camera Position Precision" };
    GUIContent fadeLabel = new GUIContent() { text = "Fade" };
    GUIContent vertexPrecisionLabel = new GUIContent() { text = "Vertex Precision" };
    GUIContent affineStrengthLabel = new GUIContent() { text = "Affine Distortion" };
    GUIContent colorDepthLabel = new GUIContent() { text = "Color Depth" };
    GUIContent ditheringStrengthLabel = new GUIContent() { text = "Dithering Strength" };
    GUIContent borderColorLabel = new GUIContent() { text = "Border Color" };

    string[] fadeOptions = new string[] { "Subtractive", "Chromatic Cascade", "Normal" };

    public override void OnInspectorGUI() {
        var resolution = serializedObject.FindProperty("resolutionScale");
        var cameraPrecision = serializedObject.FindProperty("cameraPositionPrecision");
        var fade = serializedObject.FindProperty("fadeOut");
        var fadeMethod = serializedObject.FindProperty("fadeMethod");
        var vertexPrecision = serializedObject.FindProperty("vertexPrecision");
        var affineStrength = serializedObject.FindProperty("affineDistortion");
        var triangleCullingDistance = serializedObject.FindProperty("triangleCullDistance");
        var triangleNearClipping = serializedObject.FindProperty("triangleNearClipping");
        var enableColorDepth = serializedObject.FindProperty("enableColorDepth");
        var colorDepth = serializedObject.FindProperty("colorDepth");
        var ditheringStrength = serializedObject.FindProperty("ditheringStrength");
        var enableLetterboxing = serializedObject.FindProperty("enableLetterboxing");
        var xyAspectRatio = serializedObject.FindProperty("xyAspectRatio");
        var borderColor = serializedObject.FindProperty("letterboxColor");

        EditorGUILayout.IntSlider(resolution, 1, 10, resolutionLabel); // The 10 limit is completely arbitrary. Feel free to set this as high as you would like.
        EditorGUILayout.IntSlider(cameraPrecision, 1, 10, cameraPrecisionLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Slider(fade, 0, 1, fadeLabel);

        fadeMethod.intValue = EditorGUILayout.Popup(fadeMethod.intValue, fadeOptions, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("PSX Distortion", EditorStyles.boldLabel);
        EditorGUILayout.IntSlider(vertexPrecision, 1, 8, vertexPrecisionLabel);
        EditorGUILayout.Slider(affineStrength, 0, 1, affineStrengthLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("PSX Depth", EditorStyles.boldLabel);
        triangleCullingDistance.floatValue = EditorGUILayout.FloatField("Triangle Culling Distance", triangleCullingDistance.floatValue);
        triangleNearClipping.boolValue = EditorGUILayout.Toggle("Triangle Near Clipping", triangleNearClipping.boolValue);

        EditorGUILayout.Space();
        enableColorDepth.boolValue = EditorGUILayout.BeginToggleGroup("Dithering and Color Depth", enableColorDepth.boolValue);
        EditorGUILayout.IntSlider(colorDepth, 1, 32, colorDepthLabel);
        EditorGUILayout.Slider(ditheringStrength, 0, 1, ditheringStrengthLabel);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();
        enableLetterboxing.boolValue = EditorGUILayout.BeginToggleGroup("Camera Letterboxing", enableLetterboxing.boolValue);
        xyAspectRatio.vector2Value = EditorGUILayout.Vector2Field("X:Y Aspect Ratio", xyAspectRatio.vector2Value);
        // While this is marked as obsolete, it is kept to keep compatibility with Unity 2018.x
        #pragma warning disable 0618
        borderColor.colorValue = EditorGUILayout.ColorField(borderColorLabel, borderColor.colorValue, true, false, false, hdrConfig: null);
        #pragma warning restore 0618
        EditorGUILayout.EndToggleGroup();

        serializedObject.ApplyModifiedProperties();
    }
}