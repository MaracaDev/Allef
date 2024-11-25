using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items = new List<Item>();
    public int space = 5; // Capacidade do inventário

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garante que o inventário persista entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool Add(Item item)
    {
        // Verifica se o item é empilhável e já existe no inventário
        if (item.isStackable)
        {
            Item existingItem = items.Find(i => i.itemName == item.itemName);
            if (existingItem != null)
            {
                existingItem.quantity += item.quantity; // Soma a quantidade
                return true;
            }
        }

        // Verifica se há espaço no inventário
        if (items.Count >= space)
        {
            Debug.Log("Inventário cheio!");
            return false;
        }

        // Adiciona o item ao inventário
        items.Add(item);
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void Sell()
    {
        // Cria uma lista temporária para armazenar os itens a serem removidos
        List<Item> itemsToRemove = new List<Item>();

        // Itera sobre os itens e adiciona-os à lista temporária
        foreach (Item item in items)
        {
            GameManager.instance.AddCoins(item.value * item.quantity); // Multiplica pelo valor da quantidade
            itemsToRemove.Add(item);
        }

        // Remove os itens após a iteração
        foreach (Item item in itemsToRemove)
        {
            Remove(item);
        }
    }
}
