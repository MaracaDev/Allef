using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour
{
    // Singleton para acesso global
    public static AudioControler Instance { get; private set; }

    [Header("Configura��es de �udio")]
    public AudioSource audios; // Refer�ncia ao AudioSource
    public List<AudioClip> audioClips; // Lista de sons dispon�veis

    private void Awake()
    {
        // Configurando o Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre cenas, se necess�rio
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Toca um som da lista pelo �ndice.
    /// </summary>
    /// <param name="index">�ndice do som na lista</param>
    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audios.clip = audioClips[index];
            audios.Play();
        }
        else
        {
            Debug.LogWarning("�ndice inv�lido para a lista de sons!");
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
            Debug.LogWarning($"Som '{clipName}' n�o encontrado na lista de sons!");
        }
    }
}
