using UnityEngine;
using TMPro;

public class ColorMatchComboManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _comboCountText;
    [SerializeField] private TextMeshProUGUI _multiplierText;
    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;
    [SerializeField] private string _positiveText;
    [SerializeField] private string _negativeText;

    [SerializeField] private Animator _bonusAnimator;
    [SerializeField] private Animator _comboAnimator;

    public void ShowCombo(int comboCount)
    {
        _comboCountText.text = comboCount.ToString();
        _comboAnimator.Play("ShowCombo", 0, 0);
    }

    public void ShowMultiplier(bool positive)
    {
        _multiplierText.color = positive ? _positiveColor : _negativeColor;
        _multiplierText.text = positive ? _positiveText : _negativeText;
        _bonusAnimator.Play("ShowBonus", 0, 0);
    }

    public void Reset()
    {
        _comboAnimator.Play("HideComboInstant");
        _bonusAnimator.Play("HideBonusInstant");
    }

    private void Update()
    {
        //if (_comboTimer <= 0)
        //{
        //    _animator.Play("HideCombo");
        //}
        //else
        //{
        //    _comboTimer -= Time.deltaTime;
        //}

        //if (_multiplierTimer <= 0)
        //{
        //    _animator.Play("HideBonus");
        //}
        //else
        //{
        //    _multiplierTimer -= Time.deltaTime;
        //}
    }
}
