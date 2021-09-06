using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private const int DEFAULT_COUNTDOWN_TIME = 3;

    public Action onCountdownFinish;

    [SerializeField] private TextMeshProUGUI _number;

    private Animator _animator;
    private int _countdownTimer;
    private bool _isBot = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Initialize(bool isBot)
    {
        onCountdownFinish = null;
        _isBot = isBot;
    }

    public void PlayCountdown(int countdownTime = DEFAULT_COUNTDOWN_TIME)
    {
        if (!_isBot)
        {
            ColorMatchMainManager.Instance.SoundManager.PlaySfx("countdownStart");
        }
        

        StartCoroutine(PlayCountdownAfterDelay(countdownTime, 2));
    }

    private IEnumerator PlayCountdownAfterDelay(int countdownTime, int delay = 0)
    {
        yield return new WaitForSeconds(delay);

        _countdownTimer = countdownTime;
        _number.text = countdownTime.ToString();
        _animator.Play("Countdown");

        if (!_isBot)
        {
            switch (countdownTime)
            {
                case 3:
                    ColorMatchMainManager.Instance.SoundManager.PlaySfx("countdown3");
                    break;
                case 2:
                    ColorMatchMainManager.Instance.SoundManager.PlaySfx("countdown2");
                    break;
                case 1:
                    ColorMatchMainManager.Instance.SoundManager.PlaySfx("countdown1");
                    break;
                default:
                    break;
            }
        }
    }

    public void DecrementCountdown()
    {
        _countdownTimer--;
        if (_countdownTimer > 0)
        {
            StartCoroutine(PlayCountdownAfterDelay(_countdownTimer));
        }
        else
        {
            _animator.Play("Hide");
            onCountdownFinish?.Invoke();
            if (!_isBot)
            {
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("countdownFinish");
            }
        }
    }
}
