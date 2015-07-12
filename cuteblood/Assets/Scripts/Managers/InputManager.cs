using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class InputManager : MonoBehaviour {

	struct PlayerInputs
	{
		bool[] bKeyPressed;
		KeyCode[] KeyInputs;
		
		public void Set ( KeyCode[] Keys )
		{
			bKeyPressed = new bool[ Keys.Length ];
			KeyInputs = new KeyCode[ Keys.Length ];
			Keys.CopyTo(KeyInputs, 0);
			for (int i = 0; i < Keys.Length; i++)
			{
				bKeyPressed[i] = Input.GetKeyDown(KeyInputs[i]);
			}
		}
		
		public bool GetKeyPressed (int index)
		{
			return bKeyPressed [index];
		}
		
		public KeyCode GetKey (int index)
		{
			return KeyInputs [index];
		}

		public int Length ()
		{
			return bKeyPressed.Length;
		}

		public EAction GetAction (int index)
		{
			switch (index)
			{
			case 0:
				return EAction.MoveUP;
			case 1:
				return EAction.MoveLEFT;
			case 2:
				return EAction.MoveDOWN;
			case 3:
				return EAction.MoveRIGHT;
			case 4:
				return EAction.HUG;
			}
			return EAction.NONE;
		}
	}

	public static InputManager ins;

	public bool bAllowGameInput;

	PlayerInputs PlayerOneInputs;
	PlayerInputs PlayerTwoInputs;

	// KEYBOARD
	KeyCode[] PlayerOneKeyMoves = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space }; 
	KeyCode[] PlayerTwoKeyMoves = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.RightShift };

	// CONTROLLERS

	
	void OnEnable()
	{
		ins = this;
		SetKeys (true, false);
		SetKeys (false, false);
		bAllowGameInput = false;
	}

	void Update()
	{
		ReadGameInput ();
	}
	
	public void SetKeys(bool bPlayerOne, bool bUseController)
	{
		if (bPlayerOne) {
			if (!bUseController) {
				PlayerOneInputs.Set (PlayerOneKeyMoves);
			}
		}
		else
		{
			if (!bUseController) {
				PlayerTwoInputs.Set (PlayerTwoKeyMoves);
			}
		}
	}

	public void ReadGameInput()
	{
		if (GameManager.ins.GameView == EGameView.Menu) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				GameManager.ins.BeginGame ();
			}
		} else if (GameManager.ins.GameView == EGameView.End) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				GameManager.ins.OpenMenu ();
			}
		} else if (bAllowGameInput)
			;
		{
			for (int i = 0; i < PlayerOneInputs.Length(); i++)
			{
				if (Input.GetKeyDown (PlayerOneKeyMoves[i]))
				{
					if (PlayerManager.ins != null)
					{
						PlayerManager.ins.P1.GetComponent<Player>().InputRead (PlayerOneInputs.GetAction (i));
					}
				}
			
				if (GameManager.ins.GameMode == EGameMode.MP)
				{
					if (Input.GetKeyDown (PlayerTwoKeyMoves[i]))
					{
						if (PlayerManager.ins != null)
						{
							PlayerManager.ins.P2.GetComponent<Player>().InputRead (PlayerTwoInputs.GetAction (i));
						}
					}
				}
			}
		}
	}
}
