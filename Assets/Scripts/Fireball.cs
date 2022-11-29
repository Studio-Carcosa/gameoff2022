using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("stats")]
    public float speed = 4f;
    public float lifetime = 10f;  

    [System.NonSerialized]
    public Rigidbody rb;
    public GameObject target;
    
    void Awake(){
        rb = GetComponent<Rigidbody>();
        target =  GameObject.FindWithTag("Player");
    }


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
    }
    
    public void Init(GameObject player){
        
        rb.AddForce(-(transform.position - target.transform.position), ForceMode.Impulse);

        
    }   
    
    void OnCollisionEnter(Collision other){
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other){
        Destroy(this.gameObject);
    }
}