using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Projectile bulletPrefab;
    private GameObject player;
    private GameObject playerCamera;

    Transform bulletPort;
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
    public float bulletForce = 200;
    public int maxAmmo = 100;
    public int maxShellCount = 2;
    public int shellCount;
    public int curAmmo;

    // Animation properties
    private Animator anim;
    int shotgunFire = Animator.StringToHash("shotgunFire");
    int shotgunReload = Animator.StringToHash("shotgunReload");

    // Sounds
    private AudioSource audio;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    // Camera recoil object
    public GameObject cameraRecoil;
    public float playerKnockback = 1;

    void Awake(){
        bulletPort = this.gameObject.transform.GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = player.transform.GetChild(0).gameObject;
        anim = GetComponentInChildren(typeof(Animator)) as Animator;
        audio = GetComponent<AudioSource>();
        shellCount = maxShellCount;
    }

    void Start(){
        curAmmo = 30;

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
        //player.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);   -- no workie
        // Generate projectiles
        for (int i = 0; i < shotAmount; i++){
            Projectile p = Instantiate(bulletPrefab, bulletPort.position, Quaternion.identity) as Projectile;
            p.Init((Camera.main.transform.forward + GenerateRecoil()));
            // FIXME: encapsulate in function?
            // Apply projectile modifiers
            foreach(Modifier m in GameManager.Instance.modifiers){
                m.ApplyOnProjectile(p); // TODO: pass in the projectile script when we have one
            }
            AddRecoil();
        }

        // Apply default values
        shotTimer = shotDelay;
        // Reduce ammo count
        shellCount--;
    }

    private void Melee(){
        //TODO: Make melee lol
    }

    private void Reload(){
        anim.SetTrigger(shotgunReload);
        audio.clip = reloadSound;
        audio.Play();
        shellCount = 2;
        curAmmo -= 2; //You always throw out two shells!
        Debug.Log("Ammo Remaining: " + curAmmo);
    }

    private void DecrementshotTimer(){
        shotTimer -= Time.deltaTime;
    }

    private void AddRecoil(){
        // playerCamera.GetComponent<CameraController>().cameraRecoil(recoilXaxis, recoilRecovery);
        cameraRecoil.GetComponent<Recoil>().RecoilFire();
        player.GetComponent<PlayerMovement>().recoilKnockback(playerKnockback);

    }

    private bool CanShoot(){
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("ShotgunReload")){
        return shotTimer <= 0 && shellCount > 0;
        }
        else return false;
    }

    private bool CanReload(){
        return shellCount <2 && anim.GetCurrentAnimatorStateInfo(0).IsName("ShotgunReady") &&  curAmmo > 0;
        //Using animations as locks is not a great idea
    }

    private Vector3 GenerateRecoil(){       
        return new Vector3(Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil));
    }
}
