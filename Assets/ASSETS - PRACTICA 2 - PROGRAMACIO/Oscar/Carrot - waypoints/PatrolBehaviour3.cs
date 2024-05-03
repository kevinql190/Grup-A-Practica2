using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour3 : BaseBehaviour
{
    protected float _timer;
    public float WaitTime = 3;
    float speed = 3.0f;

    public Transform target;
    private Animator _animator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        _animator = animator;
        target = GameObject.FindGameObjectWithTag("PathManagerWaypoints").transform;
        // Patrolling
        if (target != null) {
            animator.gameObject.transform.LookAt(new Vector3(target.position.x, animator.transform.position.y, target.position.z));
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isChasing
        float enemyDetectionDistance = enemy.detectionDistance;
        if (InRange(animator.transform, enemyDetectionDistance) && CheckPlayerVisibility(animator))
        {
            animator.SetBool("isChasing", true);
        }

        // isPatroling
        bool time = CheckTime();
        animator.SetBool("isPatroling", !time);

        // Patrolling
        animator.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > WaitTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.tag == "WayPoint" || other.tag == "PathManagerWaypoints")
        {
            target = other.gameObject.GetComponent<WayPoint>().nexPoint;
            if (_animator != null) 
            {
                _animator.transform.LookAt(new Vector3(target.position.x, _animator.transform.position.y, target.position.z));
            }
        }
    }
}

