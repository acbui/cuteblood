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

	public float TimeSinceLastHug;
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

	public void SetInitialTile(Tile tile)
	{
		CurrentTile = tile;
		PreviousTile = tile;
		tile.Entered (false);
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
			gameObject.GetComponentInChildren<Animator>().SetBool ("bHug", true);
			TimeSinceLastHug = 0;
			string s = CurrentTile.gameObject.name.Substring(2);

			GameObject tileObj = GameObject.Find ("P" + (ID == 0 ? 2 : 1) + s);
			Tile tileScript = null;
			if (tileObj != null)
			{
				tileScript = tileObj.GetComponent<Tile>();
			}

			if (tileScript.IsHuggable())
			{
				GameManager.ins.EndGame(Gryll);
			}
		}
	}

	void TryMove(EAction MoveDirection)
	{
		if (TimeSinceLastMove > MoveDelay)
		{
			bool bDisturbTile = (TimeSinceLastMove < MoveCooldown);
			bHasMoved = true;
			TimeSinceLastMove = 0;

			EDirection RelativeDirection = HelperFunctions.GetDirection(((int)MoveDirection + (int)LevelManager.ins.PlayerRotations[ID])%4);
			Tile Dest = CurrentTile.NeighbourInDirection (RelativeDirection);

			if (Dest != null)
			{
				CurrentTile.Exited();
				PreviousTile = CurrentTile;
				CurrentTile = Dest;
				MoveDistance = Vector3.Distance (PreviousTile.transform.position, CurrentTile.transform.position);
				MoveStartTime = Time.time;
			}

			CurrentTile.Entered (bDisturbTile);
		}
	}
}
