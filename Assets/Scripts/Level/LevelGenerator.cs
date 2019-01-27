using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator Instance;

        [SerializeField] private LevelPartsConfig _config;

        private List<LevelPart> _parts;
        private LevelPart _previousPart;
        private readonly List<int> _mapGenerationCode = new List<int>();

        private Schnegge _schnegge;

        private int _currentLevelIndex;

        public void ResetMap()
        {
            StopCoroutine(GenerateMap());

            _currentLevelIndex = 0;
            foreach (var part in _parts)
            {
                part.Destroy();
            }
            _parts.Clear();
            _schnegge.transform.position = Vector3.zero;

            Start();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetMap();
            }
        }

        private void Start()
        {
            _parts = new List<LevelPart>(_config.MaxLevelParts);

            StartCoroutine(GenerateMap());
        }

        private IEnumerator GenerateMap()
        {
            _previousPart = null;
            for (var i = 0; i < _config.MaxLevelParts; i++)
            {
                _previousPart = GeneratePart(_previousPart);
            }

            while (true)
            {
                if (SchneggePastPart(0))
                {
                    _previousPart = GeneratePart(_previousPart);
                    RemovePart(0);
                }

                if (SchneggeBeforePart(0))
                {
                    RegeneratePreviousPart(_parts[0]);
                    _previousPart = _parts[_parts.Count - 2];
                    RemovePart(_parts.Count - 1);
                }

                yield return new WaitForSeconds(_config.UpdateInterval);
            }
        }

        private void RemovePart(int removedPartIndex)
        {
            var removedPart = _parts[removedPartIndex];
            _parts.RemoveAt(removedPartIndex);
            removedPart.Destroy();
        }

        private bool SchneggePastPart(int checkedPartIndex)
        {
            var schneggenPosition = _schnegge.transform.position;
            var nextPart = _parts[checkedPartIndex + 1];

            return schneggenPosition.x > nextPart.LevelEndPoint.position.x;
        }

        private bool SchneggeBeforePart(int checkedPartIndex)
        {
            var schneggenPosition = _schnegge.transform.position;
            var part = _parts[checkedPartIndex];

            return RegeneratedPartIndex > _config.TutorialParts.Length && schneggenPosition.x < part.LevelEndPoint.position.x;
        }

        private LevelPart GeneratePart(LevelPart previousPart)
        {
            var part = _currentLevelIndex < _config.TutorialParts.Length ?
                GenerateTutorialPart() :
                GenerateRandomPart();
            
            Debug.LogWarning($"Set position for {part.name} with previous: {previousPart}");
            part.SetPositionAsNext(previousPart);

            _parts.Add(part);

            _currentLevelIndex++;

            return part;
        }

        private LevelPart GenerateTutorialPart()
        {
            var part = Instantiate(_config.TutorialParts[_currentLevelIndex], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, _currentLevelIndex, _currentLevelIndex, false);

            AddIndexToMap(_currentLevelIndex);

            return part;
        }

        private LevelPart GenerateRandomPart()
        {
            var partIndex = _currentLevelIndex >= _mapGenerationCode.Count
                ? Random.Range(0, _config.Parts.Length)
                : _mapGenerationCode[_currentLevelIndex];

            var part = Instantiate(_config.Parts[partIndex], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, partIndex, _currentLevelIndex - _config.TutorialParts.Length);

            AddIndexToMap(partIndex);

            return part;
        }

        private void SetPartName(Object part, int partNumber, int index, bool isRandom = true)
        {
            part.name = $"{(isRandom ? $"RandomPart" : $"TutorialPart")}_{partNumber}-Index{index}";
        }

        private void AddIndexToMap(int index)
        {
            if (_mapGenerationCode.Count <= _currentLevelIndex)
            {
                _mapGenerationCode.Add(index);
            }
        }

        private void RegeneratePreviousPart(LevelPart followingPart)
        {
            _currentLevelIndex--;

            var part = RegeneratedPartIndex < _config.TutorialParts.Length ?
                RegenerateTutorialPart() :
                RegenerateRandomPart();

            part.SetPositionAsPrevious(followingPart);

            _parts.Insert(0, part);
        }

        private int RegeneratedPartIndex => _currentLevelIndex - _config.MaxLevelParts;

        private LevelPart RegenerateRandomPart()
        {
            var part = Instantiate(_config.Parts[_mapGenerationCode[RegeneratedPartIndex]], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, _mapGenerationCode[RegeneratedPartIndex], RegeneratedPartIndex - _config.TutorialParts.Length);
            
            return part;
        }

        private LevelPart RegenerateTutorialPart()
        {
            var part = Instantiate(_config.TutorialParts[RegeneratedPartIndex], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, RegeneratedPartIndex, RegeneratedPartIndex, false);
            
            return part;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _schnegge = FindObjectOfType<Schnegge>();
        }
    }
}
