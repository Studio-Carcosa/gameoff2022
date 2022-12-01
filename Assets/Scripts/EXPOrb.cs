using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPOrb : MonoBehaviour
{
    public int expValue = 10;
    public float radius = 10f;
    public float moveSpeed = 10;
    private Rigidbody rb;
    private float dist;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < radius) {
            Vector3 p = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(p);
            rb.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Force);
            
        }
    }
        void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponentInChildren<Experience>().AddExp(expValue);
            Destroy(this.gameObject);
        }
    }
}
