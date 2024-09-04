using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    public static SceneLoaderScript instance;

    public static SceneLoaderScript Instance
    {
        get
        {
            if (Instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        StopCoroutine(RestartTimer());
    }

    IEnumerator RestartTimer()
    {
        SceneManager.LoadScene(2);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<SceneLoaderScript>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "Scene Loader";
            instance = gameObj.AddComponent<SceneLoaderScript>();
            DontDestroyOnLoad(gameObj);
        }
    }
}
