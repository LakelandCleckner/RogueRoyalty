using UnityEngine;

public abstract class Coin : MonoBehaviour
{
    public abstract int PointValue { get; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        GameManager.Instance.AddScore(PointValue);
        Destroy(gameObject); // Remove the coin from the scene
    }
}
