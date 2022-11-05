using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFXDemoColorShift : MonoBehaviour
{
    public Material PSFXMaterialTarget;
    public float ShiftSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        // Only work when a material is present
        if(PSFXMaterialTarget == null)
            return;

        // Only work with a PSFX shader
        if(PSFXMaterialTarget.shader.name != "PSFX/Standard")
            return;
        
        float H, S, V;
        Color color = PSFXMaterialTarget.GetColor("_Color");

        // Get HSV
        Color.RGBToHSV(color, out H, out S, out V);

        // Perform color shift
        H = (H + ShiftSpeed * Time.deltaTime) % 1;

        // Reassign it back into the material
        PSFXMaterialTarget.SetColor("_Color", Color.HSVToRGB(H, S, V));
    }
}
