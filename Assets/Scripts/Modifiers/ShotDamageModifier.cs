using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDamageModifier : Modifier
{
    public override void Init(Weapon weaponReference){
        base.Init(weaponReference);
        name = "Silver Bullet";
        description = "More Damage!";
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ApplyOnProjectile(Projectile p)
    {
        p.damage += 20;
    }
}
