using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject bronzeCoinPrefab;
    public GameObject silverCoinPrefab;
    public GameObject goldCoinPrefab;

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
    }
}
