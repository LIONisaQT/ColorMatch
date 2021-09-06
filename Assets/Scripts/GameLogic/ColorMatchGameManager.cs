using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorMatchGameManager : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    private int _totalPlayers = 0;
    private int _countdownsFinished = 0;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private List<ColorMatchPlayerManager> _playerManagers;

    // TODO: Make manager for this.
    [SerializeField] private GameObject _gameResultObject;
    [SerializeField] private TextMeshProUGUI _gameResultText;

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
        _gameResultObject.SetActive(false);
        _gameResultText.text = string.Empty;

        _playerManagers[1].gameObject.SetActive(!isSolo);
        _totalPlayers = isSolo ? 1 : 2;

        _countdownsFinished = 0;

        _gameTimer.ResetTimer();
        _gameTimer.Instantiate();
        _gameTimer.onTimerFinish += FinishGame;

        ColorMatchMainManager.Instance.SoundManager.PlayBgm(isSolo ? "soloTrack" : "versusTrack");

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
        CalculateWinner();

        foreach (var manager in _playerManagers)
        {
            manager?.EndGame();
        }
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
        }

        StartCoroutine(AnnounceResult(resultType));
    }

    private void DisplayResult(MatchResultType resultType)
    {
        _gameResultObject.SetActive(true);

        switch (resultType)
        {
            case MatchResultType.DecisiveWin:
                _gameResultText.text = "DECISIVE WIN";
                break;
            case MatchResultType.Win:
                _gameResultText.text = "YOU WIN";
                break;
            case MatchResultType.CloseWin:
                _gameResultText.text = "CLOSE WIN";
                break;
            case MatchResultType.Tie:
                _gameResultText.text = "DOUBLE KO";
                break;
            case MatchResultType.DecisiveLoss:
                _gameResultText.text = "U GOT REKT LOL";
                break;
            case MatchResultType.Loss:
                _gameResultText.text = "YOU LOSE";
                break;
            case MatchResultType.CloseLoss:
                _gameResultText.text = "CLOSE LOSS";
                break;
            default:
                break;
        }
    }

    private IEnumerator AnnounceResult(MatchResultType resultType)
    {
        yield return new WaitForSeconds(2f);

        DisplayResult(resultType);

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

        switch (resultType)
        {
            case MatchResultType.DecisiveWin:
            case MatchResultType.Win:
            case MatchResultType.CloseWin:
                ColorMatchMainManager.Instance.SoundManager.PlayBgm("victoryTrack");
                break;
            case MatchResultType.DecisiveLoss:
            case MatchResultType.Loss:
            case MatchResultType.CloseLoss:
                ColorMatchMainManager.Instance.SoundManager.PlayBgm("defeatTrack");
                break;
            case MatchResultType.Tie:
                ColorMatchMainManager.Instance.SoundManager.PlayBgm("drawTrack");
                break;
            default:
                break;
        }
    }
    #endregion

    public void GoHome()
    {
        foreach (var manager in _playerManagers)
        {
            manager.CleanUp();
        }

        ColorMatchMainManager.Instance.GoToHome();
    }
}
