using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombie : MonoBehaviour
{

    [Header("Movement stats")]
    public float moveSpeed = 4f;
    public float health = 10f;

    [Header("Alert radius")]
    public float alertDist = 15f;

    [System.NonSerialized]
    public GameObject target;

    private Rigidbody rb;
    private bool alerted;

    /*      ENABLE WHEN WE HAVE SPRITES
    *
    *
    *public Sprite defaultZombie;
    *private SpriteRenderer sr;
    */

    //TODO Improve zombie AI so they don't aim to clip through player and instead stop and wind up attacks

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        /*      ENABLE WHEN WE HAVE SPRITES
        //sr = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        */

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
        transform.forward = -transform.forward;

        //delete if no health
        if(health < 0){Destroy(gameObject);}
        
        //check if in alert radius
        if(Vector3.Distance(target.transform.position, transform.position) < alertDist){Activate();}

        if(alerted){
            if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < moveSpeed){
                rb.AddRelativeForce(-Vector3.forward * moveSpeed, ForceMode.Force);
            }
        }

    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "PlayerBullet"){
            health = health -20;
            Activate();
            //SpawnEnemy(enemy);
        } else if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<playerHealth>().Hurt(40);
        }
    }

    //enable enemy
    void Activate(){
        alerted = true;
    }
}
