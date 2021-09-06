using UnityEngine;

public class ColorMatchMainManager : MonoBehaviour
{
    [SerializeField] private ColorMatchGameManager _gameManager;
    [SerializeField] private ColorMatchHomeManager _homeManager;
    [SerializeField] private SoundManager _soundManager;

    private static ColorMatchMainManager _instance;

    public static ColorMatchMainManager Instance
    {
        get { return _instance; }
    }

    public SoundManager SoundManager => _soundManager;

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

    public void GoToGame(bool isSolo)
    {
        _gameManager.gameObject.SetActive(true);
        _gameManager.Instantiate(isSolo);
        _homeManager.gameObject.SetActive(false);
    }

    public void GoToHome()
    {
        _gameManager.gameObject.SetActive(false);
        _homeManager.Instantiate();
        _homeManager.gameObject.SetActive(true);
    }
}
