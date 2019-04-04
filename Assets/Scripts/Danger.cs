using UnityEngine;

public abstract class Danger : MonoBehaviour
{
    public void MakeDanger()
    {
        OnDanger();
    }

    protected abstract void OnDanger();
}