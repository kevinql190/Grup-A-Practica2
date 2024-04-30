using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TomatoAttack : Enemy
{
    //Animació provisional BORRAR quan tinguem les animacions
    [Header("Animació provisional :)")]
    public float attackScaleMultiplier = 1.5f;
    private Vector3 originalScale;
    public Slider heealthSliderTomato;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        originalScale = transform.localScale; // Borrar quan tinguem les animacions
    }
    private void Update()
    {
        heealthSliderTomato.value = CurrentHealth;
    }
    public override void Attack()
    {
        Debug.Log(prepareAttackTime);
        if (InRange())
        {
            Debug.Log("Tomato damage: " + damage);
            Damager();
        }
    }
    public override void FacePlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}
