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

	public float TileOffset;

	void OnEnable()
	{
		ins = this;
	}
	
	public void SpawnRectangularGrid(int Length, int Height)
	{
		Vector3 location = Vector3.zero;
		CurrentMap = new Tile[Height,Length];
		for (int row = 0; row < Height; row++)
		{
			for (int column = 0; column < Length; column++)
			{
				GameObject newTile = Instantiate(TilePrefab, location, Quaternion.identity) as GameObject;
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
			location = new Vector3 ( location.x, location.y + TileOffset, location.z );
		}
	}
}

