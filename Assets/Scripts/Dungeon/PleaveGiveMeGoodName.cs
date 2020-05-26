using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon
{
    [System.Serializable]
    public class PleaveGiveMeGoodName
    {
        [SerializeField]
        private string tilemapString = "";
        
        public List<Tile> tiles = new List<Tile>();
        public List<Vector3Int> tilePositions = new List<Vector3Int>();

        public PleaveGiveMeGoodName(string _tilemapName)
        {
            tilemapString = _tilemapName;
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
            GameObject tilemapGo = GameObject.Find("Tilemap_" + tilemapString);
            
            if (tilemapGo.GetComponent<Tilemap>())
            {
                return tilemapGo.GetComponent<Tilemap>();
            }
            else
            {
                Debug.LogError("Couldn't find the tilemap: Tilemap_" + tilemapString);
            }
            
            return null;
        }
    }
}
