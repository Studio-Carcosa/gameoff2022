using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    //Basic statistics
    public float moveSpeed = 10;
    public float health = 10;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        curBurstTime = burstTime;
        curBurstCooldown = burstCooldown;
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(target.transform.position, transform.position);

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

        if (dist < alertDist) {
            isActive = true;
        }

        if (isActive) {
            transform.LookAt(target.transform);
            if (!hasAlert){
            //Debug.Log("KAISER HAS AWOKEN!!!!!");
            hasAlert = true;
            }
            if (isBurst) {
            //Debug.Log("Bursting Now");
            //rb.velocity = -target.transform.position * moveSpeed;
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (moveSpeed * Time.deltaTime));
            curBurstTime = curBurstTime - Time.deltaTime;
            rb.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Force);
            }
        }
    }
}
