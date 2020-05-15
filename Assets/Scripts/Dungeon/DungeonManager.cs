using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    // Static fields
    public static Tilemap tilemap_walls, tilemap_doors;
    public static Tile tile_Corridor, tile_Wall, tile_Door_Locked, tile_Door_Unlocked;
    [SerializeField]
    private Tilemap _tilemap_walls, _tilemap_doors;
    [SerializeField]
    private Tile _tile_Corridor, _tile_Wall, _tile_Door_Locked, _tile_Door_Unlocked;

    void Awake()
    {
        FillStaticFields();
    }

    void FillStaticFields()
    {
        // Fill static fields
        DungeonManager.tilemap_walls = _tilemap_walls;
        DungeonManager.tilemap_doors = _tilemap_doors;
        DungeonManager.tile_Corridor = _tile_Corridor;
        DungeonManager.tile_Wall = _tile_Wall;
        DungeonManager.tile_Door_Locked = _tile_Door_Locked;
        DungeonManager.tile_Door_Unlocked = _tile_Door_Unlocked;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void OnValidate()
    {
        FillStaticFields();
    }

    public static void ClearAllTiles()
    {
        DungeonManager.tilemap_doors.ClearAllTiles();
        DungeonManager.tilemap_walls.ClearAllTiles();
    }
}
