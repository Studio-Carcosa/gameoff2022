using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDelayModifier : Modifier
{

    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        name = "Shot Delay Modifier";
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ApplyAfterShot(){
        weapon.shotTimer /= 2;
    }
}
