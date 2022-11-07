using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public int maxHealth;
    public float invulnTime;


    private int curHealth;
    private float curInvulnTime;
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        curInvulnTime = invulnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0){
            playerDeath();
        } else if (curInvulnTime < invulnTime) {
            curInvulnTime += Time.deltaTime;
            Debug.Log("Health is" + curHealth + " Current cooldown is " + curInvulnTime);
        }
    }

    public void Hurt(int damage){
        if (canHurt()){
        curHealth = curHealth - damage;
        curInvulnTime = 0;
        }
    }
    bool canHurt(){
        return curInvulnTime >= invulnTime; 
    }
    void playerDeath(){
        Debug.Log("YOU DIED!");
    }
}
