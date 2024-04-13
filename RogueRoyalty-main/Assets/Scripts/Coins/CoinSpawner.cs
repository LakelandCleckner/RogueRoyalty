using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints; // Assign this in the Inspector

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            CoinType coinType = DetermineCoinType(); // Now returns CoinType

            Coin spawnedCoin = CoinFactory.CreateCoin(coinType);
            spawnedCoin.transform.position = spawnPoint.transform.position;
        }
    }

    // Updated to return CoinType
    CoinType DetermineCoinType()
    {
        int choice = Random.Range(0, 3); // Returns 0, 1, or 2
        switch (choice)
        {
            case 0:
                return CoinType.Bronze;
            case 1:
                return CoinType.Silver;
            case 2:
                return CoinType.Gold;
            default:
                return CoinType.Bronze; // Default case, should not hit this
        }
    }
}
