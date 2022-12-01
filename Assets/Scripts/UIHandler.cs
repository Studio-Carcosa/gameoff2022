using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Don't forget this line


public class UIHandler : MonoBehaviour
{
    public PlayerHealth health;
    public Text counterText;
    
    private void Awake(){
    DontDestroyOnLoad(this.gameObject);
    }
    public void Start() {
        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
    public void Update()
    {
       counterText.text =  health.curHealth.ToString();
    }
}
