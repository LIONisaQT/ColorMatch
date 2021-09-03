using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Feedback : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    [SerializeField] private string _correctText = ":)";
    [SerializeField] private Color _correctColor;
    [SerializeField] private string _incorrectText = ":(";
    [SerializeField] private Color _incorrectColor;

    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private Image _background;

    [SerializeField] private List<AudioClip> _correctClips;
    [SerializeField] private List<AudioClip> _incorrectClips;

    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShowFeedback(bool isCorrect)
    {
        _feedbackText.text = isCorrect ? _correctText : _incorrectText;
        _background.color = isCorrect ? _correctColor : _incorrectColor;
        _animator.Play("ShowFeedback", layer: -1, normalizedTime: 0);

        _audioSource.PlayOneShot(
            isCorrect ? _correctClips[rng.Next(_correctClips.Count)] : _incorrectClips[rng.Next(_incorrectClips.Count)]
        );
    }
}
