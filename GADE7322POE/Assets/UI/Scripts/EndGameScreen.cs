using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndGameScreen : UtkBase
{
    private Button _mRestartBtn;
    private Button _mReturnToMenuBtn;
    private Button _mQuitGameBtn;
    
    protected override void Start() //Setup pause UI
    {
        base.Start();
        
        _mRestartBtn = rootElement.Q<Button>("RestartBtn");
        _mReturnToMenuBtn = rootElement.Q<Button>("ReturnToMenuBtn");
        _mQuitGameBtn = rootElement.Q<Button>("QuitBtn");

        //Handle button presses

        _mRestartBtn.clicked += () =>
        {
            Time.timeScale = 1;
            SoundManager.Instance.PlaySound(SoundType.Button);
            SceneManager.LoadScene(2);
        };

        _mReturnToMenuBtn.clicked += () =>
        {
            Time.timeScale = 1;
            SoundManager.Instance.PlaySound(SoundType.Button);
            SceneManager.LoadScene(0);
        };

        //Quit game press
        _mQuitGameBtn.clicked += Application.Quit;
        
        Health.onGameEnd += HandleEndGame;
        
    }

    private void HandleEndGame()
    {
        Time.timeScale = 0;
        Show();
    }

    private void OnDestroy()
    {
        Health.onGameEnd -= HandleEndGame;
    }
}
