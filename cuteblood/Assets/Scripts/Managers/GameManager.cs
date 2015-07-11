﻿using UnityEngine;
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
		LevelMgr = gameObject.GetComponent<LevelManager> ();
		PlayerMgr = gameObject.GetComponent<PlayerManager> ();
		InputMgr = gameObject.GetComponent<InputManager> ();
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
