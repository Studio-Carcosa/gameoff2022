using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySummoner : MonoBehaviour
{


    //TODO make this mess actually readable
    [Header("stats")]
    public float health = 2000f;
    public float moveSpeed = 14f;

    [Header("Spawn stats")]
    public float flyerSpawnRadius = 12f;
    public int flyerMinSpawn = 3;
    public int flyerMaxSpawn = 7;
    public static float flyerSpawnTimer = 7f;
    private float flyerTimer = flyerSpawnTimer;
    public EnemyFlyer flyer;
    List<EnemyFlyer> flyers = new List<EnemyFlyer>();
    public BasicZombie zomb;
    List<BasicZombie> zombs = new List<BasicZombie>();
    public static float zombMinTimer = 5f;
    public static float zombMaxTimer = 10f;
    public int zombMaxSpawns = 7;
    private float zombTimer;
    Transform fbEjector;


    [Header("sprite timer stats")]
    public  static float stepAnimTimer = 0.5f;
    public static float summonAnimTimer = 2f;
    private bool isStep = false;
    private float stepTimer = stepAnimTimer;
    private float sumTimer = summonAnimTimer;


    public float alertRadius = 65f;



    [System.NonSerialized]
    public GameObject target;
    private Rigidbody rb;
    //List<GameObject> flyers = new List<GameObject>();
    //List<GameObject> zombies = new List<GameObject>();
    Transform flyerSpawnReference;
    Transform zombSpawnReference;
    private bool alerted = false;
    private bool step = false;
    private bool summoning;
    private bool summoned = false;
    private bool sumZombies, canSumZombs;
    private bool sumFlyers, canSumFlyers;

    public Sprite defaultSummoner;
    public Sprite alertedSummoner;
    //public Sprite defaultSummonerStep;
    //public Sprite fleeingSummoner;
    //public Sprite fleeingSummonerStep;
    public Sprite summoningSummoner;
    [System.NonSerialized]
    public SpriteRenderer sr;

    public GameObject deathObject;

    public GameObject expOrb;
    public int expDrop = 5;
    public AudioClip sumSound;
    public AudioSource ac;

    // Start is called before the first frame update
    void Start()
    {
        target =  GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        sr = this.GetComponent<SpriteRenderer>();
        flyerSpawnReference = this.gameObject.transform.GetChild(0);
        zombSpawnReference = this.gameObject.transform.GetChild(1);
        canSumFlyers = true;
        canSumZombs = true;
        zombTimer = Random.Range(zombMinTimer, zombMaxTimer);
        ac = gameObject.GetComponent<AudioSource>();
        fbEjector = this.gameObject.transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        //determine if summoner needs to summon minions
        flyers.RemoveAll(flyer => flyer == null);
        if(flyers.Count == 0){sumFlyers = false;}
        if(zombs.Count < zombMaxSpawns){sumZombies = false;}
        else{sumZombies = true;}
        if(!sumFlyers && canSumFlyers){summoning = true;}

        if(!sumFlyers){
            flyerTimer -= Time.deltaTime;
            if(flyerTimer < 0){
                canSumFlyers = true;
                flyerTimer = flyerSpawnTimer;
            }
        }

        if(!sumZombies && !canSumZombs){
            if(zombTimer < 0){
                zombTimer = Random.Range(zombMinTimer, zombMaxTimer);
                canSumZombs = true;
            }
            zombTimer -= Time.deltaTime;
        }

        //look at player
        Vector3 p = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(p);
        transform.forward = -transform.forward;

        //bool fleeing = false;


            //TODO add idle animations when not alerted

        if(alerted && !summoning){

            //keep enemy within goldilocks zone
            if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < moveSpeed){
                if(Vector3.Distance(target.transform.position, transform.position) > alertRadius - 20f){
                    rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);
                    //fleeing = false;
                } else if(Vector3.Distance(target.transform.position, transform.position) < alertRadius - 40f){
                    rb.AddForce(transform.forward * moveSpeed, ForceMode.Force);
                    //fleeing = true;
                }
            }
        }

        //handle detect movement for sprite animation
        /*if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > 0.1 && !summoning){
            Animate(fleeing);
        }*/



        //summon minions and show summon anim when summoning
        if(summoning && alerted){
            if(!summoned){
                Summon();
                ac.clip = sumSound;
                ac.Play();
                summoned = true;
                sr.sprite = summoningSummoner;
            }
            else if(sumTimer >= 0){
                //freeze summoner in place
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                sr.sprite = summoningSummoner;
                sumTimer -= Time.deltaTime;
            }
            else if(sumTimer < 0){
                sumTimer = summonAnimTimer;
                summoning = false;
                summoned = false;
                sr.sprite = alertedSummoner;
            }
        }

        //delete if no health
        if(health < 0){
            Die();
            }
        
        //check if in alert radius
        if(Vector3.Distance(target.transform.position, transform.position) < alertRadius && !alerted){Activate();}

        if(!sumZombies && canSumZombs){
            SummonZombies();
        }
    }

    void Activate(){
        alerted = true;
        sr.sprite = alertedSummoner;
    }

    //summon x amount of flyer minions
    void SummonFlyers(int num){
        Debug.Log("FLYERS");
        float angle = 360 / num;
        for(int i = 0; i < num; i++){
            flyers.Add(Instantiate(flyer, flyerSpawnReference.position, transform.rotation));
            flyers[i].Activate();
            flyerSpawnReference.transform.RotateAround(transform.position, transform.up, angle);
        }
        sumFlyers = true;
        canSumFlyers = false;
    }

    void SummonZombies(){
        Debug.Log("ZOMBIES");
        sumZombies = true;
        canSumZombs = false;
        float pos = Random.Range(0, 360);
        zombSpawnReference.transform.RotateAround(transform.position, transform.up, pos);
        zombs.Add(Instantiate(zomb, zombSpawnReference.position, transform.rotation));
        zombs.Last().Activate();
    }

    //summon minions
    void Summon(){
        /*if(!sumZombies && !sumFlyers){
            int sumRollZomb = Random.Range(0, 1);
            if(sumRollZomb == 1){
                SummonZombies();
            }
            else if(canSumFlyers){
                SummonFlyers(Random.Range(flyerMinSpawn, flyerMaxSpawn));
            }
        }
        if(!sumZombies && canSumZombs){
            SummonZombies();
        }
        else */if(!sumFlyers && canSumFlyers){
            SummonFlyers(Random.Range(flyerMinSpawn, flyerMaxSpawn));
        }
    }

    //handle sprite animations
    /*void Animate(bool flee){
        stepTimer -= Time.deltaTime;
        if(!flee){
            /*if(stepTimer < 0 && sr.sprite != defaultSummonerStep){
                stepTimer = stepAnimTimer;
                sr.sprite = defaultSummonerStep;
            }
            else if(stepTimer < 0 && sr.sprite != !defaultSummoner){
                stepTimer = stepAnimTimer;
                sr.sprite = defaultSummoner;
            }
        }
        else{
            if(stepTimer < 0 && sr.sprite != fleeingSummonerStep){
                stepTimer = stepAnimTimer;
                sr.sprite = fleeingSummonerStep;
            }
            else if(stepTimer < 0 && sr.sprite != fleeingSummoner){
                stepTimer = stepAnimTimer;
                sr.sprite = fleeingSummoner;
            }
        }
    }*/

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "PlayerBullet"){
            health = health -20;
            Activate();
        }
    }

        public void Die() {
        for (int i = 0; i < expDrop; i++){
        Instantiate(expOrb, fbEjector.position, transform.rotation);
        }
        Instantiate(deathObject, fbEjector.position, transform.rotation);
        Destroy(gameObject);
    }
}
