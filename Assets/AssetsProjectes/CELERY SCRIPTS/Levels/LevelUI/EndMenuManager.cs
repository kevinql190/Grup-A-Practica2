using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject endLevelCanvas;
    [SerializeField] private TextMeshProUGUI scoreTimer;
    [SerializeField] private TextMeshProUGUI highScoreTimer;
    public bool isLevelEndActive = false;
    private void OnEnable()
    {
        LevelManager.Instance.OnLevelEnd += ShowLevelEnd;
    }
    private void ShowLevelEnd()
    {
        isLevelEndActive = true;
        endLevelCanvas.SetActive(true);
        StartCoroutine(endLevelCanvas.GetComponent<AlphaLerper>().LerpAlpha(1.5f, true));
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
        Time.timeScale = 0;
        GetScore();
    }

    private void GetScore()
    {
        float time = LevelManager.Instance.elapsedTime;
        int extractedDecimals = (int)((time - (int)time) * 100);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        scoreTimer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, extractedDecimals);
        highScoreTimer.text = scoreTimer.text;
    }
}
