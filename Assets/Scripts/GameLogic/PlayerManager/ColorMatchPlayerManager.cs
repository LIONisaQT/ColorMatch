using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class ColorMatchPlayerManager : MonoBehaviour
{
    [SerializeField] private CardSet _cardSet;
    [SerializeField] private CountdownTimer _countdownTimer;
    [SerializeField] private Feedback _feedback;
    [SerializeField] private ColorMatchScoreManager _scoreManager;
    [SerializeField] private ColorMatchResponseManager _responseManager;
    [SerializeField] private FirebaseHandler _firebaseHandler;

    [SerializeField] private bool _canReceiveInput = false;

    private Queue<CardSet.ColorMatchAnswer> _choiceHistory = new Queue<CardSet.ColorMatchAnswer>();

    private bool _isComputerPlayer = false;
    private PlayerData _botData;

    public int Score => _scoreManager.Score;

    public void SetUpGame(bool isBot, Action onCountdownFinish)
    {
        _choiceHistory.Clear();

        _cardSet.Initialize();
        _scoreManager.Initialize(isBot);
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

        _feedback.ShowFeedback(response.isCorrect, _isComputerPlayer);
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

    public void CleanUp()
    {
        _scoreManager?.Reset();
    }

    private void RecordGameplayData()
    {
        float correctTotal = _choiceHistory.Where(x => x.isCorrect).ToList().Count;
        var accuracy = _choiceHistory.Count > 0 ? correctTotal / _choiceHistory.Count : 0;
        var averageDuration = _choiceHistory.DefaultIfEmpty().Average(x => x.duration);

        var playerData = new PlayerData()
        {
            name = SystemInfo.deviceUniqueIdentifier,
            accuracy = accuracy,
            decisionDelay = averageDuration,
        };

        var json = JsonConvert.SerializeObject(playerData, Formatting.Indented);

#if UNITY_EDITOR
        //File.WriteAllText(@$"{Application.dataPath}/Metadata/metadata.json", json);
#else
        _firebaseHandler.SendPlayerData(json);
#endif
    }

    #region Bot player
    private void LoadComputerPlayer()
    {
        _isComputerPlayer = true;
        ReadGameplayData();
    }

    private void ReadGameplayData()
    {
#if UNITY_EDITOR
        if (!File.Exists(@$"{Application.dataPath}/Metadata/metadata.json")) return;
        var file = File.ReadAllText(@$"{Application.dataPath}/Metadata/metadata.json");
#else
        var file = _firebaseHandler.GetPlayerData();
        print(file);
#endif

        _botData = JsonConvert.DeserializeObject<PlayerData>(file);
    }

    private void AutoPlay()
    {
        var randomBool = ColorMatchGameManager.rng.NextDouble() < _botData.accuracy;
        ReplayChoice(new CardSet.ColorMatchAnswer
        {
            isCorrect = randomBool,
            duration = _botData.decisionDelay,
        });
    }

    private void ReplayChoice(CardSet.ColorMatchAnswer data)
    {
        _ = _cardSet.HandleInput(data, AutoPlay);
    }
#endregion

    [Serializable]
    public struct PlayerData
    {
        public string name;
        public float accuracy;
        public float decisionDelay;
    }
}
