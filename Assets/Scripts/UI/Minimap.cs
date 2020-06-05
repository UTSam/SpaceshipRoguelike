using Assets.Scripts.Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Assets.Scripts.Dungeon.TileData;

public class Minimap : MonoBehaviour
{
    private DungeonGenerator dungeon;
    private Tilemap tilemap;

    private Transform player;
    private Texture2D texture;
    [SerializeField] private Texture2D playerIndicator;

    private Texture2D test_texture;

    private int dx, dy;

    [SerializeField]
    [Range(20, 500)]
    private float mapSize = 200;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float mapScale = 2f;

    private Vector3 playerPos = new Vector3();

    [SerializeField]
    [Range(1, 30)]
    private float playerIndicatorSize = 10;

    private void Start()
    {
        player = GVC.Instance.PlayerGO.transform;
        tilemap = GVC.Instance.tilemap.walls;
        dungeon = GVC.Instance.DungeonGO.GetComponent<DungeonGenerator>();
        test_texture = new Texture2D(1, 1);
        test_texture.SetPixel(0, 0, Color.black);
        test_texture.Apply();
    }

    //void OnValidate()
    //{
    //    Generate();
    //}

    public void Generate()
    {
        int padding = (int) (mapSize / mapScale);

        // Create new texture with width and height of the tilemap
        Vector3Int tilemapSize = tilemap.cellBounds.size;
        int width = tilemapSize.x + 1 + padding;
        int height = tilemapSize.y + 1 + padding;
        texture = new Texture2D(width, height);

        // Get delta x and y to the top left of the tilemap bounds (and the texture)
        dx = tilemap.cellBounds.xMin;
        dy = tilemap.cellBounds.yMin;

        // DungeonGenerator component
        DungeonGenerator dg = dungeon.GetComponent(typeof(DungeonGenerator)) as DungeonGenerator;

        // New array with pixel colors 
        Color[] colors = new Color[width * height];

        // Loop through each room
        foreach (Room r in dg.placedRooms)
        {
            // Get TileData object with Walls 
            TileData tileData = null;
            foreach (TileData td in r.tileDataArray)
            {
                if (td.GetTilemapString().ToLower().Contains("walls"))
                {
                    tileData = td;
                }
            }
            if (tileData == null) continue;

            // Draw wall tiles
            for (int i = 0; i < tileData.tiles.Count; i++)
            {
                if (tileData.tiles[i] == null) continue;

                // Set pixel if it's a wall tile
                Vector3Int tilePos = tileData.tileLocalPositions[i];

                // Get x/y relative to room pos
                int x = tilePos.x;
                int y = tilePos.y;

                // Get absolute x and y of tile
                x = x + r.globalPosition.x;
                y = y + r.globalPosition.y;

                // Translate to texture position
                x = x - dx;
                y = y - dy;

                // Get color
                Color c = Color.blue;
                if (!r.playerEntered) c = Color.grey;

                // Set pixel
                colors[(y * width) + x] = c;
            }

            // Draw doors 
            foreach (Door door in r.doors)
            {
                // Continue if door is not connected
                if (!door.connected) continue;

                // Get absolute position
                Vector3Int pos = r.globalPosition + door.position;

                // translate x and & to texture position 
                int x = pos.x - dx;
                int y = pos.y - dy;

                colors[(y * width) + x] = Color.cyan;

                // Draw connected corridors
                if (door.corridor != null)
                {
                    TileData corridorTD = door.corridor.tileData;

                    // Draw wall tiles
                    for (int i = 0; i < corridorTD.tiles.Count; i++)
                    {
                        if (corridorTD.tiles[i] == null) continue;

                        // Check tilemap tile 
                        Tile t = (Tile)tilemap.GetTile(corridorTD.tileLocalPositions[i]);
                        if (t == null) continue;
                        if (!t.name.ToLower().Contains("wall")) continue;

                        // Translate position to texture
                        x = corridorTD.tileLocalPositions[i].x - dx;
                        y = corridorTD.tileLocalPositions[i].y - dy;

                        // Get color
                        Color c = Color.blue;

                        // Set color to gray if either of the connected rooms is not visited
                        foreach (Door d in door.corridor.connectedDoors)
                        {
                            if (!d.room.playerEntered) c = Color.gray;
                        }

                        // Set pixel
                        colors[y * width + x] = c;
                    }
                }
            }
        }

        // Set texture
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colors);

        texture.Apply();
    }
    void OnGUI()
    {
        if (!texture)
        {
            // No texture to draw
            return;
        }

        // Create positionRect (Rectangle where the map will be displayed)
        int w = (int)mapSize;
        int h = (int)mapSize;
        int x = Screen.width - w - 20;
        int y = 20;
        Rect positionRect = new Rect(x, y, w, h);

        // Draw map texture for testing
        //GUI.DrawTexture(
        //    new Rect(0, 0, texture.width, texture.height),
        //    texture);

        //////////////////////////////////////////////////////////////////////
        // Create coordinates Rect that centers on the player
        //////////////////////////////////////////////////////////////////////

        // Translate player position to texture percentage
        playerPos = new Vector3(
            (player.position.x - dx) / texture.width,
            (player.position.y - dy) / texture.height);

        // convert mapSize to scale_x and scale_y
        float scale_x = mapSize / texture.width / mapScale;
        float scale_y = mapSize / texture.height / mapScale;

        //Vector2 position = new Vector2(playerPos.x, playerPos.y);
        Vector2 position = new Vector2(playerPos.x - 0.5f * scale_x, playerPos.y - 0.5f * scale_y);
        Vector2 size = new Vector2(scale_x, scale_y);

        Rect texturecoord = new Rect(position, size);

        // Draw map texture
        GUI.DrawTextureWithTexCoords(
            positionRect,
            texture,
            texturecoord);

        // Draw player indicator 
        Rect indicatorRect = new Rect(
                positionRect.x + 0.5f * positionRect.width - playerIndicatorSize * 0.5f,
                positionRect.y + 0.5f * positionRect.height - playerIndicatorSize * 0.5f,
                playerIndicatorSize, playerIndicatorSize
            );
        GUI.DrawTexture(
            indicatorRect,
            playerIndicator);

    }
}
