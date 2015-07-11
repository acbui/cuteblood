using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LevelManager : MonoBehaviour
{
	public static LevelManager ins;

	// current map can only be rectangular anyway
	Tile[,] CurrentMap;
	int MapLength;
	int MapHeight;
	
	public GameObject TilePrefab; 
	public GameObject[] PlayerPrefabs;
	public GameObject PlayerCameraPrefab;
	
	public float TileOffset;

	void OnEnable()
	{
		ins = this;

		SpriteRenderer renderer = TilePrefab.GetComponent<SpriteRenderer> ();
		TileOffset = renderer.sprite.bounds.size.x;
	}
	
	public void SpawnRectangularGrid(int Length, int Height)
	{
		Vector3 location = Vector3.zero;
		CurrentMap = new Tile[Height,Length];
		MapLength = Length;
		MapHeight = Height;

		for (int row = 0; row < Height; row++)
		{
			for (int column = 0; column < Length; column++)
			{
				GameObject newTile = Instantiate(TilePrefab, location, Quaternion.identity) as GameObject;
				newTile.transform.parent = GameManager.ins.Map.transform;
				newTile.name = "" + row + "x" + column;

				Tile tileComponent = newTile.GetComponent<Tile>();

				if (tileComponent != null)
				{
					tileComponent.Initialize();
					
					if (row > 0)
					{
						tileComponent.SetNeighbour (CurrentMap[row-1,column], EDirection.UP);
						CurrentMap[row-1,column].SetNeighbour (tileComponent, EDirection.DOWN);

					}
					if (column > 0)
					{
						tileComponent.SetNeighbour (CurrentMap[row,column-1], EDirection.LEFT);
						CurrentMap[row,column-1].SetNeighbour(tileComponent, EDirection.RIGHT);
					}

					CurrentMap[row,column] = tileComponent;
				}

				location = new Vector3 ( location.x + TileOffset, location.y, location.z );
			}
			location = new Vector3 ( 0, location.y - TileOffset, location.z );
		}
	}

	public void SpawnPlayers()
	{
		int[] p1tile = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };
		int[] p2tile = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };

		Vector3 p1start = CurrentMap [p1tile [0], p1tile [1]].transform.position;
		while (p1tile == p2tile)
		{
			p2tile = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };
		}
		Vector3 p2start = CurrentMap [p2tile [0], p2tile [1]].transform.position;

		EGryll p1gryll = PlayerManager.ins.Player1.GetGryll ();
		EGryll p2gryll = PlayerManager.ins.Player2.GetGryll ();

		GameObject p1 = Instantiate (PlayerPrefabs[(int) p1gryll], p1start, Quaternion.identity) as GameObject;
		Player p1component = p1.GetComponent<Player>();
		p1component.Initialize(PlayerManager.ins.Player1.GetID(), PlayerManager.ins.Player1.GetGryll());
		PlayerManager.ins.Player1 = p1component;
		p1.transform.parent = GameManager.ins.Players.transform;
		p1.name = "Player1";
		SpawnCamera (p1);

		GameObject p2 = Instantiate (PlayerPrefabs[(int) p2gryll], p2start, Quaternion.identity) as GameObject;
		Player p2component = p2.GetComponent<Player>();
		p2component.Initialize(PlayerManager.ins.Player2.GetID(), PlayerManager.ins.Player2.GetGryll());
		PlayerManager.ins.Player2 = p2component;
		p2.transform.parent = GameManager.ins.Players.transform;
		p2.name = "Player2";
		SpawnCamera (p2);

		foreach (Player playerScript in gameObject.GetComponents<Player>())
		{
			Destroy (playerScript);
		}
	}

	void SpawnCamera (GameObject player)
	{
		GameObject CamObj = Instantiate (PlayerCameraPrefab, 
		                                 new Vector3 (	player.transform.position.x, 
		             									player.transform.position.y, 
		             									PlayerCameraPrefab.transform.position.z),
		                                 Quaternion.identity) as GameObject;
		CamObj.transform.parent = player.transform;
		Camera cam = CamObj.GetComponent<Camera> ();
		if (player.GetComponent<Player>().GetID () == 0)
		{
			CamObj.name = "Player1 Camera";
			cam.rect = new Rect (0,0,0.5f,1);
		}
		else 
		{
			CamObj.name = "Player2 Camera";
			cam.rect = new Rect (0.5f,0,0.5f,1);
		}
	}
}

