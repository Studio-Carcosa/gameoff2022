using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier
{
    #pragma warning disable 0108
    // TODO: Add other references
    public string name;
    protected Weapon weapon;

    public virtual void Init(Weapon weaponReference){
        weapon = weaponReference;
    }

    // Most Modifiers probably wont need this
    public virtual void Update() {
        
    }

    // Applied when shooting, before the projectiles are instantiated
    public virtual void ApplyOnShot(){}
    // Applied when shooting, after the projectiles are instantiated
    public virtual void ApplyAfterShot(){}
    // Applies to the projectiles that have been instantiated
    public virtual void ApplyOnProjectile(GameObject foo){} // TODO replace this with projectile script

    
    // NOT IMPLEMENTED YET
    

    // Applies once when the modifier is added
    public virtual void ApplyOnAttach(){}
    // Applies once when the modifier is removed
    public virtual void ApplyOnDetach(){} // Is this necessary?
    // Applies once to enemies when they spawn
    public virtual void ApplyToEnemyOnSpawn(){}
    // Applies continuously to enemies
    public virtual void ApplyToEnemyContinuous(){} // Is this necessary?
}
