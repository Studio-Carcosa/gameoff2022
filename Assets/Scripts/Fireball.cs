using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("stats")]
    public float speed = 12f;
    public float lifetime = 1.5f;  

    [System.NonSerialized]
    public Rigidbody rb;
    public GameObject target;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
        target =  GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
    }
    
    public void Init(){
        rb.AddForce(-transform.forward * speed, ForceMode.Impulse);
    }
}
