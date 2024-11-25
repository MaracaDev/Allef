using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject telaDeLoading; // Associe a tela de loading no inspetor
    public Slider barraDeProgresso;  // Associe o slider da barra de progresso no inspetor

    // Função para iniciar o carregamento da cena
    public void CarregarCena(string nomeDaCena)
    {
        StartCoroutine(CarregarCenaAsync(nomeDaCena));
    }

    private IEnumerator CarregarCenaAsync(string nomeDaCena)
    {
        // Ativa a tela de loading
        telaDeLoading.SetActive(true);

        yield return new WaitForSeconds(3f);
        // Inicia o carregamento assíncrono da cena
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeDaCena);

        

        // Desativa a tela de loading após o carregamento completo
        telaDeLoading.SetActive(false);
    }

    private void Start()
    {
        
        telaDeLoading.SetActive(false);
    }
}
