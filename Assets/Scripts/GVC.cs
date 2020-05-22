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
	private static GVC instance = null;

	public Tilemaps tilemap;
	public Tiles tiles;

	public GameObject PlayerGO;
	public GameObject DungeonGO;
	public GameObject StopWatchGO;
	public GameObject HealthPackPrefab;

	public static GVC Instance { get { return instance; } }

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}
}