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

    public Material defaultMaterial;
    public Material newMaterial;

    public List<GameObject> objectsToChangeMaterial = new List<GameObject>();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("TurnedOn", 0f, activationTime + desactivationTime);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !damaging)
        {
            damaging = true;
            StartCoroutine(DamageCoroutine());
        }
    }

    private void OnTriggerExit(Collider collision)
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
        // Animación/ cambio de color, vitro abierta
        ChangeMaterials(newMaterial, objectsToChangeMaterial);
        AdjustLightIntensity(1f);
        StopCoroutine(DamageCoroutine());
        StartCoroutine(DamageCoroutine());
        Invoke("TurnedOff", activationTime);
    }

    void TurnedOff()
    {
        turnedOn = false;
        // Animación/ cambio de color, vitro apagada
        ChangeMaterials(defaultMaterial, objectsToChangeMaterial);
        AdjustLightIntensity(14f);
    }

    void ChangeMaterials(Material material, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }
        }
    }
    void AdjustLightIntensity(float intensity)
    {
        foreach (GameObject obj in objectsToChangeMaterial)
        {
            Light lightComponent = obj.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.intensity = intensity;
            }
        }
    }
}
