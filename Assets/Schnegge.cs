using JetBrains.Annotations;
using UnityEngine;

public class Schnegge : MonoBehaviour
{
    [UsedImplicitly] private void Start()
    {
        
    }
    
    [UsedImplicitly] private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SoundService.PlaySound(Sound.Block);
    }
}