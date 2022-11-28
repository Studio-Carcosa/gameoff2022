using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Modifier
{

    public enum ModifierType{
        BIGGER_BULLETS,
        SHOT_AMOUNT,
        SHOT_DAMAGE,
        HEARTSCONTENT,
        RUN_MODIFIER
    }

    #pragma warning disable 0108
    // TODO: Add other references
    public string name;
    public string description;
    protected Weapon weapon;
    public Sprite button;

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
    public virtual void ApplyOnProjectile(Projectile p){} // TODO replace this with projectile script

    
    // NOT IMPLEMENTED YET
    

    // Applies once when the modifier is added
    public virtual void ApplyOnAttach(){}
    // Applies once when the modifier is removed
    public virtual void ApplyOnDetach(){} // Is this necessary?
    // Applies once to enemies when they spawn
    public virtual void ApplyToEnemyOnSpawn(){}
    // Applies continuously to enemies
    public virtual void ApplyToEnemyContinuous(){} // Is this necessary?

    public static T RandomEnumValue<T>() {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    public static Modifier RandModifier(){
        switch(RandomEnumValue<ModifierType>()){
            case ModifierType.BIGGER_BULLETS:
                return new BiggerBulletsModifier();
            case ModifierType.SHOT_AMOUNT:
                return new ShotAmountModifier();
            case ModifierType.SHOT_DAMAGE:
                return new ShotDamageModifier();
            case ModifierType.HEARTSCONTENT:
                return new HeartsContentModifier();
            case ModifierType.RUN_MODIFIER:
                return new RunModifier();                
        }
        return null;
    }
}
