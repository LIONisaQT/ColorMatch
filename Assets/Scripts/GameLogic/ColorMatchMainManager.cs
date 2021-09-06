using UnityEngine;

public class ColorMatchMainManager : MonoBehaviour
{
    [SerializeField] private ColorMatchGameManager _gameManager;
    [SerializeField] private ColorMatchHomeManager _homeManager;
    [SerializeField] private ColorMatchMatchMakingManager _matchMakingManager;

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
        _matchMakingManager.gameObject.SetActive(false);
        _homeManager.gameObject.SetActive(false);

        _gameManager.gameObject.SetActive(true);
        _gameManager.Instantiate(isSolo);
    }

    public void GoToHome()
    {
        _gameManager.gameObject.SetActive(false);
        _matchMakingManager.gameObject.SetActive(false);

        _homeManager.gameObject.SetActive(true);
        _homeManager.Instantiate();
    }

    public void GoToMatchMaking()
    {
        _homeManager.gameObject.SetActive(false);
        _gameManager.gameObject.SetActive(false);

        _matchMakingManager.gameObject.SetActive(true);
        _matchMakingManager.Instantiate();
    }
}
