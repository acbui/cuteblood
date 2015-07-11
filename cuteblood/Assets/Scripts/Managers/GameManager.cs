using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	public static GameManager ins;
	public EGameMode GameMode;

	public void EndGame (EGryll Winner)
	{
		switch (Winner)
		{
		case EGryll.WINSTON:
			break;
		case EGryll.ACB:
			break;
		case EGryll.BEARD:
			break;
		}
	}
}
