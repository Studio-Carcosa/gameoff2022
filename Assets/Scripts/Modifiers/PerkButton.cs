using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkButton : MonoBehaviour
{

    public Modifier.ModifierType currModifierType;
    private TMP_Text text;
    private Modifier currModifier;

    void Awake(){
        text = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton(){
        currModifier.Init(GameManager.Instance.weapon);
        GameManager.Instance.modifiers.Add(currModifier);

        GameManager.Instance.DisablePerkScreen();
    }

    public void SetPerk(Modifier m){
        currModifier = m;
        text.text = currModifier.name;
    }
}
