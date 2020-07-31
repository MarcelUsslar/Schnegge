using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootSpawner : MonoBehaviour
{
    [SerializeField] private Foot _footPrefab;
    
    [SerializeField] private int _minimumScore;
    [SerializeField] private int _maximumScore;
    [SerializeField] private float _minimumSpawnDelay;
    [SerializeField] private float _maximumSpawnDelay;
    [SerializeField] private float _spawnRandomOffset;

    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;

    private Schnegge _schnegge;

    private float _spawnDelta;
    private float _scoreDelta;

    private void Start()
    {
        _schnegge = FindObjectOfType<Schnegge>();

        _spawnDelta = _maximumSpawnDelay - _minimumSpawnDelay;
        _scoreDelta = _maximumScore - _minimumScore;

        StartCoroutine(SpawnFeet());
    }

    private IEnumerator SpawnFeet()
    {
        yield return null;
        while (true)
        {
            yield return new WaitWhile(() => _schnegge.IsDead);

            var score = ScoreCounter.Instance.Score;
            if (score <= _minimumScore)
            {
                yield return null;
            }
            else
            {
                var offset = Mathf.Lerp(_spawnDelta, 0f,
                    (score - _minimumScore) / _scoreDelta);
                var delay = _minimumSpawnDelay + offset + Random.Range(- _spawnRandomOffset, _spawnRandomOffset);

                yield return new WaitForSeconds(delay);

                if (!_schnegge.IsDead && ScoreCounter.Instance.Score > _minimumScore)
                {
                    Instantiate(_footPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    private void Update()
    {
        var schneggePosition = _schnegge.transform.position;
        transform.position = new Vector3(schneggePosition.x + _offsetX, schneggePosition.y + _offsetY, 0f);
    }
}