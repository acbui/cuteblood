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
			for (int i = 0; i < Keys.Length; i++)
			{
				bKeyPressed[i] = Input.GetKeyDown(Keys[i]);
			}
			KeyInputs = new KeyCode[ Keys.Length ];
			Keys.CopyTo(KeyInputs, 0);
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

	PlayerInputs PlayerOneInputs;
	PlayerInputs PlayerTwoInputs;

	// KEYBOARD
	KeyCode[] PlayerOneKeyMoves = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space }; 
	KeyCode[] PlayerTwoKeyMoves = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.RightShift };

	// CONTROLLERS

	
	void OnEnable()
	{
		ins = this;
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
		for (int i = 0; i < PlayerOneInputs.Length(); i++)
		{
			if (PlayerOneInputs.GetKeyPressed(i))
			{
				if (PlayerManager.ins != null)
				{
					PlayerManager.ins.Player1.InputRead (PlayerOneInputs.GetAction (i));
				}
			}
		
			if (GameManager.ins.GameMode == EGameMode.MP)
			{
				if (PlayerTwoInputs.GetKeyPressed(i))
				{
					if (PlayerManager.ins != null)
					{
						PlayerManager.ins.Player2.InputRead (PlayerTwoInputs.GetAction (i));
					}
				}
			}
			else if (GameManager.ins.GameMode == EGameMode.SP)
			{
				// magic number: number of possible actions (including none)
				EAction RandomAction = PlayerOneInputs.GetAction (Random.Range (0, 5));
				PlayerManager.ins.Player2.InputRead (RandomAction);
			}
		}
	}
}
