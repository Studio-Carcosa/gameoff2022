using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [System.NonSerialized]
    public float damage = 20f;
    [System.NonSerialized]
    public float speed = 100f;
    [System.NonSerialized]
    public float scale = 1f;
    [System.NonSerialized]
    public float lifetime = 1.5f;  
    
    [System.NonSerialized]
    public Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    private void Start(){
        Destroy(this.gameObject, lifetime);
    }

    public void Init(Vector3 dir){
        rb.AddForce(dir * speed, ForceMode.Impulse);
    }

    private void Update(){

    }
}
