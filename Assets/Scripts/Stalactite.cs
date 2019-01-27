using UnityEngine;

public class Stalactite : Danger
{
    [SerializeField] private Rigidbody2D _stalactiteBot;
    [SerializeField] private Animator _animator;
    [Space(10)]
    [SerializeField] private float _gravityScale;

    private Vector3 _startingPos;
    private bool _wasReleased;

    private void Start()
    {
        _startingPos = _stalactiteBot.transform.position;
    }

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

    public void ResetBot()
    {
        _wasReleased = false;
        _animator.enabled = false;
        _stalactiteBot.gravityScale = 0;
        _stalactiteBot.velocity = Vector2.zero;
        _stalactiteBot.transform.position = _startingPos;
        _stalactiteBot.angularVelocity = 0f;
        _stalactiteBot.transform.rotation = Quaternion.identity;
    }
}