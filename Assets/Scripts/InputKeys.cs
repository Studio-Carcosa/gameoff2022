using UnityEngine;
using System.Collections;

public class InputKeys : MonoBehaviour
{
    private static bool[] keys;
    private static bool[] pkeys;
    private static bool[] ukeys;

    private static readonly int NUM_KEYS = 14;
    public static readonly int UP = 0;
    public static readonly int LEFT = 1;
    public static readonly int DOWN = 2;
    public static readonly int RIGHT = 3;
    public static readonly int SHOOT = 4;
    public static readonly int SLOW = 5;
    public static readonly int WEP1 = 6;
    public static readonly int WEP2 = 7;
    public static readonly int WEP3 = 8;
    public static readonly int WEP4 = 9;
    public static readonly int ABL1 = 10;
    public static readonly int ABL2 = 11;
    public static readonly int ABL3 = 12;
    public static readonly int ABL4 = 13;

    void Awake(){
        keys = new bool[NUM_KEYS];
        pkeys = new bool[NUM_KEYS];
        ukeys = new bool[NUM_KEYS];
    }

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        // Sets pressed keys if the key is held down
        for(int i = 0; i < NUM_KEYS; i++){
            pkeys[i] = keys[i];
        }
        for(int i = 0; i < NUM_KEYS; i++){
            ukeys [i] = false;
        }
    }

    // Sets press status of key
    public static void setKey(int k, bool b){
        keys[k] = b;
    }

    // Is the key held down
    public static bool isDown(int k){
        return keys[k];
    }

    // Has the key just been pressed
    public static bool isPressed(int k){
        return keys[k] && !pkeys[k];
    }

    public static bool isUp(int k){
        return pkeys[k] && !keys[k];
    }
}