using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace TileMapGeneration
{
    public class TileVisualizer : MonoBehaviour
    {
        private enum TileType
        {
            None,
            Earth
        }

        private static TileMapConfig _config;

        private List<Vector2> _colliderPoints;
        private LevelImportSettings _settings;

        private Tilemap _tilemap;
        private Tile[] _tiles;

        private Vector3Int _tilemapSize;

        private TileType[,] _terrainMap;
        private int _height;
        private int _width;
        
        public void Setup(List<Vector2> points, LevelImportSettings settings, bool isGround)
        {
            _colliderPoints = points;
            _settings = settings;

            if (isGround)
            {
                PrepareTileMap();

                SetTerrain();

                GenerateTiles(); 
            }

            GenerateSpriteShape();
        }

        private void GenerateSpriteShape()
        {
            gameObject.AddComponent<EdgeCollider2D>();
            gameObject.AddComponent<SpriteShapeRenderer>();
            
            var shapeController = gameObject.AddComponent<SpriteShapeController>();
            shapeController.spriteShape = _config.GrassShape;
            shapeController.splineDetail = 16;
            shapeController.colliderDetail = 16;
            shapeController.colliderCornerType = ColliderCornerType.Square;

            var spline = shapeController.spline;
            spline.Clear();

            for (var i = 0; i < _colliderPoints.Count; i++)
            {
                InsertPoint(spline, i, _colliderPoints[i], (i == 0 || i == _colliderPoints.Count - 1) ? ShapeTangentMode.Linear : ShapeTangentMode.Continuous);
            }
            
            var y = _colliderPoints.Min(point => point.y) - 1;

            _colliderPoints.Add(new Vector2(_colliderPoints[_colliderPoints.Count - 1].x, y));
            _colliderPoints.Add(new Vector2(_colliderPoints[0].x, y));

            var splinePointCount = spline.GetPointCount();
            for (var i = 0; i < splinePointCount; i++)
            {
                spline.SetLeftTangent(i,
                    GetTangent(i, _colliderPoints[(i - 1 + splinePointCount) % splinePointCount],
                        _colliderPoints[(i + 1 + splinePointCount) % splinePointCount], true));
                spline.SetRightTangent(i,
                    GetTangent(i, _colliderPoints[(i - 1 + splinePointCount) % splinePointCount],
                        _colliderPoints[(i + 1 + splinePointCount) % splinePointCount], false));
            }

            InsertPoint(spline, splinePointCount, new Vector2(_colliderPoints[_colliderPoints.Count - 2].x, y), ShapeTangentMode.Linear);
            InsertPoint(spline, splinePointCount + 1, new Vector2(_colliderPoints[_colliderPoints.Count - 1].x, y), ShapeTangentMode.Linear);

            shapeController.BakeCollider();
        }

        private Vector3 GetTangent(int currentIndex, Vector2 beforePoint, Vector2 afterPoint, bool previous)
        {
            var beforeDiff = _colliderPoints[currentIndex] - beforePoint;
            var afterDiff = _colliderPoints[currentIndex] - afterPoint;

            if (beforeDiff.normalized == afterDiff.normalized)
            {
                return 0.25f * (previous ? beforeDiff : afterDiff);
            }

            var normal = (afterDiff.normalized + beforeDiff.normalized).normalized;
            if (normal.y < 0)
            {
                normal *= -1;
            }
            
            var tangent = Vector3.Cross(previous ? Vector3.forward : Vector3.back, normal).normalized;

            return tangent * 0.25f * (previous ? beforeDiff.magnitude : afterDiff.magnitude);
        }

        private void InsertPoint(Spline spline, int index, Vector3 point, ShapeTangentMode mode)
        {
            spline.InsertPointAt(index, point);
            spline.SetTangentMode(index, mode);
            spline.SetSpriteIndex(index, UnityEngine.Random.Range(0, _config.GrassShape.angleRanges[0].sprites.Count));
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

            _height += _settings.MaxGroundTiles;
            
            _terrainMap = new TileType[_width, _height];

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    _terrainMap[w, h] = TileType.None;
                }
            }
        }

        private void SetTerrain()
        {
            for (var w = 0; w < _width; w++)
            {
                var terrainHeight = GetTerrainHeight(w);
                var grassHeight =  -1;

                for (var h = 0; h < _height; h++)
                {
                    var currentTileHeight = GetWorldHeight(h);

                    if (currentTileHeight > terrainHeight)
                        continue;

                    grassHeight = h;
                    break;
                }

                if (grassHeight >= 0)
                {
                    for (var height = grassHeight; height < _height && height <= grassHeight + _settings.MaxGroundTiles; height++)
                    {
                        _terrainMap[w, height] = TileType.Earth;
                    }
                }
            }
        }

        private void GenerateTiles()
        {
            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    var tileType = _terrainMap[w, h];
                    if (tileType == TileType.None)
                        continue;
                    
                    var tile = GetTile(tileType);

                    _tilemap.SetTile(
                        new Vector3Int(w, _height - (h + 1), 0),
                        tile);
                }
            }
        }

        private TileBase GetTile(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Earth:
                    return _config.GroundTiles[UnityEngine.Random.Range(0, _config.GroundTiles.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
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
