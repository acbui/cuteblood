using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager ins;

	public Player Player1;
	public Player Player2;

	public void CreatePlayer (int ID, EGryll SelectedGryll)
	{
		if (ID == 0)
		{
			Player1 = new Player(ID, SelectedGryll);
			if (GameManager.ins.GameMode == EGameMode.SP)
			{
				if (SelectedGryll == EGryll.WINSTON)
				{
					Player2 = new Player(ID+1, EGryll.ACB);
                }
				else 
				{
					Player2 = new Player(ID+1, EGryll.WINSTON);
				}
			}
		}
		else 
		{
			Player2 = new Player(ID, SelectedGryll);
		}
	}
}
