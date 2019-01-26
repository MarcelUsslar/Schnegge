using UnityEngine;

public class SchneggeState : MonoBehaviour
{
    public State State;
    public bool IsActive
    {
        set { gameObject.SetActive(value); }
    }
}