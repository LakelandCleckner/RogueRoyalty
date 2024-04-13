using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject bronzeCoinPrefab;
    public GameObject silverCoinPrefab;
    public GameObject goldCoinPrefab;

    // Audio clips for each type of coin
    public AudioClip bronzeCoinSound;
    public AudioClip silverCoinSound;
    public AudioClip goldCoinSound;

    private AudioSource audioSource;
    private int totalScore;
    private UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CoinFactory.bronzeCoinPrefab = bronzeCoinPrefab;
            CoinFactory.silverCoinPrefab = silverCoinPrefab;
            CoinFactory.goldCoinPrefab = goldCoinPrefab;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
        uiManager.UpdateScore(totalScore);
        PlayCoinSound(scoreToAdd);
    }

    private void PlayCoinSound(int score)
    {
        if (score == 10) // Assuming bronze coins are worth 10 points
        {
            audioSource.clip = bronzeCoinSound;
            audioSource.Play();
        }
        else if (score == 20) // Assuming silver coins are worth 20 points
        {
            audioSource.clip = silverCoinSound;
            audioSource.Play();
        }
        else if (score == 50) // Assuming gold coins are worth 50 points
        {
            audioSource.clip = goldCoinSound;
            audioSource.Play();
        }
    }
}
