using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    /// <summary>
    /// Script for loading screen to game scene
    /// </summary>
    private void Start()
    {
        StartCoroutine(RestartTimer());
        Debug.Log("Loading");
    }
    

    IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameScene");
    }
    
}
