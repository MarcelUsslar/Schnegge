using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMapGeneration
{
    public class TileMapGenerator : MonoBehaviour
    {
        private static TileMapConfig _config;

        private List<Vector2> _colliderPoints;
        private LevelImportSettings _settings;

        private Tilemap _tilemap;
        private Tile[] _tiles;

        private Vector3Int _tilemapSize;

        private int[,] _terrainMap;
        private int _height;
        private int _width;

        public void Setup(List<Vector2> points, LevelImportSettings settings)
        {
            _colliderPoints = points;
            _settings = settings;

            PrepareTileMap();

            SetTerrain();

            GenerateTiles();
        }

        private void PrepareTileMap()
        {
            if (_config == null)
            {
                _config = Resources.Load<TileMapConfig>("TilemapGenerationSettings");
            }

            var grid = Instantiate(_config.Grid, transform, false);

            _tilemap = grid.GetComponentInChildren<Tilemap>();
            _tilemap.ClearAllTiles();

            _width = Mathf.CeilToInt(_colliderPoints[_colliderPoints.Count - 1].x) - Mathf.FloorToInt(_colliderPoints[0].x);
            _height = Mathf.CeilToInt(_colliderPoints.Max(point => point.y)) - Mathf.FloorToInt(_colliderPoints.Min(point => point.y));

            Debug.LogWarning($"Width: {_width} Height: {_height}");

            _terrainMap = new int[_width, _height];

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    _terrainMap[w, h] = -1;
                }
            }
        }

        private void SetTerrain()
        {
            for (var w = 0; w < _width; w++)
            {
                var terrainHeight = GetTerrainHeight(w);

                for (var h = 0; h < _height; h++)
                {
                    var currentTileHeight = GetWorldHeight(h);
                    var previousTileHeight = GetWorldHeight(h - 1);

                    if (currentTileHeight > terrainHeight && previousTileHeight > terrainHeight)
                        continue;
                    
                    var groundTileCount = _config.GroundTiles.Length;
                    _terrainMap[w, h] = previousTileHeight <= terrainHeight ?
                        Random.Range(0, groundTileCount) :
                        Random.Range(groundTileCount, groundTileCount + _config.GrassTiles.Length);
                }
            }
        }

        private void GenerateTiles()
        {
            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if (_terrainMap[w, h] < 0)
                        continue;

                    var tileNumber = _terrainMap[w, h];
                    
                    var tile = tileNumber < _config.GroundTiles.Length
                        ? _config.GroundTiles[tileNumber]
                        : _config.GrassTiles[tileNumber - _config.GroundTiles.Length];

                    _tilemap.SetTile(
                        new Vector3Int(w, _height - (h + 1), 0),
                        tile);
                }
            }
        }
        
        private float GetWorldWidth(int axisValue)
        {
            return axisValue + 0.5f;
        }

        private float GetWorldHeight(int height)
        {
            return _height - (height + 0.5f);
        }

        private float GetTerrainHeight(int width)
        {
            var positionX = GetWorldWidth(width);

            var pointBefore = GetPreviousPointIndex(positionX);

            var previousPoint = _colliderPoints[pointBefore];
            var followingPoint =
                _colliderPoints[pointBefore < _colliderPoints.Count - 1 ?
                    pointBefore + 1 :
                    pointBefore];

            var percentage = (positionX - previousPoint.x) / (followingPoint.x - previousPoint.x);
            
            return Vector3.Lerp(previousPoint, followingPoint, percentage).y;
        }

        private int GetPreviousPointIndex(float positionX)
        {
            for (var i = 0; i < _colliderPoints.Count; i++)
            {
                if (positionX - _colliderPoints[i].x > 0)
                    continue;
                
                return i > 0 ? i - 1 : i;
            }

            return _colliderPoints.Count - 1;
        }
    }
}
