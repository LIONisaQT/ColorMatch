using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CardSet : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    public Action<ColorMatchAnswer> onHandleInput;

    [SerializeField] private ColorMatchColors _cardData;
    [SerializeField] private CardComponent _topCard;
    [SerializeField] private CardComponent _bottomCard;

    private Animator _animator;

    private enum MatchType
    {
        SameColorSameWord,
        SameColorDiffWord,
        DiffColorSameWord,
        DiffColorDiffWord,
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Reset()
    {
        onHandleInput = null;
    }

    public void Initialize()
    {
        Reset();

        AnimateCardsIn();
        SetCards();
    }

    public void SetCards()
    {
        var values = Enum.GetValues(typeof(MatchType));
        var matchType = (MatchType)values.GetValue(rng.Next(values.Length));

        var topColor = _cardData.GetRandomColor();
        var bottomColor = topColor;
        string bottomWordValue = topColor.name;

        switch (matchType)
        {
            case MatchType.SameColorDiffWord:
                bottomWordValue = _cardData.GetRandomColorExcept(topColor).name;
                break;
            case MatchType.DiffColorSameWord:
                bottomColor = _cardData.GetRandomColorExcept(topColor);
                break;
            case MatchType.DiffColorDiffWord:
                bottomColor = _cardData.GetRandomColorExcept(topColor);
                bottomWordValue = _cardData.GetRandomColorExcept(topColor).name;
                break;
            default:
                // Same color, same word
                break;
        }

        _topCard.SetColor(topColor, true);
        _bottomCard.SetColor(bottomColor, false);
        _bottomCard.SetText(bottomWordValue);
    }

    public void OnCountdownTimerFinished()
    {
        AnimateCardsFlip();
    }

    private void TrialFinished()
    {
        AnimateCardsBlink();
        SetCards();
    }

    #region Input
    public void HandleInput(ColorMatchResponseManager.YesNoResponse response)
    {
        var isCorrect = response.saidYes == (_topCard.cardValue == _bottomCard.cardValue);

        var answer = new ColorMatchAnswer
        {
            isCorrect = isCorrect,
            duration = response.duration,
        };

        onHandleInput?.Invoke(answer);

        TrialFinished();
    }

    public IEnumerator HandleInput(ColorMatchAnswer response, Action onInputFinished)
    {
        yield return new WaitForSeconds(response.duration);

        onHandleInput?.Invoke(response);
        onInputFinished?.Invoke();

        TrialFinished();
    }
    #endregion

    #region Animation
    public void AnimateCardsIn()
    {
        _animator.Play("CardSlideIn");
    }

    public void AnimateCardsFlip()
    {
        _animator.Play("CardFlip");
    }

    public void AnimateCardsBlink()
    {
        _animator.Play("CardBlink", layer: -1, normalizedTime: 0);
    }
    #endregion

    [Serializable]
    public struct ColorMatchAnswer
    {
        public bool isCorrect;
        public float duration;
    }
}
