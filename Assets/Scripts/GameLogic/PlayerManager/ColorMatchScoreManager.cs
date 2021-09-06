using TMPro;
using UnityEngine;

public class ColorMatchScoreManager : MonoBehaviour
{
    [SerializeField, Min(0)] private int _pointsPerCorrect = 50;
    [SerializeField, Min(0)] private int _currentScore = 0;
    [SerializeField, Min(1)] private int _currentMultiplier = 1;
    [SerializeField, Min(1)] private int _currentStreak = 0;
    [SerializeField, Min(1)] private int _multiplierIncreaseRequirement = 4;

    [SerializeField] private TextMeshProUGUI _scoreText;

    public int Score => _currentScore;

    private bool _isBot;

    public void Reset()
    {
        _currentScore = 0;
        _currentMultiplier = 1;
        _currentStreak = 0;

        DisplayScore();
    }

    public void Initialize(bool isBot)
    {
        _isBot = isBot;
        Reset();
    }

    public void UpdateScore(bool isCorrect)
    {
        if (isCorrect)
        {
            _currentScore += _pointsPerCorrect * _currentMultiplier;
            _currentStreak++;
            if (_currentStreak >= _multiplierIncreaseRequirement)
            {
                if (!_isBot)
                    ColorMatchMainManager.Instance.SoundManager.PlaySfx("multiplierUp");
                _currentMultiplier++;
                _currentStreak = 0;
            }
        }
        else
        {
            if (_currentStreak == 0)
            {
                if (_currentMultiplier > 1)
                {
                    if (!_isBot)
                        ColorMatchMainManager.Instance.SoundManager.PlaySfx("multiplierDown");

                    _currentMultiplier = Mathf.Max(_currentMultiplier - 1, 1);
                    _currentStreak = _multiplierIncreaseRequirement - 1;
                }
            }
            else
            {
                _currentStreak--;
            }
        }

        DisplayScore();
    }

    private void DisplayScore()
    {
        _scoreText.text = $"Score: {_currentScore}\nMultiplier: x{_currentMultiplier}\nStreak: {_currentStreak}/{_multiplierIncreaseRequirement}";
    }
}
