using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour2 : BaseBehaviour
{
    private float Speed = 2;
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

        Move(animator.transform);
    }
    private void Move(Transform mySelf)
    {
        Vector3 targetPos = new Vector3(_player.position.x, mySelf.position.y, _player.position.z);

        mySelf.LookAt(targetPos);

        mySelf.Translate(mySelf.forward * Speed * Time.deltaTime, Space.World);
    }
}
