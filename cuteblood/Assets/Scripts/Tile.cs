using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Tile : MonoBehaviour {

	enum State
	{
		UNDISTURBED,
		DISTURBED
	}

	Tile[] Neighbours;
	State CurrentState;
	bool bOccupied;
	bool bOverlappingPlayers;

	public Tile NeighbourInDirection (EDirection MoveDirection)
	{
		return Neighbours [(int)MoveDirection];
	}

	public bool IsHuggable()
	{
		if (bOverlappingPlayers)
		{
			return true;
		}
		return false;
	}

	public void Entered(bool bDisturbed, Tile PreviousTile)
	{
		if (bDisturbed)
		{
			CurrentState = State.DISTURBED;
		}
		if (PreviousTile != this)
		{
			if (bOccupied)
			{
				bOverlappingPlayers = true;
			}
			else 
			{
				bOccupied = true;
			}
		}
	}

	public void Exited()
	{
		if (bOverlappingPlayers)
		{
			bOverlappingPlayers = false;
		}
		else 
		{
			bOccupied = false;
		}
	}

	public void ResetState()
	{
		CurrentState = State.UNDISTURBED;
	}

	public void Initialize()
	{
		Neighbours = new Tile[4];
		CurrentState = State.UNDISTURBED;
		bOccupied = false;
		bOverlappingPlayers = false;
	}

	public void SetNeighbour(Tile neighbour, EDirection direction)
	{
		if (neighbour != null)
		{
			int neighbourIndex = (int) direction;
			Neighbours[neighbourIndex] = neighbour;
		}
	}
}
