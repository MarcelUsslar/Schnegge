using UnityEditor;
using UnityEngine;

namespace Level
{
    [CustomEditor(typeof(LevelImporter))]
    public class LevelImporterEditor : Editor
    {
        private LevelImporter _levelImporter;

        public override void OnInspectorGUI()
        {
            Setup();

            DrawDefaultInspector();
            
            GUILayout.Space(5);

            if (GUILayout.Button("Import Texture"))
            {
                _levelImporter.Import();
            }
        }

        private void Setup()
        {
            if (_levelImporter != null)
                return;

            _levelImporter = (LevelImporter) target;
        }
    }
}