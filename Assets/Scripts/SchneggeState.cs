using UnityEngine;

public class SchneggeState : MonoBehaviour
{
    public State State;
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
}