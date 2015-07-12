using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Tile : MonoBehaviour {

	Tile[] Neighbours;
	bool bOccupied;

	public SpriteRenderer GryllShadow;
	public Sprite[] ShadowSprites;
	bool bShadow;
	public float FadeSpeed;
	Color StartColor;
	Color EndColor;

	void Update()
	{
		if (bShadow)
		{
			GryllShadow.color = Color.Lerp (GryllShadow.color, EndColor, FadeSpeed*Time.deltaTime);
		}
	}

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
		bShadow = false;

		StartColor = GryllShadow.color;
		EndColor = new Color (StartColor.r, StartColor.g, StartColor.b, 0);
	}

	public void SetNeighbour(Tile neighbour, EDirection direction)
	{
		if (neighbour != null)
		{
			int neighbourIndex = (int) direction;
			Neighbours[neighbourIndex] = neighbour;
		}
	}

	public void ShowShadow (EGryll Gryll)
	{
		GryllShadow.sprite = ShadowSprites [(int)Gryll];
		GryllShadow.enabled = true;

		StartCoroutine ("DelayStartFade");
		StartCoroutine ("DelayEndFade");
	}

	IEnumerator DelayStartFade()
	{
		yield return new WaitForSeconds (0.5f);
		bShadow = true;
	}

	IEnumerator DelayEndFade()
	{
		yield return new WaitForSeconds (4);
		bShadow = false;
		GryllShadow.enabled = false;
		GryllShadow.color = StartColor;
	}
}
