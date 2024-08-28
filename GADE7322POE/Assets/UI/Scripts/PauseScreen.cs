using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseScreen : UtkBase
{
    private Button _mContinueBtn;
    private Button _mSettingsBtn;
    private Button _mReturnToMenuBtn;
    private Button _mQuitGameBtn;

    public UnityEvent onContinueGame;
    
    protected override void Start()
    {
        base.Start();

        _mContinueBtn = rootElement.Q<Button>("ResumeBtn");
        _mSettingsBtn = rootElement.Q<Button>("SettingsBtn");
        _mReturnToMenuBtn = rootElement.Q<Button>("ReturnToMenuBtn");
        _mQuitGameBtn = rootElement.Q<Button>("QuitBtn");

        _mContinueBtn.clicked += () =>
        {
            Time.timeScale = 1;
            onContinueGame?.Invoke();
        };

        _mSettingsBtn.clicked += () =>
        {
            Debug.Log("Settings");
        };

        _mReturnToMenuBtn.clicked += () =>
        {
            SceneManager.LoadScene(0);
        };

        _mQuitGameBtn.clicked += Application.Quit;
        

    }
}
