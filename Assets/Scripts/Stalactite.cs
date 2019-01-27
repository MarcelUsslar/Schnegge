using UnityEngine;

public class Stalactite : Danger
{
    [SerializeField] private Rigidbody2D _stalactiteBot;
    [SerializeField] private Animator _animator;
    [Space(10)]
    [SerializeField] private float _gravityScale;

    private bool _wasReleased;

    protected override void OnDanger()
    {
        if (_wasReleased)
            return;

        _wasReleased = true;

        _animator.enabled = true;
        Invoke(nameof(DropStalactite), 1f);
    }

    private void DropStalactite()
    {
        _stalactiteBot.gravityScale = _gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}