using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatchGameManager : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    private int _countdownsFinished = 0;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private List<ColorMatchPlayerManager> _playerManagers;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _decisiveWinClip;
    [SerializeField] private List<AudioClip> _winClips;
    [SerializeField] private AudioClip _closeWinClip;
    [SerializeField] private AudioClip _tieClip;
    [SerializeField] private AudioClip _decisiveLossClip;
    [SerializeField] private List<AudioClip> _lossClips;

    private enum MatchResultType
    {
        Win,
        Loss,
        Tie,
        DecisiveWin,
        DecisiveLoss,
        CloseWin,
        CloseLoss,
    }

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Instantiate()
    {
        print("Arrive at game!");

        for (var i = 0; i < _playerManagers.Count; i++)
        {
            _playerManagers[i].SetUpGame(i != 0, OnCountdownTimerFinish); // Player 1 is always the only human for now.
        }

        _countdownsFinished = 0;

        _gameTimer.ResetTimer();
        _gameTimer.Instantiate();
        _gameTimer.onTimerFinish += FinishGame;
    }

    public void OnCountdownTimerFinish()
    {
        _countdownsFinished++;

        if (_countdownsFinished == _playerManagers.Count)
        {
            _gameTimer?.StartTimer();
        }
    }

    public void FinishGame()
    {
        foreach (var manager in _playerManagers)
        {
            manager.EndGame();
        }

        CalculateWinner();
    }

    #region Scoring
    private void CalculateWinner()
    {
        var playerScore = _playerManagers[0].Score;
        var botScore = _playerManagers[1].Score;
        MatchResultType resultType;

        if (playerScore > botScore)
        {
            if (playerScore - botScore > 200)
            {
                resultType = MatchResultType.DecisiveWin;
            }
            else if(playerScore - botScore <= 100)
            {
                resultType = MatchResultType.CloseWin;
            }
            else
            {
                resultType = MatchResultType.Win;
            }
        }
        else if (playerScore == botScore)
        {
            resultType = MatchResultType.Tie;
        }
        else
        {
            if (botScore - playerScore > 200)
            {
                resultType = MatchResultType.DecisiveLoss;
            }
            else if (botScore - playerScore <= 100)
            {
                resultType = MatchResultType.CloseLoss;
            }
            else
            {
                resultType = MatchResultType.Loss;
            }
        }

        print($"{playerScore} / {botScore} :: {resultType}");

        StartCoroutine(AnnounceResult(resultType));
        StartCoroutine(GoHome());
    }

    private IEnumerator AnnounceResult(MatchResultType resultType)
    {
        yield return new WaitForSeconds(2f);
        switch (resultType)
        {
            case MatchResultType.DecisiveWin:
                _audioSource.PlayOneShot(_decisiveWinClip);
                break;
            case MatchResultType.Win:
                _audioSource.PlayOneShot(_winClips[rng.Next(_winClips.Count)]);
                break;
            case MatchResultType.CloseWin:
                _audioSource.PlayOneShot(_closeWinClip);
                break;
            case MatchResultType.Tie:
                _audioSource.PlayOneShot(_tieClip);
                break;
            case MatchResultType.DecisiveLoss:
                _audioSource.PlayOneShot(_decisiveLossClip);
                break;
            case MatchResultType.Loss:
            case MatchResultType.CloseLoss:
                _audioSource.PlayOneShot(_lossClips[rng.Next(_lossClips.Count)]);
                break;
            default:
                break;
        }
    }
    #endregion

    private IEnumerator GoHome()
    {
        yield return new WaitForSeconds(4);
        ColorMatchMainManager.Instance.GoToHome();
    }
}
