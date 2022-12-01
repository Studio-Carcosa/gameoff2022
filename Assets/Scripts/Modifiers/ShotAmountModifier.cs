using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAmountModifier : Modifier
{   

    private int startingShotAmount;
    
    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        name = "More Where That Came From";
        description = "More pellets!";
    }

    public override void ApplyOnShot(){
        startingShotAmount = weapon.shotAmount;
        weapon.shotAmount = (int) (weapon.shotAmount * 1.5f);
    }

    public override void ApplyAfterShot(){
         weapon.shotAmount = (int) (weapon.shotAmount / 1.5f);
    }
}
