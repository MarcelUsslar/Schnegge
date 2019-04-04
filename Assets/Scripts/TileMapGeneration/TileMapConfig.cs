using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace TileMapGeneration
{
    [CreateAssetMenu(menuName = "Tilemap/Tilemap Setting", fileName = "TilemapGenerationSettings")]
    public class TileMapConfig : ScriptableObject
    {
        [SerializeField] private SpriteShape _grassShape;
        [SerializeField] private Tile[] _groundTiles;
        [Space]
        [SerializeField] private Grid _grid;
        
        public SpriteShape GrassShape => _grassShape;
        public Tile[] GroundTiles => _groundTiles;

        public Grid Grid => _grid;
    }
}