﻿using UnityEngine;

public class FootSpawner : MonoBehaviour
{
    [SerializeField] private Foot _footPrefab;
    
    [SerializeField] private float _startingRarity;

    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;

    private Schnegge _schnegge;

    private void Start()
    {
        _schnegge = FindObjectOfType<Schnegge>();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(_schnegge.transform.position.x + _offsetX, _schnegge.transform.position.y + _offsetY, 0f);

        if (Random.Range(0.0f, _startingRarity * ScoreNumber) <
            (_startingRarity * ScoreNumber < 36 ?
                _startingRarity * ScoreNumber / 180.0f :
                0.2f))
            Instantiate(_footPrefab, transform.position, Quaternion.identity);
    }

    private float ScoreNumber => ScoreCounter.Instance.ScoreCountingSpeed /
                                 (ScoreCounter.Instance.Score > 0 ? ScoreCounter.Instance.Score : 1);
}