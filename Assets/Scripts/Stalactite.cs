﻿using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _stalactiteBot;
    [SerializeField] private Animator _animator;
    [Space(10)]
    [SerializeField] private float _gravityScale;

    private bool _wasReleased;

    public void Release()
    {
        if (_wasReleased)
            return;

        _wasReleased = true;

        _animator.SetTrigger("Wiggle");
        Invoke(nameof(DropStalactite), 1f);
    }

    private void DropStalactite()
    {
        _stalactiteBot.gravityScale = _gravityScale;
    }
}
