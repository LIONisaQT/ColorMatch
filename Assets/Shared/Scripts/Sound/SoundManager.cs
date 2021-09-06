using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int FADE_DURATION = 2;
    private const float MAX_VOLUME = 0.4f;

    [SerializeField] private SoundLibrary _sfxLibrary;
    [SerializeField] private SoundLibrary _bgmLibrary;

    [SerializeField] private AudioSource _soundClipAudioSource;
    [SerializeField] private AudioSource _bgmAudioSource1;
    [SerializeField] private AudioSource _bgmAudioSource2;

    private bool _source1Active = true;

    private void Awake()
    {
        _bgmAudioSource1.volume = 1;
        _bgmAudioSource2.volume = 0;

        _sfxLibrary.InitializeDictionary();
        _bgmLibrary.InitializeDictionary();
    }

    public AudioClip GetAudioClip(string name)
    {
        return FindClip(name) ?? null;
    }

    private AudioClip FindClip(string name)
    {
        var result = _sfxLibrary.GetClip(name);

        if (result == null)
        {
            result = _bgmLibrary.GetClip(name);

            if (result == null)
            {
                Debug.LogWarning($"{name} does not exist in either sfx nor bgm libraries!");
                return null;
            }

            return _bgmLibrary.GetClip(name);
        }

        return _sfxLibrary.GetClip(name);
    }

    public void PlaySfx(string name)
    {
        _soundClipAudioSource.PlayOneShot(GetAudioClip(name));
    }

    public void PlayBgm(string name)
    {
        if (_source1Active)
        {
            StartCoroutine(StartFade(_bgmAudioSource2, _bgmAudioSource1, FADE_DURATION));
            _bgmAudioSource2.clip = GetAudioClip(name);
            _bgmAudioSource2.Play();
        }
        else
        {
            StartCoroutine(StartFade(_bgmAudioSource1, _bgmAudioSource2, FADE_DURATION));
            _bgmAudioSource1.clip = GetAudioClip(name);
            _bgmAudioSource1.Play();
        }

        _source1Active = !_source1Active;
    }

    public IEnumerator StartFade(AudioSource newSource, AudioSource oldSource, float duration)
    {
        var currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            oldSource.volume = Mathf.Lerp(MAX_VOLUME, 0, currentTime / duration);
            newSource.volume = Mathf.Lerp(0, MAX_VOLUME, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}
