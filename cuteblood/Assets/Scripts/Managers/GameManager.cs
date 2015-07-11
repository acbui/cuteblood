using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	public static GameManager ins;
	LevelManager LevelMgr;
	PlayerManager PlayerMgr;
	InputManager InputMgr;

	public EGameMode GameMode;

	void OnEnable()
	{
		ins = this;
	}

	void Start()
	{

	}

	public void EndGame (EGryll Winner)
	{
		switch (Winner)
		{
		case EGryll.WINSTON:
			break;
		case EGryll.ACB:
			break;
		case EGryll.BEARD:
			break;
		}
	}
}
