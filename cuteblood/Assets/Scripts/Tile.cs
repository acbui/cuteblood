﻿using UnityEngine;
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

	public Tile NeighbourInDirection (EDirection MoveDirection)
	{
		return Neighbours [(int)MoveDirection];
	}

	public bool IsHuggable()
	{
		return bOccupied;
	}

	public void Entered(bool bDisturbed)
	{
		if (bDisturbed)
		{
			CurrentState = State.DISTURBED;
		}
		bOccupied = true;
	}

	public void Exited()
	{
		bOccupied = false;
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
