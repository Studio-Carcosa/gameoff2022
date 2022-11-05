using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerBulletsModifier : Modifier
{
    public override void ApplyOnProjectile(GameObject foo)
    {
        foo.transform.localScale *= 2;
    }
}
