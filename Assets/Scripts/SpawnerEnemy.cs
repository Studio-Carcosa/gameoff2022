using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{

    //Basic statistics
    public float moveSpeed = 10;
    public float health = 100;

    //Distance target needs to be before activates
    public float alertDist = 15;

    //How long between bursts, and how long the burst lasts
    public float burstCooldown = 10;
    public float burstTime = 1;

    //Is the enemy actively chasing the target?
    private bool isActive = false;
    private bool hasAlert = false;

    //Target AI will chase
    public GameObject target;
    
    private Rigidbody rb;
    private float curBurstTime;
    private float curBurstCooldown;
    private bool isBurst = true;

    //Enemy to be spawned
    public GameObject spawnMinion;
    public int spawnAmount = 8;

    //Spawn Times
    public float spawnTime = 4;
    public float spawnCooldown = 10;
    private float curSpawnTime = 0;
    private float curSpawnCooldown = 0;

    //
    public float spawnPause = 2;
    [System.NonSerialized]
    private float curSpawnPause = 0;

    //Enemy Frames
    public Sprite defaultFace;
    public Sprite angryFace;
    public Sprite spawningFace;
    public Sprite surprisedFace;
    public Sprite deadFace;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        curBurstTime = burstTime;
        curBurstCooldown = burstCooldown;
        target = GameObject.FindWithTag("Player");
        spriteRenderer = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        curSpawnCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //This is a pretty bad state machine, this code needs cleaning
        float dist = Vector3.Distance(target.transform.position, transform.position);

        if (health <= 0){
            Debug.Log("BIG BOY has gone to bed!");
            Destroy(gameObject);
        }

        if (curBurstTime <= 0) {
            curBurstCooldown = 0;
            //Debug.Log("Burst Ended");
            isBurst = false;
            curBurstTime = burstTime;

        }
        if (!isBurst){
            if (curBurstCooldown < burstCooldown) {
                curBurstCooldown = curBurstCooldown + Time.deltaTime;
            }
        }
        if (curBurstCooldown >= burstCooldown) {
            //Debug.Log("Burst Refreshed");
            isBurst = true;
        }

        //Check if the player is within distance
        if (dist < alertDist) {
            Activate();
        }

        if (isActive) {
            transform.LookAt(target.transform);
            if (isBurst) {
            curBurstTime = curBurstTime - Time.deltaTime;
            rb.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Force);
            }
            if (curSpawnCooldown >= spawnCooldown) {
                SpawnMode();
            }
            else {
                curSpawnCooldown += Time.deltaTime;
                Debug.Log("curSpawnCooldown = " + curSpawnCooldown);
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
    void SpawnMode(){
        Debug.Log("Spawn TIME!");
        //Make him inactive for a moment while he spawns enemies
        isActive = false;
        //Make him ANGRY
        spriteRenderer.sprite = spawningFace;
        //Make him shake left to right TODO
        curSpawnTime = 0;
        while (curSpawnTime < spawnTime) {
            curSpawnTime += Time.deltaTime;
        }
        for (int i = 0; i < spawnAmount; i++) {
        SpawnEnemy(spawnMinion);
        }
        Debug.Log("Finished spawning!");
        //Do a lil surprised face so he looks like he sharted
        spriteRenderer.sprite = surprisedFace;
        curSpawnPause = 0;
        float saveMoveSpeed = moveSpeed;
        while (curSpawnPause < spawnPause){
            Debug.Log("curSpawnPause = " + curSpawnPause);
            curSpawnPause+= Time.deltaTime;
            moveSpeed = 0;
            isActive = false;
            isBurst = false;
        }
        //Back to normal now
        isActive = true;
        spriteRenderer.sprite = defaultFace;
        curSpawnCooldown = 0;
        curSpawnTime = 0;
    }
    void SpawnEnemy(GameObject enemy){
        GameObject spawn = Instantiate(enemy, this.transform.position, this.transform.rotation);
        //Make sure spawned enemies are activated
        if (spawn.GetComponent<BasicEnemy>()) {
            spawn.GetComponent<BasicEnemy>().Activate();
        }
    }

    //Use when the player has done something to piss him off
    void Activate(){
        isActive = true;
        defaultFace = angryFace;
        curSpawnTime = 0;
        //spriteRenderer.sprite = defaultFace;
    }

    void Die() {
        isActive = false;
        spriteRenderer.sprite = deadFace;
        float deadWait = 0;
        while (deadWait < 10) {
            deadWait += Time.deltaTime;
        }
    }
}
