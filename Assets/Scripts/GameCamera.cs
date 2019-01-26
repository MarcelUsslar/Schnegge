using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Vector2 _cameraOffset;

    private Transform _schneggeTransform;

    private void Start()
    {
        _schneggeTransform = FindObjectOfType<Schnegge>().transform;
    }

    private void Update()
    {
        var pos = _schneggeTransform.position;

        transform.position = new Vector3(pos.x + _cameraOffset.x, pos.y + _cameraOffset.y, -10);
    }
}
