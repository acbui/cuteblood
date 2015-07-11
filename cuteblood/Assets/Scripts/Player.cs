using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Player : MonoBehaviour {

	enum State
	{
		INVISIBLE,
		VISIBLE
	}

	public int ID;
	EGryll Gryll;
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
				CurrentTile.ResetState ();
				bHasMoved = false;
			}
		}
	}

	public void Initialize(int id, EGryll gryll)
	{
		ID = id;
		Gryll = gryll;
		CurrentState = State.INVISIBLE;
		TimeSinceLastHug = 0;
		TimeSinceLastMove = 0;
	}

	public EGryll GetGryll ()
	{
		return Gryll;
	}

	public int GetID()
	{
		return ID;
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
				GameManager.ins.EndGame(Gryll);
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
