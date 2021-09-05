using System;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private const int DEFAULT_COUNTDOWN_TIME = 3;

    public Action onCountdownFinish;

    [SerializeField] private TextMeshProUGUI _number;

    [SerializeField] private AudioClip _countdown1;
    [SerializeField] private AudioClip _countdown2;
    [SerializeField] private AudioClip _countdown3;
    [SerializeField] private AudioClip _countdownFinish;

    private Animator _animator;
    private AudioSource _audioSource;
    private int _countdownTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        onCountdownFinish = null;
    }

    public void PlayCountdown(int countdownTime = DEFAULT_COUNTDOWN_TIME)
    {
        _countdownTimer = countdownTime;
        _number.text = countdownTime.ToString();
        _animator.Play("Countdown");

        switch (countdownTime)
        {
            case 3:
                _audioSource.PlayOneShot(_countdown3);
                break;
            case 2:
                _audioSource.PlayOneShot(_countdown2);
                break;
            case 1:
                _audioSource.PlayOneShot(_countdown1);
                break;
            default:
                break;
        }
    }

    public void DecrementCountdown()
    {
        _countdownTimer--;
        if (_countdownTimer > 0)
        {
            PlayCountdown(_countdownTimer);
        }
        else
        {
            _animator.Play("Hide");
            onCountdownFinish?.Invoke();
            _audioSource.PlayOneShot(_countdownFinish);
        }
    }
}
