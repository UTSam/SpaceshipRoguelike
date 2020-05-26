using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon
{
    [System.Serializable]
    public class PleaveGiveMeGoodName
    {
        public string tilemapName = "";
        
        public List<Tile> tiles = new List<Tile>();
        public List<Vector3Int> tilePositions = new List<Vector3Int>();

        public PleaveGiveMeGoodName(string _tilemapName)
        {
            tilemapName = _tilemapName;
        }

        public void SpawnTiles(Vector3Int position, Tilemap drawOnThis = null)
        {
            Tilemap tilemap = drawOnThis;
            if(tilemap == null)
                tilemap = SetTilemap();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (drawOnThis)
                {
                    tilemap.SetTile(tilePositions[i], tiles[i]);
                }
                else
                {
                    tilemap.SetTile(tilePositions[i] + position, tiles[i]);
                }
            }
        }

        private Tilemap SetTilemap()
        {
            GameObject tilemapGo = GameObject.Find("Tilemap_" + tilemapName);

            if (tilemapGo == null)
            {
                Debug.LogError("Couldn't find the gameobject: Tilemap_" + tilemapName);
                Debug.Break();
            }
            
            if (!tilemapGo.GetComponent<Tilemap>())
            {
                Debug.LogError("Couldn't find the tilemap in gameobject: Tilemap_" + tilemapName);
                Debug.Break();
            }

            return tilemapGo.GetComponent<Tilemap>();
            
        }
    }
}
