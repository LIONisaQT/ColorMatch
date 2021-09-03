using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class ColorMatchGameManager : MonoBehaviour
{
    [SerializeField] private CardSet _cardSet;
    [SerializeField] private CountdownTimer _countdownTimer;
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private Feedback _feedback;

    [SerializeField] private bool _canReceiveInput = false;

    private List<ChoiceData> _choiceHistory;
    private float _timeBetweenChoice = 0;

    private bool _isComputerPlayer = false;

    private void Awake()
    {
        _choiceHistory = new List<ChoiceData>();
    }

    private void Start()
    {
        SetUpGame();
    }

    private void Update()
    {
        if (_cardSet.canReceiveInput)
        {
            _timeBetweenChoice += Time.deltaTime;
        }
    }

    public void SetUpGame()
    {
        _cardSet.AnimateCardsIn();
        _cardSet.SetCards();
        _cardSet.onPlayerChoice += OnPlayerChoice;

        _countdownTimer.onCountdownFinish += CountdownTimerFinish;
        _countdownTimer.PlayCountdown();

        if (_gameTimer != null && _canReceiveInput)
        {
            _gameTimer.onTimerFinish += EndGame;
        }
        else
        {
            LoadComputerPlayer();
        }
    }

    public void CountdownTimerFinish()
    {
        _gameTimer?.StartTimer();
        _cardSet.AnimateCardsFlip();

        if (!_isComputerPlayer)
        {
            _cardSet.canReceiveInput = _canReceiveInput;
        }
        else
        {
            if (_choiceHistory.Count > 0)
            {
                var choice = _choiceHistory[0];
                _choiceHistory.RemoveAt(0);
                _ = ReplayChoiceHistory(choice);

            }
        }
    }

    public void OnPlayerChoice(bool isCorrect)
    {
        if (!_isComputerPlayer)
        {
            _choiceHistory.Add(new ChoiceData
            {
                choice = isCorrect,
                duration = _timeBetweenChoice,
            });

            _timeBetweenChoice = 0;
        }
        
        _feedback.ShowFeedback(isCorrect);
    }

    private void EndGame()
    {
        _cardSet.canReceiveInput = false;

        //var json = JsonConvert.SerializeObject(_choiceHistory, Formatting.Indented);
        //File.WriteAllText(@$"{Application.dataPath}/Metadata/metadata.json", json);
    }

    private void LoadComputerPlayer()
    {
        _isComputerPlayer = true;
        _choiceHistory = JsonConvert.DeserializeObject<List<ChoiceData>>(File.ReadAllText(@$"{Application.dataPath}/Metadata/metadata.json"));
    }

    private async Task ReplayChoiceHistory(ChoiceData data)
    {
        await Task.Delay((int)(data.duration * 1000));

        OnPlayerChoice(data.choice);
        _cardSet.AnimateCardsBlink();
        _cardSet.SetCards();

        if (_choiceHistory.Count > 0)
        {
            var choice = _choiceHistory[0];
            _choiceHistory.RemoveAt(0);
            _ = ReplayChoiceHistory(choice);
        }
    }

    [Serializable]
    private struct ChoiceData
    {
        public bool choice;
        public float duration;
    }
}
