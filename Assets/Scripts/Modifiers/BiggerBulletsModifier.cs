using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerBulletsModifier : Modifier
{

    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        name = "Mr Big Shot";
        description = "Bigger Bullets!";
    }

    public override void ApplyOnProjectile(Projectile p)
    {
        p.transform.localScale *= 40;
    }
}
