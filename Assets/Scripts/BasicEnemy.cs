using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private bool isActive = false;
    private bool hasAlert = false;
    public float moveSpeed = 10;
    public GameObject player;
    public float alertDist = 15;
    public bool isBurst = true;
    public float burstCooldown = 10;
    public float burstTime = 1;

    private float curBurstTime;
    private float curBurstCooldown;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        curBurstTime = burstTime;
        curBurstCooldown = burstCooldown;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

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
            transform.LookAt(player.transform);
            if (!hasAlert){
            //Debug.Log("KAISER HAS AWOKEN!!!!!");
            hasAlert = true;
            }
            if (isBurst) {
            //Debug.Log("Bursting Now");
            //rb.velocity = -player.transform.position * moveSpeed;
            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed * Time.deltaTime));
            curBurstTime = curBurstTime - Time.deltaTime;
            rb.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Force);
            }
        }
    }
}
