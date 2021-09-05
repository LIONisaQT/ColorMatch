using UnityEngine;

public class ColorMatchMainManager : MonoBehaviour
{
    [SerializeField] private ColorMatchGameManager _gameManager;
    [SerializeField] private ColorMatchHomeManager _homeManager;

    private static ColorMatchMainManager _instance;

    public static ColorMatchMainManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GoToGame()
    {
        _gameManager.gameObject.SetActive(true);
        _homeManager.gameObject.SetActive(false);
    }

    public void GoToHome()
    {
        _gameManager.gameObject.SetActive(false);
        _homeManager.gameObject.SetActive(true);
    }
}
