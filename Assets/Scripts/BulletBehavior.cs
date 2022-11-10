using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    //The decay time of the bullet
    public float decayTime; // The time the bullet takes before it disappears
    private float curDecayTime;
    // Start is called before the first frame update
    void Start()
    {
        curDecayTime = decayTime;
    }

    // Update is called once per frame
    void Update()
    {
       curDecayTime -= Time.deltaTime; 
       if (curDecayTime <= 0) {
        Destroy(gameObject);
       }
    }

    // Make a projectile decay faster if it hits anything, also can be used to make sounds and bullet holes later
    void OnCollisionEnter() {
        curDecayTime = curDecayTime - 1;
    }
}
