using UnityEngine;

namespace Scripts.Level
{
    [CreateAssetMenu(menuName = "Level/Level Parts Config")]
    public class LevelPartsConfig : ScriptableObject
    {
        [SerializeField] private LevelPart[] _parts;

        public LevelPart[] Parts => _parts;
    }
}
