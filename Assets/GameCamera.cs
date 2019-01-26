using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Transform _schneggeTransform;

    private void Start()
    {
        _schneggeTransform = FindObjectOfType<Schnegge>().transform;
    }

    private void Update()
    {
        var pos = _schneggeTransform.position;

        transform.position = new Vector3(pos.x, 0, -10);
    }
}
