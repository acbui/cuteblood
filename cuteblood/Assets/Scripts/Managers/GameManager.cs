using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	public static GameManager ins;
	LevelManager LevelMgr;
	PlayerManager PlayerMgr;
	InputManager InputMgr;

	public GameObject Map;
	public GameObject Players;

	public EGameMode GameMode;
	public EGameView GameView;

	float TimeSinceGameStart;

	void OnEnable()
	{
		ins = this;
		LevelMgr = gameObject.GetComponent<LevelManager> ();
		PlayerMgr = gameObject.GetComponent<PlayerManager> ();
		InputMgr = gameObject.GetComponent<InputManager> ();

		GameView = EGameView.Menu;
	}

	void Start()
	{
		//BeginGame ();
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

		GameView = EGameView.End;
	}

	public void OpenMenu ()
	{
		GameView = EGameView.Menu;
	}

	public void BeginGame()
	{
		PlayerMgr.CreatePlayer (0, EGryll.ACB);
		LevelMgr.SpawnRectangularGrid (8, 8);
		LevelMgr.LevelSetup ();

		GameView = EGameView.Game;
	}
}
