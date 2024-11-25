using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public Transform slotsParent;

    private List<GameObject> slots = new List<GameObject>();

    public static InventoryUI instance;

    private void Start()
    {
        instance = this;
        // Inicializar slots
        for (int i = 0; i < Inventory.Instance.space; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slots.Add(slot);
        }
        
    }

  

    public void UpdateUI()
    {
        
        
            // Limpar todos os slots
            foreach (GameObject slot in slots)
            {
                slot.transform.Find("ItemIcon").GetComponent<Image>().sprite = null;
                slot.transform.Find("ItemIcon").gameObject.SetActive(false);
                slot.GetComponentInChildren<Text>().text = "";
                print("cleaned slotes");
            }

            // Preencher slots com itens
            for (int i = 0; i < Inventory.Instance.items.Count; i++)
            {
                print("entra aqui?");
                Item item = Inventory.Instance.items[i];
                GameObject slot = slots[i];
                Image icon = slot.transform.Find("ItemIcon").GetComponent<Image>();
                if (item.icon != null)
                {
                    icon.sprite = item.icon;
                    icon.gameObject.SetActive(true);
                    print("entrei aqui e era pra mudar algo");
                }
                // Se desejar exibir a quantidade
                if (item.isStackable)
                {
                    Text quantityText = slot.GetComponentInChildren<Text>();
                    quantityText.text = item.quantity.ToString();
                    print("aqui era pra mudar a quantidade");
                }
            }

        
        
    }

    public void OpenInventory()
    {
        UpdateUI();
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
