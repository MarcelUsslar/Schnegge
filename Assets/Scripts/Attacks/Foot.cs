using UnityEngine;

public class Foot : Danger
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;

    private bool _hitObject = false;

    private void Start()
    {
        OnDanger();
    }

    private void Update()
    {
        if (!_hitObject)
            return;

        _rigidBody2D.velocity = new Vector2(_horizontalSpeed, _verticalSpeed);
        _rigidBody2D.angularVelocity = 0;
    }

    protected override void OnDanger()
    {
        SoundService.PlaySound(Sound.Kick);
        _rigidBody2D.velocity = new Vector2(0f, -_fallSpeed);
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        LiftFoot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LiftFoot();
    }

    private void LiftFoot()
    {
        _hitObject = true;
        Update();
        Destroy(gameObject, 2f);
    }
}
