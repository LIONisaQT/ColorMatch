using UnityEngine;

public class ColorMatchHomeManager : MonoBehaviour
{
    private void Start()
    {
        Instantiate();
    }

    public void Instantiate()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("homeEnter");
        ColorMatchMainManager.Instance.SoundManager.PlayBgm("lobbyTrack");
    }

    public void TutorialButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("tutorialClick");
        ColorMatchMainManager.Instance.GoToGame(true);
    }

    public void SoloButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("soloClick");
        ColorMatchMainManager.Instance.GoToGame(true);
    }

    public void VersusButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("versusClick");
        ColorMatchMainManager.Instance.GoToMatchMaking();
    }
}
