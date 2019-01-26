using UnityEngine;

namespace Scripts.Level
{
    [CreateAssetMenu(menuName = "Level/Level Parts Config", fileName = "LevelPartsConfig")]
    public class LevelPartsConfig : ScriptableObject
    {
        [SerializeField] private int _maxLevelParts = 10;
        [SerializeField] private float _updateInterval = 1f;
        [Space]
        [SerializeField] private LevelPart[] _tutorialParts;
        [Space]
        [SerializeField] private LevelPart[] _parts;

        public int MaxLevelParts => _maxLevelParts;
        public float UpdateInterval => _updateInterval;

        public LevelPart[] TutorialParts => _tutorialParts;
        public LevelPart[] Parts => _parts;
    }
}
