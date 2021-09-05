using System;
using UnityEngine;

public class ColorMatchResponseManager : MonoBehaviour
{
    public Action<YesNoResponse> onInput;

    private bool _canReceiveInput = false;
    private float _answerDuration = 0;

    public bool CanReceiveInput
    {
        get => _canReceiveInput;
        set => _canReceiveInput = value;
    }

    public void Initialize()
    {
        _canReceiveInput = false;
        _answerDuration = 0;
    }

    void Update()
    {
        if (!_canReceiveInput) return;

        _answerDuration += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            HandleInput(false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HandleInput(true);
        }
    }

    public void HandleInput(bool saidYes)
    {
        var response = new YesNoResponse
        {
            saidYes = saidYes,
            duration = _answerDuration,
        };

        onInput?.Invoke(response);
    }

    public void HandleGameEnd()
    {
        _canReceiveInput = false;
    }

    [Serializable]
    public struct YesNoResponse
    {
        public bool saidYes;
        public float duration;
    }
}
