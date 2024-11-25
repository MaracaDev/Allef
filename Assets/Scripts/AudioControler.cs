using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour
{
    // Singleton para acesso global
    public static AudioControler Instance { get; private set; }

    [Header("Configurações de Áudio")]
    public AudioSource audios; // Referência ao AudioSource
    public List<AudioClip> audioClips; // Lista de sons disponíveis

    private void Awake()
    {
        // Configurando o Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre cenas, se necessário
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Toca um som da lista pelo índice.
    /// </summary>
    /// <param name="index">Índice do som na lista</param>
    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audios.clip = audioClips[index];
            audios.Play();
        }
        else
        {
            Debug.LogWarning("Índice inválido para a lista de sons!");
        }
    }

    /// <summary>
    /// Toca um som pelo nome do AudioClip.
    /// </summary>
    /// <param name="clipName">Nome do AudioClip</param>
    public void PlaySound(string clipName)
    {
        AudioClip clip = audioClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            audios.clip = clip;
            audios.Play();
        }
        else
        {
            Debug.LogWarning($"Som '{clipName}' não encontrado na lista de sons!");
        }
    }
}
