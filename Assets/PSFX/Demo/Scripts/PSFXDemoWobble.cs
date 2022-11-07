using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFXDemoWobble : MonoBehaviour {
    // Speed at which the object should turn each frame. Measured in degrees per second.
    public Vector3 WobbleSpeed;
    public Vector3 Magnitude;

    void Update() {
        Vector3 sinTime = new Vector3(Mathf.Sin(WobbleSpeed.x * Time.realtimeSinceStartup), Mathf.Sin(WobbleSpeed.y * Time.realtimeSinceStartup), Mathf.Sin(WobbleSpeed.z * Time.realtimeSinceStartup));

        transform.localEulerAngles = Vector3.Scale(Magnitude, sinTime);
    }
}
