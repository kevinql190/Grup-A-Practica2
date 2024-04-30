using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TomatoAttack : Enemy
{
    public Slider heealthSliderTomato;
    private Collider attackCollider;
    private Camera mainCamera;

    [Header("Tomato Sphere Attack")]
    public float sphereRadius = 0.6f;
    public float distanceAhead = 0.9f; 
    public float distanceAbove = 0.6f;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        mainCamera = Camera.main;
    }

    private void Update()
    {
        heealthSliderTomato.value = CurrentHealth;
        // Orientar el slider a la camera
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f; 
        heealthSliderTomato.transform.rotation = Quaternion.LookRotation(cameraForward);
    }

    public override void Attack()
    {
        CreateAttackCollider();
    }

    public override void FacePlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    private void CreateAttackCollider()
    {
        if (attackCollider != null)
        {
            Destroy(attackCollider.gameObject);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * distanceAhead + Vector3.up * distanceAbove, sphereRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Damager();
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Breakables"))
            {
 
                collider.GetComponent<IDamageable>().TakeDamage(-damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * distanceAhead + Vector3.up * distanceAbove, sphereRadius);
    }
}
