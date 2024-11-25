using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6; // Vida máxima do jogador
    private int currentHealth;

    [SerializeField] private Image[] healthSprites; // Array com 6 sprites de vida
    [SerializeField] private Sprite fullHeartSprite; // Sprite para coração cheio
    [SerializeField] private Sprite emptyHeartSprite; // Sprite para coração vazio

    public static PlayerUI instance;

    private void Start()
    {
        instance = this;
        // Inicia a vida do jogador com o valor máximo e configura os sprites
        currentHealth = maxHealth;
        UpdateHealthUI();
    }


    // Função para atualizar a UI com base na vida atual
    public void UpdateHealthUI()
    {
        print("aaa");
        switch (PlayerMovement.instance.health)
        {
            case 6:
                healthSprites[5].sprite = fullHeartSprite;
                healthSprites[4].sprite = fullHeartSprite;
                healthSprites[3].sprite = fullHeartSprite;
                healthSprites[2].sprite = fullHeartSprite;
                healthSprites[1].sprite = fullHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 5:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = fullHeartSprite;
                healthSprites[3].sprite = fullHeartSprite;
                healthSprites[2].sprite = fullHeartSprite;
                healthSprites[1].sprite = fullHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 4:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = emptyHeartSprite;
                healthSprites[3].sprite = fullHeartSprite;
                healthSprites[2].sprite = fullHeartSprite;
                healthSprites[1].sprite = fullHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 3:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = emptyHeartSprite;
                healthSprites[3].sprite = emptyHeartSprite;
                healthSprites[2].sprite = fullHeartSprite;
                healthSprites[1].sprite = fullHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 2:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = emptyHeartSprite;
                healthSprites[3].sprite = emptyHeartSprite;
                healthSprites[2].sprite = emptyHeartSprite;
                healthSprites[1].sprite = fullHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 1:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = emptyHeartSprite;
                healthSprites[3].sprite = emptyHeartSprite;
                healthSprites[2].sprite = emptyHeartSprite;
                healthSprites[1].sprite = emptyHeartSprite;
                healthSprites[0].sprite = fullHeartSprite;
                break;

            case 0:
                healthSprites[5].sprite = emptyHeartSprite;
                healthSprites[4].sprite = emptyHeartSprite;
                healthSprites[3].sprite = emptyHeartSprite;
                healthSprites[2].sprite = emptyHeartSprite;
                healthSprites[1].sprite = emptyHeartSprite;
                healthSprites[0].sprite = emptyHeartSprite;
                break;

        }

    }
}
