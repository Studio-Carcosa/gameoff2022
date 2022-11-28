using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunModifier : Modifier
{   


    private PlayerMovement playerMovement;
    
    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        name = "Run For Your Money";
        description = "Movement speed increase!";
    }

    public override void ApplyOnAttach()
    {
        base.ApplyOnAttach();
        playerMovement.moveSpeed += 5f;
        playerMovement.maxSpeed += 7f;
    }
}
