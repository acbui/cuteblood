using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Tile : MonoBehaviour {

	Tile[] Neighbours;
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
			gameObject.GetComponent<Animator>().SetBool ("bDisturbed", bDisturbed);
			string s = gameObject.name.Substring(2);
			
			GameObject tileObj = GameObject.Find ("P" + (gameObject.name.Contains ("1") ? 2 : 1) + s);
			tileObj.GetComponent<Animator> ().SetBool ("bDisturbed", bDisturbed);
		}
			
		bOccupied = true;
	}

	public void Exited()
	{
		bOccupied = false;
	}
	

	public void Initialize()
	{
		Neighbours = new Tile[4];
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
