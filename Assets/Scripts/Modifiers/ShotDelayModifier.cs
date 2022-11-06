using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDelayModifier : Modifier
{

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ApplyAfterShot(){
        weapon.shotTimer /= 2;
    }
}
