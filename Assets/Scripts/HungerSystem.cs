using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Necessário para detectar e monitorar a cena atual
using System.Collections.Generic;
public class HungerSystem : MonoBehaviour
{
    public float hunger = 100f; // Fome começa cheia
    public float hungerDecreaseRate = 10f; // A cada quanto tempo a fome diminui (em segundos)
    public int maxHunger = 6; // Número máximo de quadradinhos de fome (barra de fome de 6 quadrados)
    public List<Image> hungerBarSquares; // Lista de quadrados da barra de fome (cada Image representa um quadrado)
    public Image hungerBar; // A UI da barra de fome inteira (opcional, pode ser removida se não usada)

    private float hungerTimer = 0f; // Temporizador para a diminuição da fome
    private int currentHungerLevel; // Nível atual de fome (quantos quadrados visíveis)
    private bool isInDg1Scene = false; // Controla se estamos na cena "Dg1"
    public static HungerSystem instance;

    private void Start()
    {
        instance = this;
        // Inicializa a barra de fome com 6 quadrados visíveis
        currentHungerLevel = maxHunger;
        UpdateHungerBar();

       
    }

   

    private void Update()
    {
        // Só diminui a fome se estivermos na cena "Dg1"
       
        
            // A cada segundo, diminui a fome
            hungerTimer += Time.deltaTime;

            if (hungerTimer >= hungerDecreaseRate)
            {
                // Reduz a fome
                hunger -= 100f / maxHunger; // Decrementa 1 quadrado de fome por vez
                hunger = Mathf.Clamp(hunger, 0f, 100f); // Garantir que a fome não seja negativa

                // Atualiza a barra de fome e a UI
                UpdateHungerBar();

                // Reseta o temporizador
                hungerTimer = 0f;
            }
        
    }

    // Atualiza a barra de fome removendo quadrados
    public void UpdateHungerBar()
    {
        // Calcula quantos quadrados da barra devem estar visíveis
        currentHungerLevel = Mathf.FloorToInt(hunger / (100f / maxHunger));

        // Atualiza os quadrados da barra de fome (deixa visível até o nível atual)
        for (int i = 0; i < maxHunger; i++)
        {
            if (i < currentHungerLevel)
            {
                hungerBarSquares[i].gameObject.SetActive(true); // Ativa o quadrado
            }
            else
            {
                hungerBarSquares[i].gameObject.SetActive(false); // Desativa o quadrado
            }
        }
    }

   
}
