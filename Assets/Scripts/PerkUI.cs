using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUI : MonoBehaviour
{

    public List<PerkButton> perkButtons;

    void Awake(){
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
