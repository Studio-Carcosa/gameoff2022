using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    Transform bulletPort;
    Transform playerTransform;
    [System.NonSerialized]
    public int shotAmount = 20;
    [System.NonSerialized]
    public float shotDelay = 1.0f;
    [System.NonSerialized]
    public float shotTimer = 0.0f;
    [System.NonSerialized]
    public float reloadDelay = 1.0f;
    [System.NonSerialized]
    public float reloadTimer = 0.0f;
    [System.NonSerialized]
    public float recoil = 0.05f;
    [System.NonSerialized]
    public float kickBack; // TODO
    public float bulletForce = 200;
    public int maxShellCount = 2;
    private int shellCount;

    // Animation properties
    private Animator anim;
    int shotgunFire = Animator.StringToHash("shotgunFire");
    int shotgunReload = Animator.StringToHash("shotgunReload");

    // Sounds
    private AudioSource audio;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    void Awake(){
        bulletPort = this.gameObject.transform.GetChild(0);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = this.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        shellCount = maxShellCount;
    }

    void Start(){

    }

    void Update(){
        // FIXME: encapsulate in function?
        foreach(Modifier m in GameManager.Instance.modifiers){
            m.Update();
        }
        
        if(CanShoot() && Input.GetMouseButton(0)){
            // Apply Pre shot modifiers
            foreach(Modifier m in GameManager.Instance.modifiers){
                m.ApplyOnShot();
            }
           
            Shoot();

            // Apply Post shot modifiers
            foreach(Modifier m in GameManager.Instance.modifiers){
                m.ApplyAfterShot();
            }
        }else if(CanReload() && Input.GetKeyDown("r")){
                Reload();
        }else{
            DecrementshotTimer();
        }
    }

    private void Shoot(){
        // Animate Gun
        anim.SetTrigger(shotgunFire);
        // Make sound
        audio.clip = fireSound;
        audio.Play();
        // Generate projectiles
        for (int i = 0; i < shotAmount; i++){
            GameObject b = Instantiate(bulletPrefab, bulletPort.position, Quaternion.identity) as GameObject;
                // FIXME: encapsulate in function?
                // Apply projectile modifiers
                foreach(Modifier m in GameManager.Instance.modifiers){
                    m.ApplyOnProjectile(b); // TODO: pass in the projectile script when we have one
                }
            b.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward + GenerateRecoil()) * bulletForce, ForceMode.Impulse);
        }

        // Apply default values
        shotTimer = shotDelay;
        // Reduce ammo count
        shellCount--;
    }

    private void Reload(){
        anim.SetTrigger(shotgunReload);
        audio.clip = reloadSound;
        audio.Play();
        shellCount = 2;
    }

    private void DecrementshotTimer(){
        shotTimer -= Time.deltaTime;
    }

    private bool CanShoot(){
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("ShotgunReload")){
        return shotTimer <= 0 && shellCount > 0;
        }
        else return false;
    }

    private bool CanReload(){
        return shellCount <2;
    }

    private Vector3 GenerateRecoil(){       
        return new Vector3(Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil));
    }
}
