using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    public Animator anim;
    int shotgunFire = Animator.StringToHash("shotgunFire");
    int shotgunReload = Animator.StringToHash("shotgunReload");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && anim.GetCurrentAnimatorStateInfo(0).IsName("ShotgunReady"))
        {
            shoot();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
    }

    void shoot()
    {
        anim.SetTrigger (shotgunFire);
    }

    void reload()
    {
        anim.SetTrigger (shotgunReload);
        //anim.ResetTrigger(shotgunReload);
    }
}
