using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{ 
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        player.Spawn(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
