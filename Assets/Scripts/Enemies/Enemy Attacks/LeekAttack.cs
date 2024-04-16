using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeekAttack : Enemy
{
    [Header("Leek")]
    private Animator animator;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        StartCoroutine(RotateAndAttack());
    }

    IEnumerator RotateAndAttack()
    {
        FacePlayer();
        Quaternion targetRotation = Quaternion.Euler(70, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float elapsedTime = 0;
        float rotationDuration = 0.5f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, (elapsedTime / rotationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el ataque está activado y si ha colisionado con el jugador
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (collision.gameObject.CompareTag("Player")) 
            {
                Debug.Log("Leek damage: " + damage);
                Damager();
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