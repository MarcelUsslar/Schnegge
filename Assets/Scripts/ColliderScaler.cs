using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderScaler : MonoBehaviour
{
    private RectTransform _rectTransform;

    private List<BoxCollider2D> _boxColliderList;
    private List<CircleCollider2D> _circleColliderList;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _boxColliderList = GetComponents<BoxCollider2D>().ToList();
        _circleColliderList = GetComponents<CircleCollider2D>().ToList();
    }

    private void Update()
    {
        foreach (var boxCollider in _boxColliderList)
        {
            boxCollider.size = _rectTransform.rect.size;
        }

        foreach (var circleCollider in _circleColliderList)
        {
            var size = _rectTransform.rect.size;
            var minXY = Mathf.Min(size.x, size.y);
            circleCollider.radius = minXY / 2;
        }
    }
}
