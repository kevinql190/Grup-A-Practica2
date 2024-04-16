using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CookingSystem : MonoBehaviour
{
    [Header("Pan Burnt")]
    [SerializeField] private float stunTime;
    [SerializeField] private float stunSpeedDecrease;
    [SerializeField] private int lifeLoss;
    [Header("Skills")]
    public bool cooked = false;
    public bool skillCasted = false;

    public event Action<float> OnCookingProgressChanged;
    public event Action<float> OnSparingProgressChanged;
    public float CurrentCookingProgress
    {
        get { return _cookingprogress; }
        set { _cookingprogress = value; OnCookingProgressChanged?.Invoke(value); }
    }
    public float CurrentSparingProgress
    {
        get { return _sparingprogress; }
        set { _sparingprogress = value; OnSparingProgressChanged?.Invoke(value); }
    }
    private float _cookingprogress;
    private float _sparingprogress;
    public IEnumerator CookingProcess(float cookingTime, float spareTime)
    {
        AudioManager.Instance.PlaySFXLoop("cooking_loop", 0.5f, 1f);
        //Debug.Log("Started Cooking");
        float timer = 0f;
        while (timer < cookingTime) //Cooking
        {
            timer += Time.deltaTime;
            CurrentCookingProgress = timer / cookingTime;
            yield return null;
        }
        CurrentCookingProgress = 0f;
        GetComponent<SkillAbilities>().SetAbility(true);
        //Debug.Log("Cooked"); // Cooked
        timer = 0f;
        while (timer < spareTime && !skillCasted) //Spare Time
        {
            timer += Time.deltaTime;
            CurrentSparingProgress = timer / cookingTime;
            yield return null;
        }
        CurrentSparingProgress = 0f;
        if (!skillCasted) PanBurnt(); //Burnt
        GetComponent<PanController>().ChangeFoodType(FoodType.Default);
        AudioManager.Instance.StopLoop("cooking_loop", 1.5f);
    }
    private void PanBurnt()
    {
        Debug.Log("Pan Burnt!");
        GetComponent<SkillAbilities>().SetAbility(false);
        StartCoroutine(GetComponent<PlayerMovement>().StunPlayer(stunTime, stunSpeedDecrease));
        GetComponent<IDamageable>().TakeDamage(-lifeLoss);
    }
}
