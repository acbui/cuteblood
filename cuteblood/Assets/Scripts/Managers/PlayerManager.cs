using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager ins;

	public Player Player1;
	public Player Player2;

	public GameObject P1;
	public GameObject P2;

	void OnEnable()
	{
		ins = this;
		Player1 = P1.GetComponent<Player> ();
		Player2 = P2.GetComponent<Player> ();
	}

	public void CreatePlayer (int ID, EGryll SelectedGryll)
	{
		if (ID == 0)
		{
			Player1.Initialize (ID, SelectedGryll);
			if (GameManager.ins.GameMode == EGameMode.SP)
			{
				if (SelectedGryll == EGryll.WINSTON)
				{
					Player2.Initialize (ID+1, SelectedGryll);
                }
				else 
				{
					Player2.Initialize (ID+1, EGryll.WINSTON);
				}
			}
		}
		else 
		{
			Player2.Initialize (ID, SelectedGryll);
		}
	}
}
