using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseScreen : UtkBase
{
    private Button _mContinueBtn;
    private Button _mRestartBtn;
    private Button _mReturnToMenuBtn;
    private Button _mQuitGameBtn;

    public UnityEvent onContinueGame;
    
    protected override void Start() //Setup pause UI
    {
        base.Start();

        _mContinueBtn = rootElement.Q<Button>("ResumeBtn");
        _mRestartBtn = rootElement.Q<Button>("RestartBtn");
        _mReturnToMenuBtn = rootElement.Q<Button>("ReturnToMenuBtn");
        _mQuitGameBtn = rootElement.Q<Button>("QuitBtn");

        //Handle button presses
        _mContinueBtn.clicked += () =>
        {
            Time.timeScale = 1;
            onContinueGame?.Invoke();
            SoundManager.Instance.PlaySound(SoundType.Button);
        };

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
        
    }
}
