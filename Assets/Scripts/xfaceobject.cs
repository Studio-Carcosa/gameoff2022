using UnityEngine;
using System.Collections;

public class xfaceobject : MonoBehaviour
{

    public GameObject camera;

    void Update()
    {
        Vector3 v = camera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(camera.transform.position - v);
    }
}

