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
		Tile p1tile = CurrentMap [ Random.Range (0, MapLength), Random.Range (0, MapHeight) ];
		Tile p2tile = CurrentMap [ Random.Range (0, MapLength), Random.Range (0, MapHeight) ];

		Vector3 p1position = p1tile.transform.position;
		while (p1tile == p2tile)
		{
			p2tile = CurrentMap [ Random.Range (0, MapLength), Random.Range (0, MapHeight) ];
		}
		Vector3 p2position = p2tile.transform.position;

		PlayerManager.ins.Player1.SetTile (p1tile);
		PlayerManager.ins.Player2.SetTile (p2tile);

		EGryll p1gryll = PlayerManager.ins.Player1.GetGryll ();
		EGryll p2gryll = PlayerManager.ins.Player2.GetGryll ();

		PlayerManager.ins.P1.transform.position = p1position;
		GameObject p1 = Instantiate (PlayerPrefabs[(int) p1gryll], p1position, Quaternion.identity) as GameObject;
		p1.transform.parent = PlayerManager.ins.P1.transform;
		p1.name = "Player1";
		SpawnCamera (PlayerManager.ins.P1);

		PlayerManager.ins.P2.transform.position = p2position;
		GameObject p2 = Instantiate (PlayerPrefabs[(int) p2gryll], p2position, Quaternion.identity) as GameObject;
		p2.transform.parent = PlayerManager.ins.P2.transform;
		p2.name = "Player2";
		SpawnCamera (PlayerManager.ins.P2);
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
			cam.rect = new Rect (0,0,0.495f,1);
		}
		else 
		{
			CamObj.name = "Player2 Camera";
			cam.rect = new Rect (0.5f,0,0.5f,1);
		}
	}
}

