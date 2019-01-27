using UnityEngine;

public class StalactiteBot : Danger
{
    protected override void OnDanger()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GetComponentInParent<Stalactite>().ResetBot();
    }
}
