using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSet : MonoBehaviour
{
    public static System.Random rng = new System.Random();

    public Action<bool> onPlayerChoice;

    [SerializeField] private ColorMatchColors _cardData;
    [SerializeField] private CardComponent _topCard;
    [SerializeField] private CardComponent _bottomCard;

    [HideInInspector] public bool canReceiveInput = false;

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

    public void SetCards()
    {
        var values = Enum.GetValues(typeof(MatchType));
        var matchType = (MatchType)values.GetValue(rng.Next(values.Length));

        var topColor = _cardData.GetRandomColor();
        ColorMatchColor bottomColor = topColor;
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

        _topCard.SetColor(topColor);
        _bottomCard.SetColor(bottomColor);
        _bottomCard.SetText(bottomWordValue);
    }

    public void Update()
    {
        if (!canReceiveInput) return;

        // Respond with "no".
        if (Input.GetKeyDown(KeyCode.A))
        {
            HandleInput(false);
        }

        // Respond with "yes".
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HandleInput(true);
        }
    }

    public void HandleInput(bool saidMatch)
    {
        var isCorrect = saidMatch == (_topCard.cardValue == _bottomCard.cardValue);
        onPlayerChoice?.Invoke(isCorrect);
        AnimateCardsBlink();
        SetCards();
    }
}
