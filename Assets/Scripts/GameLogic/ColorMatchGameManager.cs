using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatchGameManager : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    private int _totalPlayers = 0;
    private int _countdownsFinished = 0;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private List<ColorMatchPlayerManager> _playerManagers;

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

    public void Instantiate(bool isSolo)
    {
        _playerManagers[1].gameObject.SetActive(!isSolo);
        _totalPlayers = isSolo ? 1 : 2;

        _countdownsFinished = 0;

        _gameTimer.ResetTimer();
        _gameTimer.Instantiate();
        _gameTimer.onTimerFinish += FinishGame;

        StartCoroutine(DelayGameStart(2, isSolo));
    }

    private IEnumerator DelayGameStart(int seconds, bool isSolo)
    {
        yield return new WaitForSeconds(seconds);

        BeginGame(isSolo);
    }

    private void BeginGame(bool isSolo)
    {
        for (var i = 0; i < _totalPlayers; i++)
        {
            _playerManagers[i].SetUpGame(i != 0, OnCountdownTimerFinish); // Player 1 is always the only human for now.
        }
    }

    public void OnCountdownTimerFinish()
    {
        _countdownsFinished++;

        if (_countdownsFinished == _totalPlayers)
        {
            _gameTimer?.StartTimer();
        }
    }

    public void FinishGame()
    {
        foreach (var manager in _playerManagers)
        {
            manager?.EndGame();
        }

        CalculateWinner();
    }

    #region Scoring
    private void CalculateWinner()
    {
        var playerScore = _playerManagers[0]?.Score;
        var botScore = _playerManagers[1]?.Score;
        MatchResultType resultType;

        if (_totalPlayers == 1)
        {
            resultType = playerScore > 0 ? MatchResultType.Win : MatchResultType.Loss;
        }
        else
        {
            if (playerScore > botScore)
            {
                if (playerScore - botScore > 200)
                {
                    resultType = MatchResultType.DecisiveWin;
                }
                else if (playerScore - botScore <= 100)
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
        }

        StartCoroutine(AnnounceResult(resultType));
        StartCoroutine(GoHome());
    }

    private IEnumerator AnnounceResult(MatchResultType resultType)
    {
        yield return new WaitForSeconds(2f);
        switch (resultType)
        {
            case MatchResultType.DecisiveWin:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultDecisiveWin");
                break;
            case MatchResultType.Win:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultWin");
                break;
            case MatchResultType.CloseWin:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultCloseWin");
                break;
            case MatchResultType.Tie:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultDraw");
                break;
            case MatchResultType.DecisiveLoss:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultDecisiveLoss");
                break;
            case MatchResultType.Loss:
            case MatchResultType.CloseLoss:
                ColorMatchMainManager.Instance.SoundManager.PlaySfx("resultLoss");
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
