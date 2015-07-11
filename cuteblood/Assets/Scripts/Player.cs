﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Player : MonoBehaviour {

	enum State
	{
		INVISIBLE,
		VISIBLE
	}

	int ID;
	Tile CurrentTile;
	State CurrentState;

	bool bHasMoved;
	bool bHasHugged;
	public float HugCooldown;
	public float HugDelay;
	public float MoveCooldown;
	public float MoveDelay;

	float TimeSinceLastHug;
	float TimeSinceLastMove;

	void Update()
	{
		if (bHasHugged)
		{
			if (TimeSinceLastHug < HugCooldown)
			{
				TimeSinceLastHug += Time.deltaTime;
			}
			else 
			{
				TimeSinceLastHug = HugCooldown;
				CurrentState = State.INVISIBLE;
				bHasHugged = false;
			}
		}
		if (bHasMoved)
		{
			if (TimeSinceLastMove < MoveCooldown)
			{
				TimeSinceLastMove += Time.deltaTime;
			}
			else 
			{
				TimeSinceLastMove = MoveCooldown;
				CurrentTile.Reset ();
				bHasMoved = false;
			}
		}
	}

	public void Initialize(int id, Tile first)
	{
		ID = id;
		CurrentTile = first;
		CurrentState = State.INVISIBLE;
		TimeSinceLastHug = 0;
		TimeSinceLastMove = 0;
	}

	public void InputRead(EAction ActionInput)
	{
		if (ActionInput == EAction.NONE)
		{
			return;
		}
		else if (ActionInput == EAction.HUG)
		{
			TryHug ();
		}
		else 
		{
			TryMove (ActionInput);
		}
	}

	void TryHug()
	{
		if (TimeSinceLastHug < HugCooldown)
		{
			CurrentState = State.VISIBLE;
		}
		if (TimeSinceLastHug > HugDelay)
		{
			bHasHugged = true;
			TimeSinceLastHug = 0;

			if (CurrentTile.IsHuggable())
			{
				GameManager.ins.EndGame(ID);
			}
		}
	}

	void TryMove(EAction MoveDirection)
	{
		if (TimeSinceLastMove > MoveDelay)
		{
			bHasMoved = true;
			TimeSinceLastMove = 0;

			Tile Dest = CurrentTile.NeighbourInDirection (MoveDirection);
			bool bDisturbTile = (TimeSinceLastMove < MoveCooldown) ? true : false;
			if (Dest != null)
			{
				CurrentTile.Exited();
				CurrentTile = Dest;
			}

			CurrentTile.Entered (bDisturbTile, CurrentTile);
		}
	}
}
