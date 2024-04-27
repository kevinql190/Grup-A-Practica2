using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenHeadMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private NavMeshAgent agent;
    [SerializeField] private float rotationAngle;
    [Header("Attack")]
    [SerializeField] private float attackChargeTime;
    [SerializeField] private float attackSpeed;
    private int rotationSign;
    private bool isAttacking = false;
    private Vector3 startingScale;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        rotationSign = Random.Range(0, 1) * 2 - 1;
        startingScale = transform.localScale;
    }

    void Update()
    {
        if (isAttacking) return;
        if(Physics.Raycast(transform.position, transform.forward, out _, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            StartCoroutine(ChargedAttackSequence());
        }
        else
        {
            agent.SetDestination(transform.position + Quaternion.Euler(0, rotationAngle * rotationSign, 0) * transform.forward - Vector3.up * agent.height / 2);
        }
    }

    private IEnumerator ChargedAttackSequence()
    {
        isAttacking = true;
        float t = 0;
        while (t < attackChargeTime)
        {
            t += Time.deltaTime;
            float value = t / attackChargeTime;
            transform.localScale = Vector3.Lerp(startingScale, new Vector3(5, 5, 5), value);
            yield return null;
        }
        transform.localScale = startingScale;
        agent.speed = attackSpeed;
        while(NavMesh.SamplePosition(transform.position + transform.forward * 3 - Vector3.up * agent.height / 2 * transform.localScale.y, out _, 1.5f, NavMesh.AllAreas))
        {
            agent.SetDestination(transform.position + transform.forward - Vector3.up * agent.height / 2);
            yield return null;
        }
        agent.speed = speed;
        rotationSign = Random.Range(0, 2) * 2 - 1;
        isAttacking = false;
    }
}
