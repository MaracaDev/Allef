using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingZone : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Inventory.Instance.Sell();
        }
    }
}
