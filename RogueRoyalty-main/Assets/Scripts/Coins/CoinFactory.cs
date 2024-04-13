using UnityEngine;

public static class CoinFactory
{
    public static GameObject bronzeCoinPrefab { get; set; }
    public static GameObject silverCoinPrefab { get; set; }
    public static GameObject goldCoinPrefab { get; set; }

    public static Coin CreateCoin(CoinType type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case CoinType.Bronze:
                prefab = bronzeCoinPrefab;
                break;
            case CoinType.Silver:
                prefab = silverCoinPrefab;
                break;
            case CoinType.Gold:
                prefab = goldCoinPrefab;
                break;
        }
        if (prefab != null)
        {
            GameObject coinGO = GameObject.Instantiate(prefab);
            return coinGO.GetComponent<Coin>();
        }
        throw new System.ArgumentException("Invalid coin type");
    }
}
