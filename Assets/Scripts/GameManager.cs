using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnPos;
    [SerializeField] int coins;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        Instantiate(player, spawnPos.position, Quaternion.identity);
    }


    public void AddCoins(int coinsValue)
    {
        coins += coinsValue;
    }
}
