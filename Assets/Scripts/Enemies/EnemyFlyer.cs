using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyer : MonoBehaviour
{
    
    [Header("stats")]
    public float flySpeed = 10f;
    //public float sidespeed = 10f; //TODO add strafe around player when attacking
    //public float swoopSpeed = 15f;
    public float health = 20f;
    public float flyHeight = 5f;
    public float flyUpStrength = 10f;


    [Header("alert radius")]
    public float alertRadius = 40f;


    [System.NonSerialized]
    public GameObject target;
    public Fireball fbPrefab;
    Transform fbEjector;
    
    bool tooLowFloor;
    bool flyUp;
    bool touchedGrass = true; //TODO make this not true by default incase enemy doesn't spawn on ground level
    
    private Rigidbody rb;
    private bool alerted = false;


    public Sprite defaultFlyer;
    public Sprite flyerFlap;
    public Sprite flyerBack;
    public Sprite flyerBackFlap;
    public SpriteRenderer sr;

    
    

    public static float flapAnimTimer = 0.2f;
    private bool spriteIsFlap = false;
    private float flapTimer = flapAnimTimer;

    //private bool swooping = false;

    [Header("fireball stats")]
    public float fbTimerFloor = 5f;
    public float fbTimerCeil = 10f;
    private float fbTimer;



    // Start is called before the first frame update
    void Start()
    {
        //fireballEjector = this.gameObject.transform.GetChild(1);
        target =  GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        fbTimer = Random.Range(fbTimerFloor, fbTimerCeil);
        tooLowFloor = false;
        flyUp = false;
        fbEjector = this.gameObject.transform.GetChild(0);


        sr = gameObject.GetComponent<SpriteRenderer>();
        
        
    }

    // Update is called once per frame
    void Update()
    {        
        RaycastHit hit; 
        bool fleeing = false;

        //look at player
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
        transform.forward = -transform.forward;

        //have flyer realistically and bob up and down as if flying and keep enemy at certain height
        bool tooLowCeil = Physics.Raycast(transform.position, -transform.up, out hit, flyHeight + 2.5f);
        if(tooLowCeil){
            tooLowFloor = Physics.Raycast(transform.position, -transform.up, out hit, flyHeight);
            if(tooLowFloor){flyUp = true;}
            if(flyUp){rb.AddForce(transform.up * flyUpStrength, ForceMode.Force);}
        }else{flyUp = false;}

        if(!alerted){
            
        }
        else{

            //keep enemy within goldilocks zone
            if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < flySpeed){
                if(Vector3.Distance(target.transform.position, transform.position) > alertRadius - 5f){
                    rb.AddForce(-transform.forward * flySpeed, ForceMode.Force);
                    fleeing = false;
                } else if(Vector3.Distance(target.transform.position, transform.position) < alertRadius - 15f){
                    rb.AddForce(transform.forward * flySpeed, ForceMode.Force);
                    fleeing = true;
                }
            }
        }
        
        
        //Animate flapping
        touchedGrass = Physics.Raycast(transform.position, -transform.up, out hit, 0.5f);
        if(touchedGrass){
            flapTimer -= Time.deltaTime;
            if(spriteIsFlap){
                sr.sprite = defaultFlyer;
                if(flapTimer < 0){
                    spriteIsFlap = false;
                    flapTimer = flapAnimTimer;
                    }
            } else {
                sr.sprite = flyerFlap;
                if(flapTimer < 0){
                    spriteIsFlap = true;
                    flapTimer = flapAnimTimer;
                    }
            }
            if(!tooLowCeil){touchedGrass = false;} 
        } else {

            if(rb.velocity.y > 0){
                if(fleeing){
                    sr.sprite = flyerBackFlap;
                }
                else{
                    sr.sprite = flyerFlap;
                }
            }
            else if(rb.velocity.y < 0){
                if(fleeing){
                    sr.sprite = flyerBack;
                }
                else{
                    sr.sprite = defaultFlyer;
                }
                
            }
        }

        //delete if no health
        if(health < 0){Destroy(gameObject);}
        
        //check if in alert radius
        if(Vector3.Distance(target.transform.position, transform.position) < alertRadius){Activate();}


        //fireball timer, randomised
        if(alerted){
            if(fbTimer < 0){
                fbTimer = Random.Range(fbTimerFloor, fbTimerCeil);
                Fireball();

            }
            else if(sr.sprite == defaultFlyer || sr.sprite == flyerFlap){
                fbTimer -= Time.deltaTime;
            }
            else if(fbTimer > 2){
                fbTimer -= Time.deltaTime;
            }
        }
        

        
    }

    //take damage from bullets
    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "PlayerBullet"){
            health = health -20;
            Activate();
            //SpawnEnemy(enemy);
        }
    }

    public void Activate(){
        alerted = true;
    }

    /* TODO add initial spawn spiral animation when spawned by summoner enemy
    *
    *
    public void Init(){

    }
    */

    void Fireball(){
        //throw fireball at player
        Fireball fb = Instantiate(fbPrefab, fbEjector.position, transform.rotation);
        fb.Init(target); 
    }
}
