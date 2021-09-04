using UnityEngine;
using TMPro;

public class CardComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public string cardValue;

    public void SetColor(ColorMatchColor color, bool isTop)
    {
        _text.text = color.name;
        if (!isTop) _text.color = color.color;
        cardValue = color.name;
    }

    public void SetText(string color)
    {
        _text.text = color;
    }
}
