using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items = new List<Item>();
    public int space = 5; // Capacidade do invent�rio

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garante que o invent�rio persista entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool Add(Item item)
    {
        // Verifica se o item � empilh�vel e j� existe no invent�rio
        if (item.isStackable)
        {
            Item existingItem = items.Find(i => i.itemName == item.itemName);
            if (existingItem != null)
            {
                existingItem.quantity += item.quantity; // Soma a quantidade
                return true;
            }
        }

        // Verifica se h� espa�o no invent�rio
        if (items.Count >= space)
        {
            Debug.Log("Invent�rio cheio!");
            return false;
        }

        // Adiciona o item ao invent�rio
        items.Add(item);
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void Sell()
    {
        // Cria uma lista tempor�ria para armazenar os itens a serem removidos
        List<Item> itemsToRemove = new List<Item>();

        // Itera sobre os itens e adiciona-os � lista tempor�ria
        foreach (Item item in items)
        {
            GameManager.instance.AddCoins(item.value * item.quantity); // Multiplica pelo valor da quantidade
            itemsToRemove.Add(item);
        }

        // Remove os itens ap�s a itera��o
        foreach (Item item in itemsToRemove)
        {
            Remove(item);
        }
    }
}
