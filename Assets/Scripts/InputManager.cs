using UnityEngine;
using System.Collections;
/***
 * THIS CLASS CURRENTLY ONLY HAS HARD CODED KEYS 
 */
public class InputManager : MonoBehaviour
{
	KeyCode UP, DOWN, LEFT, RIGHT, SHOOT, SLOW, WEP1, WEP2, WEP3, WEP4, ABL1, ABL2, ABL3, ABL4;
	// Use this for initialization
	void Start ()
	{
		UP = KeyCode.W;
		DOWN = KeyCode.S;
		LEFT = KeyCode.A;
		RIGHT = KeyCode.D;
		SHOOT = KeyCode.Mouse0;
		SLOW = KeyCode.LeftShift;
		WEP1 = KeyCode.Alpha1;
		WEP2 = KeyCode.Alpha2;
		WEP3 = KeyCode.Alpha3;
		WEP4 = KeyCode.Alpha4;
        ABL1 = KeyCode.Mouse1;
        ABL2 = KeyCode.Q;
        ABL3 = KeyCode.E;
        ABL4 = KeyCode.R;


		//UP = KeyCode.UpArrow;
		//DOWN = KeyCode.DownArrow;
		//LEFT = KeyCode.LeftArrow;
		//RIGHT = KeyCode.RightArrow;
	}

	// Update is called once per frame
	void Update ()
	{
		// DOWN
		if (Input.GetKeyDown (UP)) {
			InputKeys.setKey (InputKeys.UP, true);
			InputKeys.setKey (InputKeys.DOWN, false);
		}
		if (Input.GetKeyDown (LEFT)) {
			InputKeys.setKey (InputKeys.LEFT, true);
			InputKeys.setKey (InputKeys.RIGHT, false);
		}
		if (Input.GetKeyDown (DOWN)) {
			InputKeys.setKey (InputKeys.DOWN, true);
			InputKeys.setKey (InputKeys.UP, false);
		}
		if (Input.GetKeyDown (RIGHT)) {
			InputKeys.setKey (InputKeys.RIGHT, true);
			InputKeys.setKey (InputKeys.LEFT, false);
		}
		if(Input.GetKeyDown(SHOOT)){
			InputKeys.setKey (InputKeys.SHOOT, true);
		}
		if(Input.GetKeyDown(SLOW)){
			InputKeys.setKey (InputKeys.SLOW, true);
		}
		if(Input.GetKeyDown(WEP1)){
			InputKeys.setKey (InputKeys.WEP1, true);
		}
		if(Input.GetKeyDown(WEP2)){
			InputKeys.setKey (InputKeys.WEP2, true);
		}
		if(Input.GetKeyDown(WEP3)){
			InputKeys.setKey (InputKeys.WEP3, true);
		}
		if(Input.GetKeyDown(WEP4)){
			InputKeys.setKey (InputKeys.WEP4, true);
		}
        if (Input.GetKeyDown(ABL1)){
            InputKeys.setKey(InputKeys.ABL1, true);
        }
        if (Input.GetKeyDown(ABL2))
        {
            InputKeys.setKey(InputKeys.ABL2, true);
        }
        if (Input.GetKeyDown(ABL3))
        {
            InputKeys.setKey(InputKeys.ABL3, true);
        }
        if (Input.GetKeyDown(ABL4))
        {
            InputKeys.setKey(InputKeys.ABL4, true);
        }


		// UP
		if(Input.GetKeyUp(UP)){
			InputKeys.setKey (InputKeys.UP, false);
			if(Input.GetKey(DOWN)){
				InputKeys.setKey (InputKeys.DOWN, true);
			}
		}
		if(Input.GetKeyUp(LEFT)){
			InputKeys.setKey (InputKeys.LEFT, false);
			if(Input.GetKey(RIGHT)){
				InputKeys.setKey (InputKeys.RIGHT, true);
			}
		}
		if(Input.GetKeyUp(DOWN)){
			InputKeys.setKey (InputKeys.DOWN, false);
			if(Input.GetKey(UP)){
				InputKeys.setKey (InputKeys.UP, true);
			}
		}
		if(Input.GetKeyUp(RIGHT)){
			InputKeys.setKey (InputKeys.RIGHT, false);
			if(Input.GetKey(LEFT)){
				InputKeys.setKey (InputKeys.LEFT, true);
			}
		}
		if(Input.GetKeyUp(SHOOT)){
			InputKeys.setKey (InputKeys.SHOOT, false);
		}
		if(Input.GetKeyUp(SLOW)){
			InputKeys.setKey (InputKeys.SLOW, false);
		}
		if(Input.GetKeyUp(WEP1)){
			InputKeys.setKey (InputKeys.WEP1, false);
		}
		if(Input.GetKeyUp(WEP2)){
			InputKeys.setKey (InputKeys.WEP2, false);
		}
		if(Input.GetKeyUp(WEP3)){
			InputKeys.setKey (InputKeys.WEP3, false);
		}
		if(Input.GetKeyUp(WEP4)){
			InputKeys.setKey (InputKeys.WEP4, false);
		}
        if (Input.GetKeyUp(ABL1))
        {
            InputKeys.setKey(InputKeys.ABL1, false);
        }
        if (Input.GetKeyUp(ABL2))
        {
            InputKeys.setKey(InputKeys.ABL2, false);
        }
        if (Input.GetKeyUp(ABL3))
        {
            InputKeys.setKey(InputKeys.ABL3, false);
        }
        if (Input.GetKeyUp(ABL4))
        {
            InputKeys.setKey(InputKeys.ABL4, false);
        }
	}

}