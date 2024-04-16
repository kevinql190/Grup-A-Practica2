using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    private bool isPaused = false;
    private void Start()
    {
        GetComponent<ButtonInstantiator>().GenerateReceptariPanel();
    }
    private void Update()
    {
        HandlePause();
    }
    private void HandlePause()
    {
        if (!PlayerInputHandler.PauseJustPressed) return;
        SetPause(!isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        pauseUI.SetActive(isPaused);
    }

    public void SetPause(bool willPause)
    {
        isPaused = willPause;
        AudioListener.pause = willPause;
        Time.timeScale = willPause ? 0 : 1;
        if (willPause) PlayerInputHandler.Instance.LockInputs();
        else PlayerInputHandler.Instance.EnableInputs();
    }
    public void Restart()
    {
        SetPause(false);
        pauseUI.SetActive(false);
        CrossSceneInformation.CurrentTimerValue = LevelManager.Instance.elapsedTime;
        GetComponent<ASyncLoader>().LoadLevelBtn(SceneManager.GetActiveScene().name);
    }
    public void ReturnHome(string menuSceneName)
    {
        SetPause(false);
        CrossSceneInformation.CurrentCheckpoint = 0;
        CrossSceneInformation.CurrentTimerValue = 0;
        GetComponent<ASyncLoader>().LoadLevelBtn(menuSceneName);
    }
}
