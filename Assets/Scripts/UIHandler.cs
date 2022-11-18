using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Don't forget this line


public class UIHandler : MonoBehaviour
{
    public Weapon weapon;
    public Text counterText;
    
    public void Update()
    {
       counterText.text =  weapon.shellCount + " / " + weapon.maxShellCount;
    }
}
