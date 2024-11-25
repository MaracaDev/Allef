using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{

    public Image tutorialImage; // Referência ao componente de imagem onde as imagens do tutorial serão exibidas
    public Sprite[] tutorialPages; // Array de imagens do tutorial (adicione as duas imagens no Inspetor)
    public Image Ad;

    private int currentPage = 0; // Índice da página atual

    // Função para mostrar a próxima página
    public void AvancarPagina()
    {
        if (currentPage < tutorialPages.Length - 1) // Verifica se há uma página seguinte
        {
            currentPage++; // Avança para a próxima página
            AtualizarImagem(); // Atualiza a imagem no componente de imagem
        }

        else {
            tutorialImage.gameObject.SetActive(false);
            currentPage = 0;
        }
    }

    public void Tutorial()
    {

        currentPage = 0;
        AtualizarImagem(); // Atualiza a imagem no componente de imagem
    }

    

    // Atualiza a imagem exibida com base no índice da página atual
    private void AtualizarImagem()
    {
        tutorialImage.sprite = tutorialPages[currentPage];
    }

    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


    public void OpenAd()
    {
        Ad.gameObject.SetActive(true);
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        HungerSystem.instance.hunger = 100;
        HungerSystem.instance.UpdateHungerBar();
        Ad.gameObject.SetActive(false);
    }


}
