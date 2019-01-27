using UnityEngine;

public class FootSpawner : MonoBehaviour
{
    [SerializeField] private Foot _footPrefab;
    [SerializeField] private int _rarityPerFrame;

    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;

    private Schnegge _schnegge;

    private void Start()
    {
        _schnegge = FindObjectOfType<Schnegge>();
    }

    private void Update()
    {
        transform.position = new Vector3(_schnegge.transform.position.x + _offsetX, _schnegge.transform.position.y + _offsetY, 0f);

        if (Random.Range(0, _rarityPerFrame) == 0)
            Instantiate(_footPrefab, transform.position, Quaternion.identity);
    }
}