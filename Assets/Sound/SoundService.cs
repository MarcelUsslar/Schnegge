using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundService : MonoBehaviour
{
    [SerializeField] private AudioClip _mainMusic;
    [SerializeField] private Tuple[] _audioClips;

    private readonly List<AudioSource> _audioSources = new List<AudioSource>();
    private static SoundService _instance;

    [UsedImplicitly] private void Start()
    {
        _instance = this;
        Play(_mainMusic, true);
    }
    
    public static void PlaySound(Sound sound)
    {
        var sounds = _instance._audioClips.Where(tuple => tuple.Sound == sound).ToArray();
        var randomSound = sounds[Random.Range(0, sounds.Length)];

        _instance.Play(randomSound.AudioClip);
    }

    private void Play(AudioClip audioClip, bool loop = false)
    {
        var audioSource = GetAvailableAudioSource();

        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = loop;
    }

    private AudioSource GetAvailableAudioSource()
    {
        return _audioSources.Any(source => !source.isPlaying) 
            ? _audioSources.First(source => !source.isPlaying) 
            : ExpandSources();
    }

    private AudioSource ExpandSources()
    {
        var source = gameObject.AddComponent<AudioSource>();
        _audioSources.Add(source);

        return source;
    }

    [Serializable] private class Tuple
    {
        public AudioClip AudioClip;
        public Sound Sound;
    }
}