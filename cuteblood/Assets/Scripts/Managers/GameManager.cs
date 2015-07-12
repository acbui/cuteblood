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
	public float GameDuration;
	bool bGameStarted;
	public float FadeSpeed;
	Color StartColor;
	Color EndColor;

	public GameObject SpriteScreen;
	public Sprite CharacterScreen;
	public Sprite[] WinScreens;

	void OnEnable()
	{
		ins = this;
		LevelMgr = gameObject.GetComponent<LevelManager> ();
		PlayerMgr = gameObject.GetComponent<PlayerManager> ();
		InputMgr = gameObject.GetComponent<InputManager> ();

		GameView = EGameView.Menu;
		bGameStarted = false;
	}

	void Start()
	{
		//BeginGame ();
	}

	void Update()
	{
		if (bGameStarted)
		{
			SpriteScreen.GetComponent<SpriteRenderer>().color = Color.Lerp (SpriteScreen.GetComponent<SpriteRenderer>().color, EndColor, FadeSpeed*Time.deltaTime);
		}
		if (GameView == EGameView.Game)
		{
			TimeSinceGameStart += Time.deltaTime;
			if (TimeSinceGameStart >= GameDuration)
			{
				EndGame (EGryll.BEARD);
			}
		}
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
		InputMgr.bAllowGameInput = false;
	}

	public void OpenMenu ()
	{
		GameView = EGameView.Menu;
		SpriteScreen.SetActive (true);
		SpriteScreen.GetComponentInChildren<MeshRenderer> ().enabled = true;
		bGameStarted = false;
	}

	public void BeginGame()
	{
		if (!bGameStarted)
		{
			GameView = EGameView.Game;
			bGameStarted = true;
			SpriteScreen.GetComponentInChildren<MeshRenderer> ().enabled = false;
			SpriteScreen.GetComponent<SpriteRenderer> ().sprite = CharacterScreen;
			
			StartColor = SpriteScreen.GetComponent<SpriteRenderer>().color;
			EndColor = new Color (StartColor.r, StartColor.g, StartColor.b, 0);
			
			StartCoroutine ("DelayFadeStart");
			StartCoroutine ("DelayGameStart");
		}
	}

	IEnumerator DelayFadeStart()
	{
		yield return new WaitForSeconds (0.5f);
		bGameStarted = true;
	}

	IEnumerator DelayGameStart()
	{
		yield return new WaitForSeconds (1.5f);
		GenerateGame ();
	}

	void GenerateGame()
	{
		SpriteRenderer renderer = SpriteScreen.GetComponent<SpriteRenderer> ();
		renderer.enabled = false;
		renderer.color = StartColor;
		bGameStarted = false;

		PlayerMgr.CreatePlayer (0, EGryll.ACB);
		LevelMgr.SpawnRectangularGrid (8, 8);
		LevelMgr.LevelSetup ();

		InputMgr.bAllowGameInput = true;

		TimeSinceGameStart = 0;
	}
}
