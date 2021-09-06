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
    }

    public void TutorialButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("tutorialClick");
    }

    public void SoloButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("soloClick");
        ColorMatchMainManager.Instance.GoToGame(true);
    }

    public void VersusButtonPressed()
    {
        ColorMatchMainManager.Instance.SoundManager.PlaySfx("versusClick");
        ColorMatchMainManager.Instance.GoToGame(false);
    }
}
