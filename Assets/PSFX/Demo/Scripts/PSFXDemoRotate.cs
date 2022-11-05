using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFXDemoRotate : MonoBehaviour {
    // Speed at which the object should turn each frame. Measured in degrees per second.
    public Vector3 RotationSpeed;

    void Update() {
        transform.Rotate(RotationSpeed*Time.deltaTime);
    }
}
