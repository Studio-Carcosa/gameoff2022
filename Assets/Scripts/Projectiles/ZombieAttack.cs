using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [Header("stats")]
    public float speed = 4f;
    public float lifetime = 0.1f;  

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

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Init(GameObject player){
        
        rb.AddForce(-(transform.position - target.transform.position), ForceMode.Impulse);

        
    }   
    
    void OnCollisionEnter(Collision other){
        Destroy(this.gameObject);
    }
}
