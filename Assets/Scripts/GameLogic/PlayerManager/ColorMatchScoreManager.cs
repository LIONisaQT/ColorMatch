using UnityEngine;

public class ColorMatchScoreManager : MonoBehaviour
{
    [SerializeField, Min(0)] private int _pointsPerCorrect = 50;
    [SerializeField, Min(0)] private int _currentScore = 0;
    [SerializeField, Min(1)] private int _currentMultiplier = 1;
    [SerializeField, Min(1)] private int _currentStreak = 0;
    [SerializeField, Min(1)] private int _multiplierIncreaseRequirement = 4;

    public int Score => _currentScore;

    public void Reset()
    {
        _currentScore = 0;
        _currentMultiplier = 1;
        _currentStreak = 0;
    }

    public void UpdateScore(bool isCorrect)
    {
        if (isCorrect)
        {
            _currentScore += _pointsPerCorrect * _currentMultiplier;
            _currentStreak++;
            if (_currentStreak >= _multiplierIncreaseRequirement)
            {
                _currentMultiplier++;
                _currentStreak = 0;
            }
        }
        else
        {
            if (_currentStreak == 0)
            {
                _currentMultiplier = Mathf.Max(_currentMultiplier - 1, 1);
            }
            _currentStreak = 0;
        }
    }
}
