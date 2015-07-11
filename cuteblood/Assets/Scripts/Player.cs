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
	public Tile CurrentTile;
	private Tile PreviousTile;
	State CurrentState;

	bool bHasMoved;
	bool bHasHugged;
	public float HugCooldown;
	public float HugDelay;
	public float MoveCooldown;
	public float MoveDelay;

	public float MoveSpeed;
	public float MoveStartTime;
	public float MoveDistance;

	float TimeSinceLastHug;
	public float TimeSinceLastMove;

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

				float distanceCovered = (Time.time - MoveStartTime) * MoveSpeed;
				float fractionMove = distanceCovered / MoveDistance;
				transform.position = Vector3.Lerp(PreviousTile.transform.position, CurrentTile.transform.position, fractionMove);
			}
			else 
			{
				TimeSinceLastMove = MoveCooldown;
				CurrentTile.ResetState ();
				bHasMoved = false;
				transform.position = CurrentTile.transform.position;
			}
		}
	}

	public void Initialize(int id, EGryll gryll)
	{
		ID = id;
		Gryll = gryll;
		CurrentState = State.INVISIBLE;
		TimeSinceLastHug = HugCooldown;
		TimeSinceLastMove = MoveCooldown;
	}

	public void SetTile(Tile tile)
	{
		CurrentTile = tile;
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
			print ("here");
			bHasMoved = true;
			TimeSinceLastMove = 0;

			Tile Dest = CurrentTile.NeighbourInDirection (MoveDirection);
			bool bDisturbTile = (TimeSinceLastMove < MoveCooldown) ? true : false;
			if (Dest != null)
			{
				CurrentTile.Exited();
				PreviousTile = CurrentTile;
				CurrentTile = Dest;
				MoveDistance = Vector3.Distance (PreviousTile.transform.position, CurrentTile.transform.position);
				MoveStartTime = Time.time;
			}

			CurrentTile.Entered (bDisturbTile, CurrentTile);
		}
	}
}
