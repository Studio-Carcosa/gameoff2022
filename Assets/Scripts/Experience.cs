using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int curExp;
    public int nextLevel;
    public GameManager GM;
    public Weapon gun;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        gun = GameObject.FindWithTag("Player").GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curExp >= nextLevel){
            LevelUp();
        }
    }

    public void AddExp(int exp) {
        curExp += exp;
    }

    void LevelUp() {
        curExp -= nextLevel;
        gun.curAmmo = gun.maxAmmo;
        nextLevel = (nextLevel + (nextLevel /2));
        GM.EnablePerkScreen();
    }
}
