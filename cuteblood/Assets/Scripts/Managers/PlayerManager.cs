using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager ins;

	public Player Player1;
	public Player Player2;

	void OnEnable()
	{
		ins = this;
	}

	public void CreatePlayer (int ID, EGryll SelectedGryll)
	{
		if (ID == 0)
		{
			Player1 = gameObject.AddComponent<Player>();
			Player1.Initialize (ID, SelectedGryll);
			if (GameManager.ins.GameMode == EGameMode.SP)
			{
				if (SelectedGryll == EGryll.WINSTON)
				{
					Player2 = gameObject.AddComponent<Player>();
					Player2.Initialize (ID+1, SelectedGryll);
                }
				else 
				{
					Player2 = gameObject.AddComponent<Player>();
					Player2.Initialize (ID+1, EGryll.WINSTON);
				}
			}
		}
		else 
		{
			Player2 = gameObject.AddComponent<Player>();
			Player2.Initialize (ID, SelectedGryll);
		}
	}
}
