using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _fadingTime;
    [SerializeField] private GameObject _textHighscore;
    private float _currentAlpha = 1;
    private bool _fadedIn;

    private bool _fadingIn;
    private bool _fadingOut;

    private static Fader _instance;

    [UsedImplicitly] private void Start()
    {
        _instance = this;
        //image.color = Color.black
    }

    public static void FadeIn()
    {
        if (_instance._fadedIn || _instance._fadingOut)
            return;

        _instance._fadingIn = true;

    }

    public static void FadeOut()
    {
        if (!_instance._fadedIn || _instance._fadingIn)
            return;

        _instance._fadingOut = true;
        
    }

    [UsedImplicitly]
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeIn();
            _textHighscore.SetActive(false);
        }
        if (_fadingOut)
            IncrementAlpha(Time.deltaTime/_fadingTime);

        if (_fadingIn)
            IncrementAlpha(-1 * Time.deltaTime / _fadingTime);
    }

    private void IncrementAlpha(float increment)
    {
        _currentAlpha += increment;

        if (_currentAlpha >= 1)
        {
            _fadedIn = false;
            _fadingOut = false;
            _currentAlpha = 1;
        }

        if (_currentAlpha <= 0)
        {
            _fadedIn = true;
            _fadingIn = false;
            _currentAlpha = 0;
        }

        _image.color = new Color(1, 1, 1, _currentAlpha);
    }
}