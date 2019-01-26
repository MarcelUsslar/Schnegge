using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Level
{
    public class LevelGenerator : MonoBehaviour
    {
        private static LevelGenerator _instance;

        private List<LevelPart> _parts;
        private readonly List<int> _mapGenerationCode = new List<int>();

        [SerializeField] private LevelPartsConfig _config;

        private Schnegge _schnegge;

        private int _currentLevelIndex;
        
        private void Start()
        {
            _parts = new List<LevelPart>(_config.MaxLevelParts);

            StartCoroutine(GenerateMap());
        }

        private IEnumerator GenerateMap()
        {
            LevelPart previousPart = null;
            for (var i = 0; i < _config.MaxLevelParts; i++)
            {
                previousPart = GeneratePart(previousPart);
            }

            while (true)
            {
                yield return new WaitForSeconds(_config.UpdateInterval);

                if (SchneggePastPart(_parts[0]))
                {
                    previousPart = GeneratePart(previousPart);
                    RemovePart(0);
                }

                if (SchneggeBeforePart(_parts[0]))
                {
                    previousPart = RegeneratePreviousPart(_parts[0]);
                    RemovePart(_parts.Count - 1);
                }
            }
        }

        private void RemovePart(int removedPartIndex)
        {
            var removedPart = _parts[removedPartIndex];
            _parts.RemoveAt(removedPartIndex);
            removedPart.Destroy();
        }

        private bool SchneggePastPart(LevelPart checkedPart)
        {
            // TODO check if schnegge is past last part
            return false;
        }

        private bool SchneggeBeforePart(LevelPart checkedPart)
        {
            // TODO check if schnegge is before previous part
            return false;
        }

        private LevelPart GeneratePart(LevelPart previousPart)
        {
            var part = _currentLevelIndex < _config.TutorialParts.Length ?
                GenerateTutorialPart() :
                GenerateRandomPart();
            
            part.SetPositionAsNext(previousPart);

            _parts.Add(part);

            _currentLevelIndex++;

            return part;
        }

        private LevelPart GenerateRandomPart()
        {
            var randomPart = UnityEngine.Random.Range(0, _config.Parts.Length);
            var part = Instantiate(_config.Parts[randomPart], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, randomPart, _currentLevelIndex - _config.TutorialParts.Length);

            AddIndexToMap(randomPart);

            return part;
        }

        private LevelPart GenerateTutorialPart()
        {
            var part = Instantiate(_config.TutorialParts[_currentLevelIndex], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, _currentLevelIndex, _currentLevelIndex, false);

            AddIndexToMap(_currentLevelIndex);

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

        private LevelPart RegeneratePreviousPart(LevelPart followingPart)
        {
            _currentLevelIndex--;

            var part = _currentLevelIndex < _config.TutorialParts.Length ?
                RegenerateTutorialPart() :
                RegenerateRandomPart();

            part.SetPositionAsPrevious(followingPart);

            _parts.Insert(0, part);

            return part;
        }

        private LevelPart RegenerateRandomPart()
        {
            var part = Instantiate(_config.Parts[_mapGenerationCode[_currentLevelIndex]], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, _mapGenerationCode[_currentLevelIndex], _currentLevelIndex - _config.TutorialParts.Length);

            AddIndexToMap(_mapGenerationCode[_currentLevelIndex]);

            return part;
        }

        private LevelPart RegenerateTutorialPart()
        {
            var part = Instantiate(_config.TutorialParts[_currentLevelIndex], Vector3.zero, Quaternion.identity, null);
            SetPartName(part, _currentLevelIndex, _currentLevelIndex, false);

            AddIndexToMap(_currentLevelIndex);

            return part;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            _schnegge = FindObjectOfType<Schnegge>();
        }
    }
}
