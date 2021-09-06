using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    public Action onTimerFinish;

    [SerializeField] private float _timeValue = 10f;
    [SerializeField] public TextMeshProUGUI _timeText;

    private float _currentTimeValue;
    private bool _timerStarted = false;
    private bool _invokedTimerFinishAction = false;

    private void Awake()
    {
        _currentTimeValue = _timeValue;
    }

    public void Instantiate()
    {
        DisplayTime(_currentTimeValue);
    }

    private void Update()
    {
        if (!_timerStarted) return;

        if (_currentTimeValue > 0)
        {
            _currentTimeValue -= Time.deltaTime;
        }
        else
        {
            _currentTimeValue = 0;
            _timerStarted = false;

            if (!_invokedTimerFinishAction)
            {
                TimerFinished();
            }
        }

        DisplayTime(_currentTimeValue);
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
    
    private void TimerFinished()
    {
        _invokedTimerFinishAction = true;
        onTimerFinish?.Invoke();
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("timesUp");
    }

    public void ResetTimer()
    {
        _timerStarted = false;
        _invokedTimerFinishAction = false;
        _currentTimeValue = _timeValue;
        onTimerFinish = null;
    }
}
