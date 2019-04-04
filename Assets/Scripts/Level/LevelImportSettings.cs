using System;
using System.Linq;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Level/Level Import Setting", fileName = "LevelImportSettings")]
    public class LevelImportSettings : ScriptableObject
    {
        [Serializable]
        private class ColorMapping
        {
            public Color Color;
            public ColorAction ColorAction;
        }

        [SerializeField] private int _pixelPerUnit;
        [SerializeField] private float _unitSize;
        [SerializeField] private int _maxGroundTiles;
        [SerializeField] private ColorMapping[] _colorMapping;

        public float UnitSize => _unitSize;
        public int MaxGroundTiles => _maxGroundTiles;

        public Vector2 GetPosition(int pixelWidth, int pixelHeight)
        {
            return new Vector2(pixelWidth * _unitSize / _pixelPerUnit, pixelHeight * _unitSize / _pixelPerUnit);
        }

        public ColorAction GetAction(Color color)
        {
            var foundMapping = _colorMapping.FirstOrDefault(mapping => mapping.Color.Equals(color));
            if (foundMapping == null)
                return ColorAction.None;

            return foundMapping.ColorAction;
        }
    }
}
