using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class ColorMatchPlayerManager : MonoBehaviour
{
    [SerializeField] private CardSet _cardSet;
    [SerializeField] private CountdownTimer _countdownTimer;
    [SerializeField] private Feedback _feedback;

    [SerializeField] private bool _canReceiveInput = false;

    private Queue<ChoiceData> _choiceHistory = new Queue<ChoiceData>();
    private float _timeBetweenChoice = 0;

    private bool _isComputerPlayer = false;

    private void Update()
    {
        if (_cardSet.canReceiveInput)
        {
            _timeBetweenChoice += Time.deltaTime;
        }
    }

    public void SetUpGame(bool isBot, ColorMatchGameManager gameManager)
    {
        if (isBot)
        {
            LoadComputerPlayer();
        }
        else
        {
            _canReceiveInput = true;
        }

        _cardSet.AnimateCardsIn();
        _cardSet.SetCards();
        _cardSet.onPlayerChoice += OnPlayerChoice;

        _countdownTimer.onCountdownFinish += CountdownTimerFinish;
        _countdownTimer.onCountdownFinish += gameManager.CountdownTimerFinish;
        _countdownTimer.PlayCountdown();
    }

    public void OnPlayerChoice(bool isCorrect)
    {
        if (!_isComputerPlayer)
        {
            _choiceHistory.Enqueue(new ChoiceData
            {
                choice = isCorrect,
                duration = _timeBetweenChoice,
            });

            _timeBetweenChoice = 0;
        }

        _feedback.ShowFeedback(isCorrect);
    }

    public void CountdownTimerFinish()
    {
        _cardSet.AnimateCardsFlip();

        if (!_isComputerPlayer)
        {
            _cardSet.canReceiveInput = _canReceiveInput;
        }
        else
        {
            AutoPlay();
        }
    }

    private void LoadComputerPlayer()
    {
        _isComputerPlayer = true;
        ReadGameplayData();
    }

    private void AutoPlay()
    {
        if (_choiceHistory.Count > 0)
        {
            var choice = _choiceHistory.Dequeue();
            _ = ReplayChoiceHistory(choice);
        }
    }
    private async Task ReplayChoiceHistory(ChoiceData data)
    {
        await Task.Delay((int)(data.duration * 1000));

        OnPlayerChoice(data.choice);
        _cardSet.AnimateCardsBlink();
        _cardSet.SetCards();

        AutoPlay();
    }

    public void EndGame()
    {
        _cardSet.canReceiveInput = false;
        if (!_isComputerPlayer)
        {
            RecordGameplayData();
        }
    }

    private void RecordGameplayData()
    {
        var json = JsonConvert.SerializeObject(_choiceHistory, Formatting.Indented);
        File.WriteAllText(@$"{Application.dataPath}/Metadata/metadata.json", json);
    }

    private void ReadGameplayData()
    {
        _choiceHistory = JsonConvert.DeserializeObject<Queue<ChoiceData>>(
            File.ReadAllText(@$"{Application.dataPath}/Metadata/metadata.json")
        );
    }

    [Serializable]
    private struct ChoiceData
    {
        public bool choice;
        public float duration;
    }
}
