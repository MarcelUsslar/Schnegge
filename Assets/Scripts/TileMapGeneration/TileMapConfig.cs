using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMapGeneration
{
    [CreateAssetMenu(menuName = "Tilemap/Tilemap Setting", fileName = "TilemapGenerationSettings")]
    public class TileMapConfig : ScriptableObject
    {
        [SerializeField] private Tile[] _grassTiles;
        [SerializeField] private Tile[] _groundTiles;
        [Space]
        [SerializeField] private Grid _grid;

        public Tile[] GrassTiles => _grassTiles;
        public Tile[] GroundTiles => _groundTiles;

        public Grid Grid => _grid;
    }
}