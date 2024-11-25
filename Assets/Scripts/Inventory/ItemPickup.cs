using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryUI.instance.UpdateUI();
            bool added = Inventory.Instance.Add(item);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
