using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Color Match/Color Data", order = 1)]
public class ColorMatchColors : ScriptableObject
{
    private static System.Random rng = new System.Random();

    [SerializeField] private List<ColorMatchColor> _colors;

    public ColorMatchColor GetRandomColor()
    {
        return _colors[rng.Next(_colors.Count)];
    }

    public ColorMatchColor GetRandomColorExcept(ColorMatchColor color)
    {
        var selection = _colors[rng.Next(_colors.Count)];
        while (selection.Equals(color))
        {
            selection = _colors[rng.Next(_colors.Count)];
        }
        return selection;
    }
}

[Serializable]
public struct ColorMatchColor
{
    public string name;
    public Color color;
}
