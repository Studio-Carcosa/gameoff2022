using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombie : MonoBehaviour
{

    [Header("Movement stats")]
    public float moveSpeed = 4f;
    public float health = 10f;

    [Header("Attack stats")]
    public static float attackTimer = 1.0f;
    private float attTim = attackTimer;

    [Header("Alert radius")]
    public float alertDist = 15f;

    [System.NonSerialized]
    public GameObject target;
    Transform attReference;
    
    public ZombieAttack att;
    private Rigidbody rb;
    private bool alerted;
    private bool canAttack = false;
    public int expDrop = 1;
    public GameObject expOrb;



    //TODO Improve zombie AI so they don't aim to clip through player and instead stop and wind up attacks

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        attReference = this.gameObject.transform.GetChild(0);


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
        transform.forward = -transform.forward;

        //delete if no health
        if(health <= 0){Die();}
        
        //check if in alert radius
        if(Vector3.Distance(target.transform.position, transform.position) < alertDist){Activate();}

        if(alerted){
            if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < moveSpeed && Vector3.Distance(target.transform.position, transform.position) > 1.5f){
                rb.AddRelativeForce(-Vector3.forward * moveSpeed, ForceMode.Force);
                canAttack = false;
            }
            else if(Vector3.Distance(target.transform.position, transform.position) < 1.5f){
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                canAttack = true;
            }
        }

        if(canAttack){
            if(attTim < 0){
                Attack();
                attTim = attackTimer;
            }
            else {
                attTim -= Time.deltaTime;
            }
        }
        else {
            attTim = attackTimer;
        }

    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "PlayerBullet"){
            health = health -20;
            Activate();
        } 
    }

    void Attack(){
        ZombieAttack a = Instantiate(att, attReference.position, transform.rotation);
        a.Init(target);
    }

    //enable enemy
    public void Activate(){
        alerted = true;
    }

    public void Die() {
        for (int i = 0; i < expDrop; i++){
        Instantiate(expOrb, gameObject.transform);
        
        }
        Destroy(gameObject);
    }
}
