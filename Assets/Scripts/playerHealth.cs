using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public float invulnTime;

    public GameManager GM;

    public int curHealth;
    private float curInvulnTime;
    private bool isDying = false;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        curHealth = maxHealth;
        curInvulnTime = invulnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0){
            if (!isDying){
            isDying = true;
            playerDeath();
            }
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

    public void FullHeal() {
        curHealth = maxHealth;
        Debug.Log("Full Heal, health is now " +curHealth);
    }
    
    public void Heal(int amount) {
        curHealth += amount;
            if (curHealth > maxHealth) {
            curHealth = maxHealth;
        }
        Debug.Log("Partial Heal, health is now " + curHealth);
    }

    bool canHurt(){
        return curInvulnTime >= invulnTime; 
    }
    void playerDeath(){
        Debug.Log("YOU DIED!");
        GM.GetComponent<GunQuoteManager>().DieQuote();
        Debug.Log("Dying");
        StartCoroutine(waiter());
    }

    IEnumerator waiter() {
        yield return new WaitForSeconds(7);
        GM.Restart();
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Fireball"){
            Hurt(15);
        }
    }
}
