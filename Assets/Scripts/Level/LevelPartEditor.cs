using UnityEditor;
using UnityEngine;

namespace Scripts.Level
{
    [CustomEditor((typeof(LevelPart)))]
    public class LevelPartEditor : Editor
    {
        private LevelPart _part;

        public override void OnInspectorGUI()
        {
            ValidateScript();

            DrawDefaultInspector();


        }

        private void ValidateScript()
        {
            if (_part != null)
                return;

            _part = (LevelPart) target;
        }

        private void OnSceneGUI()
        {
            ValidateScript();

            DrawPickableArrows(_part.LevelStartPoint);
            DrawPickableArrows(_part.LevelEndPoint);
        }

        private void DrawPickableArrows(Transform targetObject)
        {
            EditorGUI.BeginChangeCheck();
            var newPosition =
                Handles.PositionHandle(targetObject.position, targetObject.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetObject, $"Changed Positions of {targetObject.name}");
                targetObject.position = newPosition;
            }
        }
    }
}
