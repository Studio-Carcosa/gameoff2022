using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player") {
            if (other.gameObject.GetComponent<PlayerHealth>().curHealth < other.gameObject.GetComponent<PlayerHealth>().maxHealth) {
            other.gameObject.GetComponent<PlayerHealth>().Heal(healAmount);
            Destroy(gameObject);
            }
        }
    }
}
