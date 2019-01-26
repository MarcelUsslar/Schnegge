﻿using System;
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

        private void OnDrawGizmos()
        {
            DrawCircle(LevelStartPoint, Color.green);
            DrawCircle(LevelEndPoint, Color.red);
        }

        private void DrawCircle(Transform targetObject, Color circleColor)
        {
            Handles.color = circleColor;
            Handles.DrawWireDisc(targetObject.position, Vector3.forward, 1);
        }

        private void OnValidate()
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

        private void Reset()
        {
            OnValidate();
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