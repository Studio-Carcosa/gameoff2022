using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float leftMax;
    public float rightMax;
    public float bobSpeed;
    private float originalBobSpeed;
    public float current = 0;
    private GameObject sprite;
    private float posBobSpeed;
    private float negBobSpeed;
    public KeyCode sprintKey = KeyCode.LeftShift;
    // Start is called before the first frame update
    void Start()
    {
        sprite = this.gameObject;
        posBobSpeed = bobSpeed;
        negBobSpeed = (bobSpeed * -1);
        originalBobSpeed = bobSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (current > rightMax) {
            bobSpeed = negBobSpeed;
        }
        else if (current < -leftMax) {
            bobSpeed = posBobSpeed;
        }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
        current = current + (bobSpeed * Time.deltaTime);
        sprite.transform.position += transform.right * current * Time.deltaTime;
        }
    }

}
