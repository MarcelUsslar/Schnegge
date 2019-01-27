using UnityEngine;

public class FootSpawner : MonoBehaviour
{
    [SerializeField] private Foot _footPrefab;
    [SerializeField] private int _rarityPerFrame;

    private Schnegge _schnegge;

    private void Start()
    {
        _schnegge = FindObjectOfType<Schnegge>();
    }

    private void Update()
    {
        var pos = transform.position;

        transform.position = new Vector3(_schnegge.transform.position.x, _schnegge.transform.position.y + 30f, 0f);

        if (Random.Range(0, _rarityPerFrame) == 0)
            Instantiate(_footPrefab, transform.position, Quaternion.identity);
    }
}