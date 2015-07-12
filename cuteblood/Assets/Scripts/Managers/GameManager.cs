using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	enum EditSettings
	{
		MODE,
		DURATION
	}

	public static GameManager ins;
	public GameObject ManagerPrefab;
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

	public GameObject Start;
	public GameObject Settings;
	public GameObject ModeSetting;
	public GameObject DurationSetting;
	public SpriteRenderer RightTriangle;
	public SpriteRenderer LeftTriangle; 

	bool bInSettings;
	EditSettings CurrentSetting;

	void OnEnable()
	{
		ins = this;
		LevelMgr = gameObject.GetComponent<LevelManager> ();
		PlayerMgr = gameObject.GetComponent<PlayerManager> ();
		InputMgr = gameObject.GetComponent<InputManager> ();

		GameView = EGameView.Menu;
		bGameStarted = false;
		MainCamera.clearFlags = CameraClearFlags.Nothing;
		MainCamera.depth = 5;
	}

	void Update()
	{
		if (bInSettings)
		{
			if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				CurrentSetting = EditSettings.DURATION;
				foreach (SpriteRenderer sr in ModeSetting.GetComponentsInChildren<SpriteRenderer>())
				{
					sr.enabled = false;
				}
				foreach (SpriteRenderer sr in DurationSetting.GetComponentsInChildren<SpriteRenderer>())
				{
					sr.enabled = true;
					if (sr.gameObject.name.Contains ("right"))
					{
						RightTriangle = sr;
					}
					else 
					{
						LeftTriangle = sr;
					}
				}
			}
			else if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				CurrentSetting = EditSettings.MODE;
				foreach (SpriteRenderer sr in DurationSetting.GetComponentsInChildren<SpriteRenderer>())
				{
					sr.enabled = false;
				}
				foreach (SpriteRenderer sr in ModeSetting.GetComponentsInChildren<SpriteRenderer>())
				{
					if (sr.gameObject.name.Contains ("right"))
					{
						RightTriangle = sr;
					}
					else 
					{
						LeftTriangle = sr;
					}
					if (GameMode == EGameMode.MP)
					{
						RightTriangle.enabled = true;
					}
					else 
					{
						LeftTriangle.enabled = true;
					}
				}
			}
			else if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				if (CurrentSetting == EditSettings.MODE)
				{
					ModeSetting.GetComponent<TextMesh>().text = "single player";
					GameMode = EGameMode.SP;
					RightTriangle.enabled = false;
					LeftTriangle.enabled = true;
				}
				else 
				{
					GameDuration += 5;
					DurationSetting.GetComponent<TextMesh>().text = GameDuration + " seconds";
				}
			}
			else if (Input.GetKeyDown (KeyCode.LeftArrow))
			{
				if (CurrentSetting == EditSettings.MODE)
				{
					ModeSetting.GetComponent<TextMesh>().text = "multiplayer";
					GameMode = EGameMode.MP;
					RightTriangle.enabled = true;
					LeftTriangle.enabled = false;
				}
				else 
				{
					GameDuration -= 5;
					DurationSetting.GetComponent<TextMesh>().text = GameDuration + " seconds";
				}
			}
		}
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
		ResetGame ();
		GameView = EGameView.Menu;
		SpriteScreen.GetComponent<SpriteRenderer> ().sprite = StartScreen;

		bGameStarted = false;
	}

	public void OpenSettings()
	{
		Start.SetActive (false);
		Settings.SetActive (true);
		bInSettings = true;
		CurrentSetting = EditSettings.MODE;
	}

	public void BeginGame()
	{
		if (!bGameStarted)
		{
			GameView = EGameView.Game;
			foreach (MeshRenderer m in SpriteScreen.GetComponentsInChildren<MeshRenderer>())
			{
				m.enabled = false;
			}
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

	void ResetGame()
	{
		LevelMgr.Reset ();
		PlayerMgr.Reset ();
	}
}
