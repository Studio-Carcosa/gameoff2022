using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUI : MonoBehaviour
{
    private static PerkUI _instance;
    public static PerkUI Instance {get {return _instance;}}

    public List<PerkButton> perkButtons;
        private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        perkButtons = new List<PerkButton>(GetComponentsInChildren<PerkButton>());
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandPerks(){
        foreach (PerkButton p in perkButtons){
            Modifier m = Modifier.RandModifier();
            m.Init(GameManager.Instance.weapon);
            p.SetPerk(m);
        }
    }
    
    
}
