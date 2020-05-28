﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    // Static fields
    public static Player player;
    public static Tilemap tilemap_walls, tilemap_doors, tilemap_floors;
    public static Tile tile_Corridor_Horizontal, tile_Corridor_Vertical, tile_Floor, tile_Door_Locked, tile_Door_Unlocked;
    [SerializeField]
    private Tilemap _tilemap_walls, _tilemap_doors, _tilemap_floors;
    [SerializeField]
    private Tile _tile_Corridor_Horizontal, _tile_Corridor_Vertical, _tile_Floor, _tile_Door_Locked, _tile_Door_Unlocked;
    [SerializeField]
    public Player _player;


    void Awake()
    {
        FillStaticFields();
    }

    void FillStaticFields()
    {
        // Fill static fields
        DungeonManager.tilemap_walls = _tilemap_walls;
        DungeonManager.tilemap_doors = _tilemap_doors;
        DungeonManager.tilemap_floors = _tilemap_floors;
        DungeonManager.tile_Corridor_Horizontal = _tile_Corridor_Horizontal;
        DungeonManager.tile_Corridor_Vertical = _tile_Corridor_Vertical;
        DungeonManager.tile_Floor = _tile_Floor;
        DungeonManager.tile_Door_Locked = _tile_Door_Locked;
        DungeonManager.tile_Door_Unlocked = _tile_Door_Unlocked;
        DungeonManager.player = _player;
    }

    public static Player GetPlayer()
    {
        return player;
    }

    void OnValidate()
    {
        FillStaticFields();
    }

    public static void ClearAllTiles()
    {
        DungeonManager.tilemap_doors.ClearAllTiles();
        DungeonManager.tilemap_walls.ClearAllTiles();
        DungeonManager.tilemap_floors.ClearAllTiles();
    }
}