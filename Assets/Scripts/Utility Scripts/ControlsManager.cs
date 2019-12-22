using UnityEngine;
using System.Collections;

public enum ControlsEvent
{
	NONE,
	ARROW_LEFT,
	ARROW_UP,
	ARROW_DOWN,
	ARROW_RIGHT,
	ARROW_DIAGONAL_UP_LEFT,
	ARROW_DIAGONAL_UP_RIGHT,
	ARROW_DIAGONAL_DOWN_LEFT,
	ARROW_DIAGONAL_DOWN_RIGHT,
	KEY_PLUS,
	KEY_MINUS,
	KEY_0,
	KEY_1,
	KEY_2,
	KEY_3,
	KEY_4,
	KEY_5,
	KEY_6,
	KEY_7,
	KEY_8,
	KEY_9,
	KEY_A,
	KEY_B,
	KEY_C,
	KEY_D,
	KEY_E,
	KEY_F,
	KEY_G,
	KEY_H,
	KEY_I,
	KEY_J,
	KEY_K,
	KEY_L,
	KEY_M,
	KEY_N,
	KEY_O,
	KEY_P,
	KEY_Q,
	KEY_R,
	KEY_S,
	KEY_T,
	KEY_U,
	KEY_V,
	KEY_W,
	KEY_X,
	KEY_Y,
	KEY_Z
}

public class ControlsManager : ManagerClass 
{
	public static ControlsManager instance;

	public bool readActiveInput;

	ControlsEvent currentEvent;
	public ControlsEvent controlEvent
	{
		get
		{
//			ControlsEvent current = currentEvent;
//			currentEvent = ControlsEvent.NONE;
//			return current;
			return currentEvent;
		}
	}

	void Awake()
	{
		instance = this;
	}
	void Start () 
	{
		// Debug.Log ("Starting Controls Manager");
		readActiveInput = false;
		Initialize();
	}

	public void ReadInput()
	{
		if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
			currentEvent = ControlsEvent.ARROW_DIAGONAL_UP_LEFT;
		else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
			currentEvent = ControlsEvent.ARROW_DIAGONAL_UP_RIGHT;
		else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
			currentEvent = ControlsEvent.ARROW_DIAGONAL_DOWN_LEFT;
		else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
			currentEvent = ControlsEvent.ARROW_DIAGONAL_DOWN_RIGHT;
		else if (Input.GetKey(KeyCode.LeftArrow))
			currentEvent = ControlsEvent.ARROW_LEFT;
		else if (Input.GetKey(KeyCode.RightArrow))
			currentEvent = ControlsEvent.ARROW_RIGHT;
		else if (Input.GetKey(KeyCode.UpArrow))
			currentEvent = ControlsEvent.ARROW_UP;
		else if (Input.GetKey(KeyCode.DownArrow))
			currentEvent = ControlsEvent.ARROW_DOWN;
		else if (Input.GetKey(KeyCode.Equals))
			currentEvent = ControlsEvent.KEY_PLUS;
		else if (Input.GetKey(KeyCode.Minus))
			currentEvent = ControlsEvent.KEY_MINUS;
		else if (Input.GetKey(KeyCode.Alpha0))
			currentEvent = ControlsEvent.KEY_0;
		else if (Input.GetKey(KeyCode.Alpha1))
			currentEvent = ControlsEvent.KEY_1;
		else if (Input.GetKey(KeyCode.Alpha2))
			currentEvent = ControlsEvent.KEY_2;
		else if (Input.GetKey(KeyCode.Alpha3))
			currentEvent = ControlsEvent.KEY_3;
		else if (Input.GetKey(KeyCode.Alpha4))
			currentEvent = ControlsEvent.KEY_4;
		else if (Input.GetKey(KeyCode.Alpha5))
			currentEvent = ControlsEvent.KEY_5;
		else if (Input.GetKey(KeyCode.Alpha6))
			currentEvent = ControlsEvent.KEY_6;
		else if (Input.GetKey(KeyCode.Alpha7))
			currentEvent = ControlsEvent.KEY_7;
		else if (Input.GetKey(KeyCode.Alpha8))
			currentEvent = ControlsEvent.KEY_8;
		else if (Input.GetKey(KeyCode.Alpha9))
			currentEvent = ControlsEvent.KEY_9;
		else if (Input.GetKey(KeyCode.A))
			currentEvent = ControlsEvent.KEY_A;
		else if (Input.GetKey(KeyCode.B))
			currentEvent = ControlsEvent.KEY_B;
		else if (Input.GetKey(KeyCode.C))
			currentEvent = ControlsEvent.KEY_C;
		else if (Input.GetKey(KeyCode.D))
			currentEvent = ControlsEvent.KEY_D;
		else if (Input.GetKey(KeyCode.E))
			currentEvent = ControlsEvent.KEY_E;
		else if (Input.GetKey(KeyCode.F))
			currentEvent = ControlsEvent.KEY_F;
		else if (Input.GetKey(KeyCode.G))
			currentEvent = ControlsEvent.KEY_G;
		else if (Input.GetKey(KeyCode.H))
			currentEvent = ControlsEvent.KEY_H;
		else if (Input.GetKey(KeyCode.I))
			currentEvent = ControlsEvent.KEY_I;
		else if (Input.GetKey(KeyCode.J))
			currentEvent = ControlsEvent.KEY_J;
		else if (Input.GetKey(KeyCode.K))
			currentEvent = ControlsEvent.KEY_K;
		else if (Input.GetKey(KeyCode.L))
			currentEvent = ControlsEvent.KEY_L;
		else if (Input.GetKey(KeyCode.M))
			currentEvent = ControlsEvent.KEY_M;
		else if (Input.GetKey(KeyCode.N))
			currentEvent = ControlsEvent.KEY_N;
		else if (Input.GetKey(KeyCode.O))
			currentEvent = ControlsEvent.KEY_O;
		else if (Input.GetKey(KeyCode.P))
			currentEvent = ControlsEvent.KEY_P;
		else if (Input.GetKey(KeyCode.Q))
			currentEvent = ControlsEvent.KEY_Q;
		else if (Input.GetKey(KeyCode.R))
			currentEvent = ControlsEvent.KEY_R;
		else if (Input.GetKey(KeyCode.S))
			currentEvent = ControlsEvent.KEY_S;
		else if (Input.GetKey(KeyCode.T))
			currentEvent = ControlsEvent.KEY_T;
		else if (Input.GetKey(KeyCode.U))
			currentEvent = ControlsEvent.KEY_U;
		else if (Input.GetKey(KeyCode.V))
			currentEvent = ControlsEvent.KEY_V;
		else if (Input.GetKey(KeyCode.W))
			currentEvent = ControlsEvent.KEY_W;
		else if (Input.GetKey(KeyCode.X))
			currentEvent = ControlsEvent.KEY_X;
		else if (Input.GetKey(KeyCode.Y))
			currentEvent = ControlsEvent.KEY_Y;
		else if (Input.GetKey(KeyCode.Z))
			currentEvent = ControlsEvent.KEY_Z;
	}

	// Update is called once per frame
	void Update () 
	{
		if (readActiveInput && Input.anyKey)
		{
			ReadInput();
		}
		else if (Input.anyKeyDown) 
		{
			ReadInput();
		}
		else 
		{
			currentEvent = ControlsEvent.NONE;
			readActiveInput = false;
		}
	}
}
