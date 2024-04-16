using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBehaviour : BaseBehaviour
{
    protected float _timer;
    protected float cooldownAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float attackRange = enemy.attackRange;
        float cooldownAttack = enemy.cooldownAttack;
        if (CheckTime()) 
        {
            animator.SetBool("inRangeAttack", InRange(animator.transform, attackRange));
        }
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > cooldownAttack;
    }
}
