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
    [SerializeField] private ColorMatchResponseManager _responseManager;

    [SerializeField] private bool _canReceiveInput = false;

    private Queue<CardSet.ColorMatchAnswer> _choiceHistory = new Queue<CardSet.ColorMatchAnswer>();

    private bool _isComputerPlayer = false;

    public int Score => _scoreManager.Score;

    public void SetUpGame(bool isBot, Action onCountdownFinish)
    {
        _choiceHistory.Clear();

        _cardSet.Initialize();
        _scoreManager.Initialize();
        _countdownTimer.Initialize(isBot);
        _responseManager.Initialize();

        _responseManager.onInput += _cardSet.HandleInput;
        _cardSet.onHandleInput += HandleResponse;

        if (isBot)
        {
            LoadComputerPlayer();
        }
        else
        {
            _canReceiveInput = true;
        }

        _countdownTimer.onCountdownFinish += _cardSet.OnCountdownTimerFinished;
        _countdownTimer.onCountdownFinish += OnCountdownTimerFinish;
        _countdownTimer.onCountdownFinish += onCountdownFinish;

        _countdownTimer.PlayCountdown();
    }

    public void OnCountdownTimerFinish()
    {
        if (!_isComputerPlayer)
        {
            _responseManager.CanReceiveInput = _canReceiveInput;
        }
        else
        {
            AutoPlay();
        }
    }

    public void HandleResponse(CardSet.ColorMatchAnswer response)
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
        _responseManager?.HandleGameEnd();

        if (!_isComputerPlayer)
        {
            RecordGameplayData();
        }

        _choiceHistory.Clear();
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
        _choiceHistory = JsonConvert.DeserializeObject<Queue<CardSet.ColorMatchAnswer>>(file);
    }

    private void AutoPlay()
    {
        if (_choiceHistory.Count > 0)
        {
            var choice = _choiceHistory.Dequeue();
            ReplayChoice(choice);
        }
    }

    private void ReplayChoice(CardSet.ColorMatchAnswer data)
    {
        _ = _cardSet.HandleInput(data, AutoPlay);
    }
    #endregion
}
