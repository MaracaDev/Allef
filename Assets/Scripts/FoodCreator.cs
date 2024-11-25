using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FoodCreator : MonoBehaviour
{
    // Ingredientes disponíveis no inventário
    public int NucleoAcido = 3;
    public int VenenoViuva = 5;
    public int FragmentoCoracaoRochoso = 2;
    public int CoracaoRochaViva = 1;

    public TMP_Text coracao, fragmento, veneno, nucleo;

    // Dicionário de receitas
    private Dictionary<string, Dictionary<string, int>> receitas;

    private void Start()
    {
        UpdateValues();
        // Inicializar as receitas
        receitas = new Dictionary<string, Dictionary<string, int>>()
        {
            {
                "Prato 1", new Dictionary<string, int>
                {
                    { "NucleoAcido", 2 },
                    { "VenenoViuva", 1 },
                    { "FragmentoCoracaoRochoso", 1 }
                }
            },
            {
                "Prato 2", new Dictionary<string, int>
                {
                    { "FragmentoCoracaoRochoso", 3 },
                    { "NucleoAcido", 1 },
                    { "VenenoViuva", 1 }
                }
            },
            {
                "Prato 3", new Dictionary<string, int>
                {
                    { "CoracaoRochaViva", 1 },
                    { "FragmentoCoracaoRochoso", 2 },
                    { "NucleoAcido", 2 }
                }
            }
        };
    }
    public void UpdateValues()
    {
        coracao.text = CoracaoRochaViva.ToString();
        nucleo.text = NucleoAcido.ToString();
        veneno.text = VenenoViuva.ToString();
        fragmento.text = FragmentoCoracaoRochoso.ToString();
    }

    // Método público para cozinhar
    public void Cozinhar(string prato)
    {
        if (!receitas.ContainsKey(prato))
        {
            Debug.Log("Receita não encontrada.");
            
        }

        var ingredientesNecessarios = receitas[prato];

        // Verificar se há ingredientes suficientes
        foreach (var ingrediente in ingredientesNecessarios)
        {
            int quantidadeDisponivel = GetIngredienteQuantidade(ingrediente.Key);
            if (quantidadeDisponivel < ingrediente.Value)
            {
                Debug.Log($"Faltam ingredientes para {prato}: {ingrediente.Key}");
                
            }
        }

        // Deduzir os ingredientes usados
        foreach (var ingrediente in ingredientesNecessarios)
        {
            UsarIngrediente(ingrediente.Key, ingrediente.Value);
        }

        Debug.Log($"{prato} foi criado com sucesso!");
        HungerSystem.instance.hunger = 100;
        HungerSystem.instance.UpdateHungerBar();
        UpdateValues();

    }

    // Métodos auxiliares para manipular os ingredientes
    private int GetIngredienteQuantidade(string ingrediente)
    {
        return ingrediente switch
        {
            "NucleoAcido" => NucleoAcido,
            "VenenoViuva" => VenenoViuva,
            "FragmentoCoracaoRochoso" => FragmentoCoracaoRochoso,
            "CoracaoRochaViva" => CoracaoRochaViva,
            _ => 0
        };
    }

    private void UsarIngrediente(string ingrediente, int quantidade)
    {
        switch (ingrediente)
        {
            case "NucleoAcido":
                NucleoAcido -= quantidade;
                break;
            case "VenenoViuva":
                VenenoViuva -= quantidade;
                break;
            case "FragmentoCoracaoRochoso":
                FragmentoCoracaoRochoso -= quantidade;
                break;
            case "CoracaoRochaViva":
                CoracaoRochaViva -= quantidade;
                break;
        }
    }
}
