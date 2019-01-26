using System;
using System.Linq;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(menuName = "Attack/Attack Trigger Settings", fileName = "AttackTriggerSettings")]
    public class AttackMappingConfig : ScriptableObject
    {
        [Serializable]
        private class AttackTypeMapping
        {
            public Color Color;
            public AttackType AttackType;
        }

        [SerializeField] private AttackTypeMapping[] _attackMapping;

        public AttackType GetAttackType(Color color)
        {
            var foundMapping = _attackMapping.FirstOrDefault(mapping => mapping.Color.Equals(color));
            if (foundMapping == null)
                return AttackType.None;

            return foundMapping.AttackType;
        }
    }
}
