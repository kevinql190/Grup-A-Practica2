using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vitro : MonoBehaviour
{
    [Header("Vitro")]
    public int damage = 1;
    [SerializeField] private GameObject target;
    public float vitroCooldownDamage = 2f;
    private bool damaging = false;

    [Header("Turned On/ Off")]
    public float activationTime = 5f; // Tiempo para activar la vitro
    public float desactivationTime = 5f; // Tiempo para desactivar la vitro
    private bool turnedOn = false;

    // Material Vitro
    public Material vitroMaterial;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("TurnedOn", 0f, activationTime + desactivationTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damaging)
        {
            damaging = true;
            StartCoroutine(DamageCoroutine());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damaging = false;
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (damaging && turnedOn)
        {
            target.GetComponent<IDamageable>().TakeDamage(-damage);
            yield return new WaitForSeconds(vitroCooldownDamage);
        }
    }
    void TurnedOn()
    {
        turnedOn = true;
        // Animació/ canvi color, vitro oberta
        if (vitroMaterial != null)
        {
            vitroMaterial.color = Color.red;
        }
        StopCoroutine(DamageCoroutine());
        StartCoroutine(DamageCoroutine());
        Invoke("TurnedOff", activationTime);
    }

    void TurnedOff()
    {
        turnedOn = false;
        // Animació/ canvi color, vitro apagada
        if (vitroMaterial != null)
        {
            vitroMaterial.color = Color.black; 
        }

    }
}
