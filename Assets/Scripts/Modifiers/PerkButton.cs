using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkButton : MonoBehaviour
{

    public Modifier.ModifierType currModifierType;
    private TMP_Text nameText;
    private TMP_Text descText;
    private Modifier currModifier;
    public Sprite sprite;

    void Awake(){
        nameText = transform.GetChild(0).GetComponent<TMP_Text>();
        descText = transform.GetChild(1).GetComponent<TMP_Text>();
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
        Debug.Log ("You picked: " + currModifier.name);
        GameManager.Instance.modifiers.Add(currModifier);
        currModifier.ApplyOnAttach();
        GameManager.Instance.DisablePerkScreen();
    }

    public void SetPerk(Modifier m){
        currModifier = m;
        nameText.text = currModifier.name;
        descText.text = currModifier.description;
        sprite = currModifier.button;
    }
}
