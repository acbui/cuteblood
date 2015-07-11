using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LevelManager : MonoBehaviour
{
	public static LevelManager ins;

	// current map can only be rectangular anyway
	public GameObject P1Map;
	public GameObject P2Map;
	Tile[,] Player1Map;
	Tile[,] Player2Map;
	
	public ERotation[] PlayerRotations;

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

		PlayerRotations = new ERotation[2];
		PlayerRotations[0] = GetRandomRotation ();
		PlayerRotations[1] = GetRandomRotation ();
	}
	
	public void SpawnRectangularGrid(int Length, int Height)
	{
		MapLength = Length;
		MapHeight = Height;

		Vector3 p1location = Vector3.zero;
		Player1Map = new Tile[Height, Length];

		Vector3 p2location = new Vector3 (p1location.x + TileOffset * (Length + 5), 0, 0);
		Vector3 initP2location = p2location;
		Player2Map = new Tile[Height, Length];

		for (int row = 0; row < Height; row++)
		{
			for (int column = 0; column < Length; column++)
			{
				GameObject newTile = Instantiate(TilePrefab, p1location, Quaternion.identity) as GameObject;
				newTile.transform.parent = P1Map.transform;
				newTile.name = "P1 " + row + "x" + column;

				Tile tileComponent = newTile.GetComponent<Tile>();

				if (tileComponent != null)
				{
					tileComponent.Initialize();
					
					if (row > 0)
					{
						tileComponent.SetNeighbour (Player1Map[row-1,column], EDirection.UP);
						Player1Map[row-1,column].SetNeighbour (tileComponent, EDirection.DOWN);
						
					}
					if (column > 0)
					{
						tileComponent.SetNeighbour (Player1Map[row,column-1], EDirection.LEFT);
						Player1Map[row,column-1].SetNeighbour(tileComponent, EDirection.RIGHT);
					}
					
					Player1Map[row,column] = tileComponent;
				}

				GameObject newTile2 = Instantiate(TilePrefab, p2location, Quaternion.identity) as GameObject;
				newTile2.transform.parent = P2Map.transform;
				newTile2.name = "P2 " + row + "x" + column;
				
				Tile tileComponent2 = newTile2.GetComponent<Tile>();
				
				if (tileComponent2 != null)
				{
					tileComponent2.Initialize();
					
					if (row > 0)
					{
						tileComponent2.SetNeighbour (Player2Map[row-1,column], EDirection.UP);
						Player2Map[row-1,column].SetNeighbour (tileComponent2, EDirection.DOWN);
						
					}
					if (column > 0)
					{
						tileComponent2.SetNeighbour (Player2Map[row,column-1], EDirection.LEFT);
						Player2Map[row,column-1].SetNeighbour(tileComponent2, EDirection.RIGHT);
					}
					
					Player2Map[row,column] = tileComponent2;
				}
				
				p1location = new Vector3 ( p1location.x + TileOffset, p1location.y, p1location.z );
				p2location = new Vector3 ( p2location.x + TileOffset, p2location.y, p2location.z );
			}
			p1location = new Vector3 ( 0, p1location.y - TileOffset, p1location.z );
			p2location = new Vector3 ( initP2location.x , p2location.y - TileOffset, p2location.z );
		}
	}

	public void SpawnPlayers()
	{
		int[] p1start = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };
		int[] p2start = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };

		Tile p1tile = Player1Map [ p1start[0], p1start[1] ];


		Vector3 p1position = p1tile.transform.position;
		while (p1start == p2start)
		{
			p2start = new int[] { Random.Range (0, MapLength), Random.Range (0, MapHeight) };
		}
		Tile p2tile = Player2Map [ p2start[0], p2start[1] ];
		Vector3 p2position = p2tile.transform.position;

		PlayerManager.ins.Player1.SetInitialTile (p1tile);
		PlayerManager.ins.Player2.SetInitialTile (p2tile);

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

	public void LevelSetup()
	{
		SpawnPlayers ();
		RotatePlayerView (PlayerRotations[0], PlayerManager.ins.P1, Player1Map);
		RotatePlayerView (PlayerRotations[1], PlayerManager.ins.P2, Player2Map);
	}

	void RotatePlayerView(ERotation Rot, GameObject Player, Tile[,] PlayerMap)
	{
		Player.transform.Rotate (0, 0, (int)Rot * 90);

		for (int i = 0; i < MapHeight; i++)
		{
			for (int j = 0; j < MapLength; j++)
			{
				PlayerMap[i,j].gameObject.transform.Rotate (0, 0, (int)Rot * 90);
			}
		}
	}

	ERotation GetRandomRotation()
	{
		int ran = Random.Range (0, 4);
		
		switch (ran) {
		case 0:
			return ERotation.CW0;
		case 1:
			return ERotation.CW90;
		case 2:
			return ERotation.CW180;
		case 3:
			return ERotation.CW270;
		}
		return ERotation.CW0;
	}
}

