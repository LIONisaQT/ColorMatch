using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ColorMatchPlayerManager : MonoBehaviour
{
    [SerializeField] private CardSet _cardSet;
    [SerializeField] private CountdownTimer _countdownTimer;
    [SerializeField] private Feedback _feedback;
    [SerializeField] private ColorMatchScoreManager _scoreManager;

    [SerializeField] private bool _canReceiveInput = false;

    private Queue<CardSet.Response> _choiceHistory = new Queue<CardSet.Response>();

    private bool _isComputerPlayer = false;

    public int Score => _scoreManager.Score;

    public void SetUpGame(bool isBot, Action onCountdownFinish)
    {
        _cardSet.AnimateCardsIn();
        _cardSet.SetCards();
        _cardSet.onPlayerChoice += OnPlayerChoice;

        _choiceHistory.Clear();

        // Player response setup.
        if (isBot)
        {
            LoadComputerPlayer();
        }
        else
        {
            _canReceiveInput = true;
        }

        _scoreManager.Reset();

        _countdownTimer.onCountdownFinish += OnCountdownTimerFinish;
        _countdownTimer.onCountdownFinish += onCountdownFinish;
        _countdownTimer.PlayCountdown();
    }

    public void OnCountdownTimerFinish()
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

    public void OnPlayerChoice(CardSet.Response response)
    {
        if (!_isComputerPlayer)
        {
            _choiceHistory.Enqueue(response);
        }

        _feedback.ShowFeedback(response.isCorrect);
        _scoreManager.UpdateScore(response.isCorrect);
    }

    public void EndGame()
    {
        _cardSet.canReceiveInput = false;

        if (!_isComputerPlayer)
        {
            RecordGameplayData();
        }

        _choiceHistory.Clear();
        _cardSet.onPlayerChoice = null;
        _countdownTimer.onCountdownFinish = null;
    }

    private void RecordGameplayData()
    {
        //var json = JsonConvert.SerializeObject(_choiceHistory, Formatting.Indented);
        //File.WriteAllText(@$"{Application.dataPath}/Metadata/metadata.json", json);
    }

    #region Bot player
    private void LoadComputerPlayer()
    {
        _isComputerPlayer = true;
        ReadGameplayData();
    }

    private void ReadGameplayData()
    {
        if (!File.Exists(@$"{Application.dataPath}/Metadata/metadata.json")) return;

        var file = File.ReadAllText(@$"{Application.dataPath}/Metadata/metadata.json");
        _choiceHistory = JsonConvert.DeserializeObject<Queue<CardSet.Response>>(file);
    }

    private void AutoPlay()
    {
        if (_choiceHistory.Count > 0)
        {
            var choice = _choiceHistory.Dequeue();
            ReplayChoice(choice);
        }
    }

    private void ReplayChoice(CardSet.Response data)
    {
        _ = _cardSet.HandleInput(data, AutoPlay);
    }
    #endregion
}
