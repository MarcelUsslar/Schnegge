using UnityEngine;

public class Rock : Danger
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _triggerDistance;

    [SerializeField] private float _lifeTime;

    private Schnegge _schnegge;

    private void Start()
    {
        _schnegge = FindObjectOfType<Schnegge>();
    }

    private void Update()
    {
        if (transform.position.x - _schnegge.transform.position.x <= _triggerDistance)
        {
            OnDanger();
        }
    }

    protected override void OnDanger()
    {
        _rigidBody2D.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, _lifeTime);
    }
}