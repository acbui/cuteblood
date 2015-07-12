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

	public Camera MainCamera;
	public GameObject SpriteScreen;
	public Sprite StartScreen;
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
		MainCamera.clearFlags = CameraClearFlags.Nothing;
		MainCamera.depth = 5;
		SpriteRenderer renderer = SpriteScreen.GetComponent<SpriteRenderer> ();
		renderer.sprite = WinScreens [(int)Winner];
		renderer.enabled = true;

		GameView = EGameView.End;
		InputMgr.bAllowGameInput = false;
	}

	public void OpenMenu ()
	{
		GameView = EGameView.Menu;
		SpriteScreen.GetComponent<SpriteRenderer> ().sprite = StartScreen;
		SpriteScreen.GetComponentInChildren<MeshRenderer> ().enabled = true;
		bGameStarted = false;
	}

	public void BeginGame()
	{
		if (!bGameStarted)
		{
			GameView = EGameView.Game;
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
		yield return new WaitForSeconds (1f);
		bGameStarted = true;
	}

	IEnumerator DelayGameStart()
	{
		yield return new WaitForSeconds (2f);
		GenerateGame ();
	}

	void GenerateGame()
	{
		MainCamera.clearFlags = CameraClearFlags.SolidColor;
		MainCamera.depth = -5;
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
