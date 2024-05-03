using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour3 : BaseBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isPreparingAttack
        float attackRange = enemy.attackRange;
        bool isAttacking = InRange(animator.transform, attackRange);
        animator.SetBool("isPreparingAttack", isAttacking);

        // Ara no el persegueix, posa aqui el codi
        Move(animator);
    }
    // Ara no el persegueix, posa aqui el codi
    private void Move(Animator animator)
    {

    }
}
