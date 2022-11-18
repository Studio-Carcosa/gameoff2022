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
    public int gameLevel = 1; //The level the game is currently at


    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
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
        
    }
}
