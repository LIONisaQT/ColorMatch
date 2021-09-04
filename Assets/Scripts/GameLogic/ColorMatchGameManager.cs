using System.Collections.Generic;
using UnityEngine;

public class ColorMatchGameManager : MonoBehaviour
{
    public System.Action onCountdownFinish;
    private int _countdownsFinished = 0;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private List<ColorMatchPlayerManager> _playerManagers;

    private void Start()
    {
        for (var i = 0; i < _playerManagers.Count; i++)
        {
            _playerManagers[i].SetUpGame(i != 0, this); // Player 1 is always the only human for now.

            if (_gameTimer != null)
            {
                _gameTimer.onTimerFinish += _playerManagers[i].EndGame;
            }
        }
    }

    public void CountdownTimerFinish()
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
    }
}
