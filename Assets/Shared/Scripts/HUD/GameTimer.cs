using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    public Action onTimerFinish;

    [SerializeField] public float _timeValue = 10f;
    [SerializeField] public TextMeshProUGUI _timeText;
    [SerializeField] private AudioClip _timeUpClip;

    private bool _timerStarted = false;
    private bool _invokedTimerFinishAction = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        DisplayTime(_timeValue);
    }

    private void Update()
    {
        if (!_timerStarted) return;

        if (_timeValue > 0)
        {
            _timeValue -= Time.deltaTime;
        }
        else
        {
            _timeValue = 0;
            _timerStarted = false;

            if (!_invokedTimerFinishAction)
            {
                _invokedTimerFinishAction = true;
                onTimerFinish?.Invoke();
                _audioSource.PlayOneShot(_timeUpClip);
            }
        }

        DisplayTime(_timeValue);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        var minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        _timerStarted = true;
    }
}
