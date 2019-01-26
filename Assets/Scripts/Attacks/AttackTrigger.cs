using UnityEngine;

namespace Attacks
{
    public class AttackTrigger : MonoBehaviour
    {
        public void Setup(float unitSize)
        {
            var triggerCollider = gameObject.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
            triggerCollider.size = new Vector2(0.5f * unitSize, 3f * unitSize);
            triggerCollider.offset = new Vector2(0f, -0.75f * unitSize);
        }
    }
}
