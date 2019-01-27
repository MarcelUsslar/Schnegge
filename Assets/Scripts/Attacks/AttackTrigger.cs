using UnityEngine;

namespace Attacks
{
    public class AttackTrigger : MonoBehaviour
    {
        private static AttackMappingConfig _attackMappingConfig;
        [SerializeField] private Color _key;

        private Danger _danger;

        public void Setup(Color key, float unitSize)
        {
            _key = key;
            var triggerCollider = gameObject.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
            triggerCollider.size = new Vector2(0.5f * unitSize, 30f * unitSize);
        }

        private void Awake()
        {
            gameObject.layer = 8;

            if (_attackMappingConfig != null)
                return;

            _attackMappingConfig = Resources.Load<AttackMappingConfig>("AttackTriggerSettings");
        }

        private void Start()
        {
            _danger = Instantiate(_attackMappingConfig.GetAttackPrefab(_key), gameObject.transform, false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _danger.MakeDanger();
        }
    }
}
