using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsContentModifier : Modifier
{   


    private PlayerHealth playerHealth;
    
    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        name = "Heart's Content";
        description = "+100 Max Health and Full Heal!";
    }

    public override void ApplyOnAttach()
    {
        base.ApplyOnAttach();
        playerHealth.maxHealth += 100;
        playerHealth.FullHeal();
    }
}
