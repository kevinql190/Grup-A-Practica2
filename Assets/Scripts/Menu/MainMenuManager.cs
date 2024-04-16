using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("Receptari Scroll")]
    public GameObject lastSelected;
    [Header("Start Game Text")]
    [SerializeField] private TMP_Text startGameText;
    [SerializeField] private Button selectLevel;
    private void Start()
    {
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("UI");
        GetComponent<ButtonInstantiator>().GenerateReceptariPanel();
        GetComponent<ButtonInstantiator>().GenerateLevelPanel();
        SetStartGameText();
    }
    #region Start Game Text
    private void SetStartGameText()
    {
        startGameText.text = GameManager.Instance.levels[0].unlocked ? "CONTINUE GAME" : "START NEW GAME";
        selectLevel.interactable = GameManager.Instance.levels[0].unlocked;
    }
    #endregion
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

}
