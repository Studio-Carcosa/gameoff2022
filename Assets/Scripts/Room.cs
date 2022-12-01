using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameManager gameManager;
    [System.NonSerialized]
    public GameObject player;
    [System.NonSerialized]
    public bool isBossRoom = false;
    public Enemy[] enemyList;
    private Bounds bounds;
    private bool isClear = false;
    public SpawnPoints spawns;
    public int spawnAmountLow = 0;
    public int spawnAmountHigh = 2;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !isClear) {
            //Debug.Log("Player has entered a room!");
            //lock room
            SpawnEnemies();
        }
    }

    void SpawnEnemies() {
        foreach (Enemy enemy in enemyList){
            int amountSpawn = Random.Range(spawnAmountLow, spawnAmountHigh);
            //Debug.Log("Amount spawned in this room is " + amountSpawn);
            for(int i = amountSpawn; i > 0; i--) {
                //Debug.Log("First for done");
                foreach (Transform curSpawn in spawns.spawnPoints) {
                    if (amountSpawn > 0){
                    //Debug.Log("Instantiating Enemy");   
                    Instantiate(enemy, curSpawn.position, Quaternion.identity);
                    amountSpawn--;
                    }
                }
            }
            isClear = true; 
            //For now, this simply tracks if a room has had enemies spawned in it before, otherwise we could do a BOI style system
            //where enemies will respawn if a room is exited before it has been cleared

        }
    }
}
