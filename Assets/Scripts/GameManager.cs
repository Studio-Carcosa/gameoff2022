using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// README:
// Access the instance and its properties through GameManager.Instance

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}
    public List<Modifier> modifiers;
    public PerkUI perkUI;
    public int gameLevel = 1; //The level the game is currently at


    private CameraController playerCameraController;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        perkUI = GameObject.FindGameObjectWithTag("PerkCanvas").GetComponent<PerkUI>();
        playerCameraController = Camera.main.gameObject.GetComponent<CameraController>();
        DisablePerkScreen();
    }

    void Start(){
        modifiers = new List<Modifier>();

        ShotDelayModifier foo = new ShotDelayModifier();
        foo.Init(FindObjectOfType<Weapon>());
        modifiers.Add(foo);

        BiggerBulletsModifier bar = new BiggerBulletsModifier();
        bar.Init(FindObjectOfType<Weapon>());
        modifiers.Add(bar);
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
        playerCameraController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisablePerkScreen(){
        perkUI.gameObject.SetActive(false);
        playerCameraController.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
