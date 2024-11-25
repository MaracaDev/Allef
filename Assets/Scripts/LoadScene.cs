using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;

    private void Awake()
    {
        instance = this;
    }
    public void LoaderScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
