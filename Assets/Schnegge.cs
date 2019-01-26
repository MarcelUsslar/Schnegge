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

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Fader.FadeIn();

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Fader.FadeOut();
    }
}