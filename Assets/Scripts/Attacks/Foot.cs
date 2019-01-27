using UnityEngine;

public class Foot : Danger
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _speed;

    private void Start()
    {
        OnDanger();
    }

    protected override void OnDanger()
    {
        SoundService.PlaySound(Sound.Kick);
        _rigidBody2D.velocity = new Vector2(0f, -_speed);
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _rigidBody2D.velocity = new Vector2(0f, -_speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _rigidBody2D.velocity = new Vector2(0f, -_speed);
    }
}
