using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LeekAttack : Enemy
{
    private Camera mainCamera;

    [Header("Leek")]
    private Animator animator;
    public float rotationAttackTime = 0.5f;
    public Slider healthSliderLeek;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        healthSliderLeek.value = CurrentHealth;
        // Orientar el slider a la camera
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f;
        healthSliderLeek.transform.rotation = Quaternion.LookRotation(cameraForward);
    }

    public override void Attack()
    {
        StartCoroutine(RotateAndAttack());
    }

    IEnumerator RotateAndAttack()
    {
        Quaternion targetRotation = Quaternion.Euler(70, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float elapsedTime = 0;
        float rotationDuration = rotationAttackTime;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, (elapsedTime / rotationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    void OnTriggerEnter(Collider Collider)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareAttack"))
        {
            if (Collider.gameObject.CompareTag("Player")) 
            {
                Damager();
            }
            if (Collider.gameObject.layer == LayerMask.NameToLayer("Breakables")) 
            {
                Collider.GetComponent<IDamageable>().TakeDamage(-damage);
            } 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Breakables"))
            {
                other.GetComponent<IDamageable>().TakeDamage(-damage);
            }
        }
    }

    public override void FacePlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}