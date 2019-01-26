using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private float speedFactor;
    private Vector2 speed = new Vector2(1f, 1f);
    private float width = 1;
    private float height = 1;
    [SerializeField] private Rigidbody2D schnegge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        repositionsBackground();
    }

    private void repositionsBackground()
    {
        setSpeed(schnegge.velocity);
        width = background.uvRect.width;
        height = background.uvRect.height;
        float x = background.uvRect.x + speed.x * speedFactor * Time.deltaTime;
        float y = background.uvRect.y + speed.y * speedFactor * Time.deltaTime;
        background.uvRect = new Rect(x, y, width, height);
    }

    public void setSpeed(Vector2 _speed){
        speed = _speed;
    }
}
