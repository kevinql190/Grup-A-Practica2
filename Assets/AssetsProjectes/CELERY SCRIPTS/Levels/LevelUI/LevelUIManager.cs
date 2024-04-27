using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private BlackFade blackFade;
    [SerializeField] private float fadeTime = 1.5f;
    private PlayerHealth _playerHealth;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMainPanel;
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject receptariPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject endLevelCanvas;
    [SerializeField] private GameObject endLevelFirstSelected;
    private PauseMenuManager _pauseMenu;
    private EndMenuManager _endMenu;
    private void Awake()
    {
        _pauseMenu = GetComponent<PauseMenuManager>();
        _endMenu = GetComponent<EndMenuManager>();
    }
    void Start()
    {
        GetComponent<ButtonInstantiator>().GenerateReceptariPanel();
    }
    private void OnEnable()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _playerHealth.OnPlayerDeath += ReturnCheckpoint;
    }
    private void OnDisable()
    {
        _playerHealth.OnPlayerDeath -= ReturnCheckpoint;
    }
    private void Update()
    {
        HandleEscape();
    }
    private void HandleEscape()
    {
        if (!PlayerInputHandler.PauseJustPressed) return;
        ReturnFromReceptari();
    }

    public void ReturnFromReceptari()
    {
        if (_pauseMenu.isPaused && !pauseMainPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            receptariPanel.SetActive(false);
            pauseMainPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pauseFirstSelected); 
        }
        else if (!_endMenu.isLevelEndActive)
        {
            _pauseMenu.SetPause(!_pauseMenu.isPaused);
            Cursor.lockState = _pauseMenu.isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            pausePanel.SetActive(_pauseMenu.isPaused);
            if(_pauseMenu.isPaused)
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
        }
        else
        {
            endLevelCanvas.SetActive(true);
            receptariPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(endLevelFirstSelected);
        }
    }
    #region Return Checkpoint
    public void ReturnCheckpoint()
    {
        StartCoroutine(ReturnCheckpointSequence());
    }
    private IEnumerator ReturnCheckpointSequence()
    {
        PlayerInputHandler.Instance.DisableInputs();
        Time.timeScale = 0;
        AudioManager.Instance.StopAllLoops(fadeTime);
        yield return StartCoroutine(blackFade.FadeToBlack(fadeTime));
        Time.timeScale = 1;
        CrossSceneInformation.CurrentTimerValue = LevelManager.Instance.elapsedTime;
        PlayerInputHandler.Instance.EnableInputs();
        //AudioManager.Instance.StopAllLoops();
        GetComponent<ASyncLoader>().LoadLevelBtn(SceneManager.GetActiveScene().name);
    }
    #endregion
    #region Exit Level
    private IEnumerator ExitSequence(string scene)
    {
        AudioManager.Instance.StopAllLoops(fadeTime);
        yield return StartCoroutine(blackFade.FadeToBlack(fadeTime));
        AudioListener.pause = false;
        Time.timeScale = 1;
        CrossSceneInformation.CurrentCheckpoint = 0;
        CrossSceneInformation.CurrentTimerValue = 0;
        PlayerInputHandler.Instance.EnableInputs();
        GetComponent<ASyncLoader>().LoadLevelBtn(scene);
    }
    public void ExitEndLevel(string scene)
    {
        StartCoroutine(ExitSequence(scene));
    }
    public void RestartLevel()
    {
        StartCoroutine(ExitSequence(SceneManager.GetActiveScene().name));
    }
    #endregion
}
