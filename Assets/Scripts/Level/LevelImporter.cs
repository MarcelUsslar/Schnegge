using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scripts.Level
{
    public class LevelImporter : MonoBehaviour
    {
        [SerializeField] private LevelImportSettings _settings;
        [SerializeField] private Vector2 _levelOffset;

        [SerializeField] private Texture2D _importedTexture;
        [SerializeField] private LevelPart _generatedLevel;

        private List<Vector2> _levelPoints;
        private Dictionary<Color, List<Vector2>> _flyingIslandMapping;

        public void Import()
        {
            if (_importedTexture == null)
                return;
            
            Setup();

            GenerateCollider();
        }

        private void Setup()
        {
            _levelPoints = new List<Vector2>();
            _flyingIslandMapping = new Dictionary<Color, List<Vector2>>();
            
            _generatedLevel = new GameObject("LevelPart").AddComponent<LevelPart>();
            _generatedLevel.Reset();
        }

        private void GenerateCollider()
        {
            for (var width = 0; width < _importedTexture.width; width++)
            {
                for (var height = _importedTexture.height - 1; height >= 0; height--)
                {
                    GeneratePoint(width, height);
                }
            }

            GenerateEdgeCollider(_levelPoints, "Level");
            foreach (var key in _flyingIslandMapping.Keys)
            {
                GenerateEdgeCollider(_flyingIslandMapping[key], key.ToString());
            }

            SetBoundingPoints();
        }

        private void SetBoundingPoints()
        {
            _generatedLevel.LevelStartPoint.position = _levelPoints[0];
            _generatedLevel.LevelEndPoint.position = _levelPoints[_levelPoints.Count - 1];
        }

        private void GenerateEdgeCollider(List<Vector2> points, string objectName)
        {
            var edgeColliderObject = new GameObject(objectName);
            edgeColliderObject.transform.SetParent(_generatedLevel.gameObject.transform);

            var edgeCollider = edgeColliderObject.AddComponent<EdgeCollider2D>();
            edgeCollider.points = points.ToArray();
            
            var lineRenderer = edgeColliderObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.Select(vec => (Vector3) vec).ToArray());
        }

        private void GeneratePoint(int width, int height)
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
                    _levelPoints.Add(_settings.GetPosition(width, height) + _levelOffset);
                    return;
                case ColorAction.GenerateFlyingIslandPoint:
                    if (!_flyingIslandMapping.ContainsKey(pixelColor))
                    {
                        _flyingIslandMapping.Add(pixelColor, new List<Vector2>());
                    }
                    _flyingIslandMapping[pixelColor].Add(_settings.GetPosition(width, height) + _levelOffset);
                    return;
                case ColorAction.GenerateAttackTrigger:
                    break;
                default:
                    Debug.LogError("No definition for case: " + action + " yet");
                    return;
            }
        }
    }
}
