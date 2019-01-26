using UnityEngine;

public class Schnegge : MonoBehaviour
{
    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SoundService.PlaySound(Sound.Block);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Fader.FadeIn();

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Fader.FadeOut();
    }
}