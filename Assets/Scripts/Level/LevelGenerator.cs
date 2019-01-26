using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        private const int KMaxLevelParts = 10;
        private const float KUpdateInterval = 5f;

        private static LevelGenerator _instance;

        [SerializeField] private LevelPartsConfig _config;

        private Schnegge _schnegge;

        private int _currentLevelIndex;
        private List<int> _mapGenerationCode;

        private readonly List<LevelPart> _parts = new List<LevelPart>(KMaxLevelParts);

        private void Start()
        {
            StartCoroutine(GenerateMap());
        }

        private IEnumerator GenerateMap()
        {
            LevelPart previousPart = null;
            for (var i = 0; i < KMaxLevelParts; i++)
            {
                previousPart = GeneratePart(previousPart);
            }

            while (true)
            {
                yield return new WaitForSeconds(KUpdateInterval);

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
            return true;
        }

        private bool SchneggeBeforePart(LevelPart checkedPart)
        {
            // TODO check if schnegge is before previous part
            return false;
        }

        private LevelPart GeneratePart(LevelPart previousPart)
        {
            var randomPart = UnityEngine.Random.Range(0, _config.Parts.Length);
            var part = Instantiate(_config.Parts[randomPart], Vector3.zero, Quaternion.identity, null);
            part.name = "Part" + randomPart + " Index" + _currentLevelIndex;

            part.SetPositionAsNext(previousPart);

            _parts.Add(part);

            _currentLevelIndex++;

            return part;
        }

        private LevelPart RegeneratePreviousPart(LevelPart followingPart)
        {
            var part = Instantiate(_config.Parts[_mapGenerationCode[_currentLevelIndex]], Vector3.zero, Quaternion.identity, null);
            part.name = "Part" + _mapGenerationCode[_currentLevelIndex] + " Index" + _currentLevelIndex;

            part.SetPositionAsPrevious(followingPart);

            _parts.Insert(0, part);

            _currentLevelIndex--;

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
