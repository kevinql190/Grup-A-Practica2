using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        _playerMovement.OnDashChargeChanged += DashSliderChange;
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
}
