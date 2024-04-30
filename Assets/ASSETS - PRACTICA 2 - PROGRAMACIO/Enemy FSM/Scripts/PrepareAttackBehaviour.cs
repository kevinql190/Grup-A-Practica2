using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAttackBehaviour : BaseBehaviour
{
    protected float _timer;
    protected float prepareAttackTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.FacePlayer();
        _timer = 0;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prepareAttackTime = enemy.prepareAttackTime;
        animator.SetBool("isAttacking", CheckTime());
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > prepareAttackTime;
    }
}