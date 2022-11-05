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
    public float recoil = 0.05f;
    [System.NonSerialized]
    public float kickBack; // TODO

    void Awake(){
        bulletPort = this.gameObject.transform.GetChild(0);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        }else{
            DecrementshotTimer();
        }
    }

    private void Shoot(){
        // Generate projectiles
        for (int i = 0; i < shotAmount; i++){
            GameObject b = Instantiate(bulletPrefab, bulletPort.position, Quaternion.identity) as GameObject;
                // FIXME: encapsulate in function?
                // Apply projectile modifiers
                foreach(Modifier m in GameManager.Instance.modifiers){
                    m.ApplyOnProjectile(b); // TODO: pass in the projectile script when we have one
                }
            b.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward + GenerateRecoil()) * 100.0f, ForceMode.Impulse);
        }

        // Apply default values
        shotTimer = shotDelay;
    }

    private void DecrementshotTimer(){
        shotTimer -= Time.deltaTime;
    }

    private bool CanShoot(){
        return shotTimer <= 0;
    }

    private Vector3 GenerateRecoil(){       
        return new Vector3(Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil), 
                           Random.Range(-recoil,recoil));
    }
}
