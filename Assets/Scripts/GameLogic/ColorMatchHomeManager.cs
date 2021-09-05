using UnityEngine;

public class ColorMatchHomeManager : MonoBehaviour
{
    private void Start()
    {
        Instantiate();
    }

    public void Instantiate()
    {
        print("Arrive at home!");
    }

    public void TutorialButtonPressed()
    {
        print("Tutorial button pressed!");
    }

    public void SoloButtonPressed()
    {
        ColorMatchMainManager.Instance.GoToGame();

    }

    public void VersusButtonPressed()
    {
        ColorMatchMainManager.Instance.GoToGame();
    }
}
