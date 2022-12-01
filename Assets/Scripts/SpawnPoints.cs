using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoints : MonoBehaviour
{
    public IEnumerable<Transform> spawnPoints => GetComponentsInChildren<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        //spawnPoints = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
