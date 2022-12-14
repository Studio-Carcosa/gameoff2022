using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// README:
// Access the instance and its properties through GameManager.Instance

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}
    public List<Modifier> modifiers;
    public PerkUI perkUI;
    public Weapon weapon;
    public int gameLevel = 1; //The level the game is currently at
    public bool bigShot = false;


    private CameraController playerCameraController;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        perkUI = GameObject.FindGameObjectWithTag("PerkCanvas").GetComponent<PerkUI>();
        weapon = GameObject.FindObjectOfType<Weapon>();
        playerCameraController = Camera.main.gameObject.GetComponent<CameraController>();
        DisablePerkScreen();
    }

    void Start(){
        modifiers = new List<Modifier>();
    }

    void Update(){
        foreach(Modifier m in modifiers){
            m.Update();
        }
        // CODE FOR TESTING
        if(Input.GetKeyDown("p")){
            if(perkUI.gameObject.activeInHierarchy){
                DisablePerkScreen();
            }else{
                EnablePerkScreen();
            }
        }
    }

    public void EnablePerkScreen(){
        perkUI.gameObject.SetActive(true);
        perkUI.RandPerks();
        playerCameraController.enabled = false;
        weapon.active = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void DisablePerkScreen(){
        perkUI.gameObject.SetActive(false);
        playerCameraController.enabled = true;
        weapon.active = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    
    public void Restart() {
         foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
            if (o.name != "GameManager"){
             Destroy(o);
            }
         }
         SceneManager.LoadScene("Death");
     }

}
