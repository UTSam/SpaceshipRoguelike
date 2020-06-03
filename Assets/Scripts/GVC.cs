using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Tilemaps
{
	public Tilemap walls;
	public Tilemap floor;
	public Tilemap doors;

	public void ClearAllTiles()
	{
		if(this.doors)
			this.doors.ClearAllTiles();

		if (this.floor)
			this.floor.ClearAllTiles();

		if (this.walls)
			this.walls.ClearAllTiles();
	}
}

[System.Serializable]
public class Tiles
{
	public Tile corridorHorizontal;
	public Tile corridorVertical;
	public Tile floor;
	public Tile doorLocked;
	public Tile doorUnlocked;
}

// GVC = GlobalVariableContainer
public class GVC : MonoBehaviour
{
	public static GVC Instance { get; private set; } = null;

	public Tilemaps tilemap;
	public Tiles tiles;

	public GameObject PlayerGO;
	public GameObject DungeonGO;
	public GameObject StopWatchGO;
	public GameObject HealthPackPrefab;
	public Minimap Minimap;
	public GameObject Patrick;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
		PlayerGO = FindObjectOfType<Player>().transform.root.gameObject;
	}

	private void Start()
	{
		//PlayerGO = FindObjectOfType<Player>().transform.root.gameObject;
	}
}