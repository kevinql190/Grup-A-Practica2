using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LeekAttack : Enemy
{
    [Header("Leek")]
    private Animator animator;
    public float rotationAttackTime = 0.5f;
    public Slider healthSliderLeek;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        healthSliderLeek.value = CurrentHealth;
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
                Debug.Log("aaaaaaaaaa");
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
                Debug.Log("aaaaaaaaaa");
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