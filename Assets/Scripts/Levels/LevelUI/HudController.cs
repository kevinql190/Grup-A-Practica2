using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject canvas;
    private GameObject _player;
    [Header("Life System")]
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private PlayerHealth _playerHealth;
    private CookingSystem _cookingSystem;
    private PlayerMovement _playerMovement;
    [Header("Dash Slider")]
    [SerializeField] private float dashSliderChangeVelocity;
    [SerializeField] List<Sprite> dashHandleSprites;
    private Slider dashSlider;
    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerMinSecs;
    [SerializeField] TextMeshProUGUI timerMilisec;
    private float ElapsedTime => LevelManager.Instance.elapsedTime;
    private void Awake()
    {
        dashSlider = canvas.transform.Find("DashSlider").transform.Find("Slider").GetComponent<Slider>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _cookingSystem = _player.GetComponent<CookingSystem>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        SetCanvasHearts();
    }
    private void OnEnable()
    {
        _playerHealth.OnHealthChanged += UpdateHearts;
        _cookingSystem.OnCookingProgressChanged += UpdateCookSlider;
        _cookingSystem.OnSparingProgressChanged += UpdateSpareSlider;
        _playerMovement.OnDashChargeChanged += DashSliderChange;
    }
    private void Update()
    {
        UpdateTimer();
    }
    #region Life HUD
    private void SetCanvasHearts()
    {
        for (int i = 0; i < _playerHealth.maxHealth; i++)
        {
            Instantiate(heartPrefab, heartsContainer);
        }
    }

    private void UpdateHearts(int health)
    {
        foreach (Transform child in canvas.transform.Find("Hearts").transform)
        {
            if (child.GetSiblingIndex() < health)
            {
                child.GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                child.GetComponent<Image>().sprite = emptyHeart;
            }
        }
    }
    #endregion
    #region Cooking Skill Slider HUD
    public void UpdateCookSlider(float amount)
    {
        Color handleColor = amount == 0f || amount == 360f ? new Color32(78, 57, 57, 255) : new Color32(255, 213, 65, 255);
        canvas.transform.Find("SkillSlider").transform.Find("PanFill").GetComponent<Image>().fillAmount = amount;
        canvas.transform.Find("SkillSlider").transform.Find("PanHandle").transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -amount * 360));
        canvas.transform.Find("SkillSlider").transform.Find("PanHandle").GetComponent<Image>().color = handleColor;
    }
    public void UpdateSpareSlider(float amount)
    {
        canvas.transform.Find("SkillSlider").transform.Find("Slider").transform.Find("SliderFill").GetComponent<Image>().fillAmount = amount;
    }
    #endregion
    #region Dash Slider HUD
    private void DashSliderChange(float value)
    {
        Slider slider = canvas.transform.Find("DashSlider").transform.Find("Slider").GetComponent<Slider>();
        var unlerpedValule = Mathf.InverseLerp(0, 3, value);
        if (slider.value < unlerpedValule)
        {
            slider.value = unlerpedValule;
        }
        else
        {
            slider.value -= dashSliderChangeVelocity * Time.deltaTime;
        }
        dashSlider.handleRect.GetComponent<Image>().sprite = dashHandleSprites[(int)value];
    }
    #endregion
    #region Timer
    private void UpdateTimer()
    {
        int extractedDecimals = (int)((ElapsedTime - (int)ElapsedTime) * 100);
        int minutes = Mathf.FloorToInt(ElapsedTime / 60);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60);
        timerMinSecs.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerMilisec.text = extractedDecimals.ToString("00");

    }
    #endregion
}
