using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    [SerializeField] private string _correctText = ":)";
    [SerializeField] private Color _correctColor;
    [SerializeField] private string _incorrectText = ":(";
    [SerializeField] private Color _incorrectColor;

    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private Image _background;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowFeedback(bool isCorrect, bool isBot)
    {
        _feedbackText.text = isCorrect ? _correctText : _incorrectText;
        _background.color = isCorrect ? _correctColor : _incorrectColor;
        _animator.Play("ShowFeedback", layer: -1, normalizedTime: 0);

        if (!isBot)
            ColorMatchMainManager.Instance.SoundManager.PlaySfx(isCorrect ? "feedbackCorrect" : "feedbackIncorrect");
    }
}
