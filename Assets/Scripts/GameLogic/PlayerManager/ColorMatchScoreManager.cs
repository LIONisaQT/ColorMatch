using TMPro;
using UnityEngine;

public class ColorMatchScoreManager : MonoBehaviour
{
    private const int MAX_MULTIPLIER = 10;

    [SerializeField, Min(0)] private int _pointsPerCorrect = 50;
    [SerializeField, Min(0)] private int _currentScore = 0;
    [SerializeField, Min(1)] private int _currentMultiplier = 1;
    [SerializeField, Min(1)] private int _currentStreak = 0;
    [SerializeField, Min(1)] private int _multiplierIncreaseRequirement = 4;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private ColorMatchComboManager _comboManager;

    public int Score => _currentScore;

    private bool _isBot;
    private int _currentCombo = 0;

    public void Reset()
    {
        _currentScore = 0;
        _currentMultiplier = 1;
        _currentStreak = 0;
        _currentCombo = 0;

        DisplayScore();
        _comboManager.Reset();
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
            _currentCombo++;
            _comboManager.ShowCombo(_currentCombo);
            if (_currentStreak >= _multiplierIncreaseRequirement)
            {
                if (!_isBot)
                    ColorMatchMainManager.Instance.SoundManager.PlaySfx("multiplierUp");
                _currentMultiplier++;
                _currentMultiplier = Mathf.Min(MAX_MULTIPLIER, _currentMultiplier);
                _currentStreak = 0;
                _comboManager.ShowMultiplier(true);
            }
        }
        else
        {
            _currentCombo = 0;
            if (_currentStreak == 0)
            {
                if (_currentMultiplier > 1)
                {
                    if (!_isBot)
                        ColorMatchMainManager.Instance.SoundManager.PlaySfx("multiplierDown");

                    _currentMultiplier = Mathf.Max(_currentMultiplier - 1, 1);
                    _currentStreak = _multiplierIncreaseRequirement - 1;
                    _comboManager.ShowMultiplier(false);
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
