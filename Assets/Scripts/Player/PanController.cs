using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanController : MonoBehaviour
{
    [Header("FoodType Properties")]
    public FoodType currentFoodType = FoodType.Default;
    FoodScriptableObject CurrentFood => Array.Find(GameManager.Instance.receptariInfo, x => x.FoodType.FoodType == currentFoodType).FoodType;
    private GameObject CurrentPrefabAssigned => CurrentFood.prefabAssigned;
    private float CurrentSpareCookingTime => CurrentFood.cookingTime;
    private float CurrentCookingTime => CurrentFood.spareCookingTime;
    [HideInInspector] public Sprite CurrentSkillSprite => CurrentFood.skillSprite;
    private void Start()
    {
        // Actualitzar Mesh al principi
        UpdatePanPrefab();
    }

    // Mètode per canviar FoodType
    public void ChangeFoodType(FoodType newState)
    {
        currentFoodType = newState;
        if (newState != FoodType.Default)
        {
            AudioManager.Instance.PlaySFXOnce("agafar_ingredient", 0.5f);
            StartCoroutine(GetComponent<CookingSystem>().CookingProcess(CurrentCookingTime, CurrentSpareCookingTime));
        }
        UpdatePanPrefab();
    }
    // Update the prefab based on the current pan state
    private void UpdatePanPrefab()
    {
        Transform childToDestroy = transform.Find("PanContent");
        if (childToDestroy != null)
        {
            Destroy(childToDestroy.gameObject);
        }
        GameObject newObject = Instantiate(CurrentPrefabAssigned, transform);
        newObject.name = "PanContent";
        }

}