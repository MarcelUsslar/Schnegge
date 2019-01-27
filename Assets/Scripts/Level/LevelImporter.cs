using System.Collections.Generic;
using System.Linq;
using Attacks;
using UnityEngine;
using TileMapGeneration;

namespace Level
{
    public class LevelImporter : MonoBehaviour
    {
        [SerializeField] private LevelImportSettings _settings;

        [SerializeField] private Texture2D _importedTexture;
        [SerializeField] private LevelPart _generatedLevel;

        private List<Vector2> _levelPoints;
        private Dictionary<Color, List<Vector2>> _flyingIslandMapping;
        private Dictionary<Color, List<Vector2>> _attackTriggerMapping;

        public void Import()
        {
            if (_importedTexture == null)
                return;
            
            Setup();

            ReadTexture();

            GenerateCollider();

            GenerateAttackTriggers();

            SetBoundingPoints();
        }

        private void Setup()
        {
            _levelPoints = new List<Vector2>();
            _flyingIslandMapping = new Dictionary<Color, List<Vector2>>();
            _attackTriggerMapping = new Dictionary<Color, List<Vector2>>();

            _generatedLevel = CreateChildObject(_importedTexture.name, null).AddComponent<LevelPart>();
            _generatedLevel.Reset();
        }

        private void ReadTexture()
        {
            for (var width = 0; width < _importedTexture.width; width++)
            {
                for (var height = _importedTexture.height - 1; height >= 0; height--)
                {
                    EvaluatePoint(width, height);
                }
            }
        }

        private void GenerateCollider()
        {
            GenerateEdgeCollider(_levelPoints, "Level", true);
            foreach (var key in _flyingIslandMapping.Keys)
            {
                GenerateEdgeCollider(_flyingIslandMapping[key], key.ToString(), false);
            }
        }

        private void GenerateEdgeCollider(List<Vector2> points, string objectName, bool generateTileMap)
        {
            var edgeColliderObject = CreateChildObject(objectName, _generatedLevel.transform);

            var edgeCollider = edgeColliderObject.AddComponent<EdgeCollider2D>();
            edgeCollider.points = points.ToArray();

            if (generateTileMap)
            {
                var tileMapGenerator = edgeColliderObject.AddComponent<TileMapGenerator>();
                tileMapGenerator.Setup(points, _settings); 
            }

            var lineRenderer = edgeColliderObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            lineRenderer.widthMultiplier = 0.1f * _settings.UnitSize;
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.Select(vec => (Vector3)vec).ToArray());
        }

        private void GenerateAttackTriggers()
        {
            foreach (var key in _attackTriggerMapping.Keys)
            {
                var attackTriggerParent = CreateChildObject($"AttackTrigger: {key}", _generatedLevel.transform);
                for (var i = 0; i < _attackTriggerMapping[key].Count; i++)
                {
                    var triggerObject = CreateChildObject($"Trigger{i}", attackTriggerParent.transform);
                    var attackTrigger = triggerObject.AddComponent<AttackTrigger>();
                    attackTrigger.Setup(_settings.UnitSize);
                    attackTrigger.transform.position = _attackTriggerMapping[key][i];
                }
            }
        }

        private void SetBoundingPoints()
        {
            _generatedLevel.LevelStartPoint.position = _levelPoints[0];
            _generatedLevel.LevelEndPoint.position = _levelPoints[_levelPoints.Count - 1];
        }

        private GameObject CreateChildObject(string objectName, Transform parent)
        {
            var edgeColliderObject = new GameObject(objectName);
            edgeColliderObject.transform.SetParent(parent);
            return edgeColliderObject;
        }

        private void EvaluatePoint(int width, int height)
        {
            var pixelColor = _importedTexture.GetPixel(width, height);

            if (pixelColor.a == 0)
                return;
            
            var action = _settings.GetAction(pixelColor);
            switch (action)
            {
                case ColorAction.None:
                    return;
                case ColorAction.GenerateColliderPoint:
                    _levelPoints.Add(_settings.GetPosition(width, height));
                    return;
                case ColorAction.GenerateFlyingIslandPoint:
                    AddPointToMapping(_flyingIslandMapping, pixelColor, _settings.GetPosition(width, height));
                    return;
                case ColorAction.GenerateAttackTrigger:
                    AddPointToMapping(_attackTriggerMapping, pixelColor, _settings.GetPosition(width, height));
                    break;
                default:
                    Debug.LogError("No definition for case: " + action + " yet");
                    return;
            }
        }

        public void AddPointToMapping(Dictionary<Color, List<Vector2>> mapping, Color color, Vector2 position)
        {
            if (!mapping.ContainsKey(color))
            {
                mapping.Add(color, new List<Vector2>());
            }

            mapping[color].Add(position);
        }
    }
}
