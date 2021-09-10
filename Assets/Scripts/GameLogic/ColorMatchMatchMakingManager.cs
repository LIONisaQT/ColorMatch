using System.Collections;
using UnityEngine;
using TMPro;

public class ColorMatchMatchMakingManager : MonoBehaviour
{
    private const int NOTIFICATION_DURATION = 10;

    [SerializeField] private TextMeshProUGUI _elapsedTime;
    [SerializeField] private TextMeshProUGUI _estimatedTime;
    [SerializeField] private GameObject _matchFoundObject;

    private bool _foundMatch = false;
    private float _timeElapsed = 0;

    public void Instantiate()
    {
        _matchFoundObject.SetActive(false);
        _timeElapsed = 0;
        _foundMatch = false;

        var estimate = ColorMatchGameManager.rng.Next(5) + 3;
        var minutes = Mathf.FloorToInt(estimate / 60f);
        var seconds = Mathf.FloorToInt(estimate % 60f);

        _estimatedTime.text = string.Format("Estimated wait time: {0:00}:{1:00}", minutes, seconds);

        StartCoroutine(FindMatchAfterDelay(estimate));
    }

    private void Update()
    {
        if (_foundMatch) return;

        _timeElapsed += Time.deltaTime;

        DisplayTime(_timeElapsed);
    }

    private void DisplayTime(float timeToDisplay)
    {
        var minutes = Mathf.FloorToInt(timeToDisplay / 60);
        var seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _elapsedTime.text = string.Format("Elapsed time: {0:00}:{1:00}", minutes, seconds);
    }

    public IEnumerator FindMatchAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        FoundMatch();
    }

    private void FoundMatch()
    {
        _foundMatch = true;
        _matchFoundObject.SetActive(true);
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("matchFound");
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("matchFoundSfx");
        ColorMatchMainManager.Instance.SoundManager.PlayBgm("opponentFound");

        StartCoroutine(GoToMatch());
    }

    private IEnumerator GoToMatch()
    {
        yield return new WaitForSeconds(NOTIFICATION_DURATION);

        ColorMatchMainManager.Instance.GoToGame(false);
    }

    public void CancelMatchMakingPressed()
    {
        StopAllCoroutines();

        ColorMatchMainManager.Instance.GoToHome();
    }
}
