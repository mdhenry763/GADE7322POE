using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : UtkBase
{

    public UnityEvent onShowRef;
    
    // Start is called before the first frame update
    protected override void Start()
    {

        var startGameBtn = rootElement.Q<Button>("StartBtn");
        var quitBtn = rootElement.Q<Button>("QuitBtn");
        var refBtn = rootElement.Q<Button>("ReferencesBtn");

        startGameBtn.clicked += (() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        quitBtn.clicked += Application.Quit;

        refBtn.clicked += () =>
        {
            onShowRef.Invoke();
        };

    }
}
