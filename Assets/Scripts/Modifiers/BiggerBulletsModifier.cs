using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerBulletsModifier : Modifier
{
    public override void ApplyOnProjectile(Projectile p)
    {
        p.transform.localScale *= 2;
    }
}
