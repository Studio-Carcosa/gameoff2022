using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFXDemoBob : MonoBehaviour
{
    public float Offset = 1234f;
    public float BobbingMagnitude = 0.5f;
    public float BobbingFrequency = 0.5f;

    float startingY = 0;

    void Start()
    {
        startingY = transform.position.y;
    }

    void Update()
    {
        float positionChange = Mathf.Sin(Offset + Time.realtimeSinceStartup * BobbingFrequency) * BobbingMagnitude;

        transform.position = new Vector3(transform.position.x, startingY + positionChange, transform.position.z);  
    }
}
