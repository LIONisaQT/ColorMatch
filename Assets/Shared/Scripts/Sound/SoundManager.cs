using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundLibrary _sfxLibrary;
    [SerializeField] private SoundLibrary _bgmLibrary;

    [SerializeField] private AudioSource _soundClipAudioSource;
    [SerializeField] private AudioSource _bgmAudioSource;

    private void Awake()
    {
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

    public void PlayBGM(string name)
    {
        _bgmAudioSource.clip = GetAudioClip(name);
        _bgmAudioSource.Play();
    }
}
