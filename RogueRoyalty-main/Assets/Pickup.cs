using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    // Item type to identify what kind of item this is, avoiding health-related items
    public ItemType itemType;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Spawn the button at the first available inventory slot
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (inventory.items[i] == 0) // Check whether the slot is EMPTY
                {
                    // Update the inventory to reflect the picked-up item
                    inventory.items[i] = IdentifyItem(itemType); // Update based on item type
                    Instantiate(itemButton, inventory.slots[i].transform, false); // Spawn the button so that the player can interact with it
                    Destroy(gameObject); // Destroy the pickup object
                    break;
                }
            }
        }
    }

    // IdentifyItem method to process different item types, excluding health-related items
    private int IdentifyItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Coin:
                // Logic for coin pickup, returning an example identifier '1'
                return 1;
            case ItemType.Key:
                // Logic for key pickup, returning an example identifier '2'
                return 2;
            // Add more cases as needed for different item types
            default:
                return 0; // Unknown or generic item type
        }
    }
}

// Enum for different types of items that can be picked up, explicitly non-health related
public enum ItemType
{
    Coin,
    Key,
    // Add more non-health related item types as needed
}
