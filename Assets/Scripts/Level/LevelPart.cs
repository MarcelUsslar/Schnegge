using System;
using UnityEditor;
using UnityEngine;

namespace Scripts.Level
{
    public class LevelPart : MonoBehaviour
    {
        [SerializeField] private GameObject _levelStartPoint;
        [SerializeField] private GameObject _levelEndPoint;

        public Transform LevelStartPoint => _levelStartPoint.transform;
        public Transform LevelEndPoint => _levelEndPoint.transform;

        public void Destroy()
        {
            // TODO remove correctly
            Destroy(gameObject);
        }

        public void SetPositionAsNext(LevelPart previousPart)
        {
            if (previousPart == null)
            {
                gameObject.transform.position = Vector3.zero;
                return;
            }
            
            var ownOffset = gameObject.transform.position - LevelStartPoint.position;
            var levelOffset = previousPart.LevelEndPoint.position;

            gameObject.transform.position = levelOffset + ownOffset;
        }

        public void SetPositionAsPrevious(LevelPart nextPart)
        {
            if (nextPart == null)
            {
                gameObject.transform.position = Vector3.zero;
                return;
            }

            var ownOffset = gameObject.transform.position - LevelEndPoint.position;
            var levelOffset = nextPart.LevelStartPoint.position;

            gameObject.transform.position = levelOffset + ownOffset;
        }

        private void OnDrawGizmos()
        {
            DrawCircle(LevelStartPoint, Color.green, 0.5f);
            DrawCircle(LevelEndPoint, Color.red, 0.25f);
        }

        private void DrawCircle(Transform targetObject, Color circleColor, float size)
        {
            Handles.color = circleColor;
            Handles.DrawWireDisc(targetObject.position, Vector3.forward, size);
        }

        public void Reset()
        {
            if (_levelStartPoint == null)
            {
                _levelStartPoint = CreateObject("Start", Vector2.left);
            }
            if (_levelEndPoint == null)
            {
                _levelEndPoint = CreateObject("End", Vector2.right);
            }
        }

        private void OnValidate()
        {
            Reset();
        }

        private GameObject CreateObject(string objName, Vector2 position)
        {
            var newObject = new GameObject(objName);

            newObject.transform.SetParent(transform, false);
            newObject.transform.position = position;

            return newObject;
        }
    }
}
