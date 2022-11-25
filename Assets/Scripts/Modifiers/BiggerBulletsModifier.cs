using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerBulletsModifier : Modifier
{

    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        name = "Bigger Bullets Modifier";
    }

    public override void ApplyOnProjectile(Projectile p)
    {
        p.transform.localScale *= 2;
    }
}
