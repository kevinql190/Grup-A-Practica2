using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBehaviour : BaseBehaviour
{
    [Header("Enemy")]
    public FoodScriptableObject EnemyType;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.CurrentHealth = EnemyType.enemyHealth;
        animator.SetBool("needsHeal", false);
        animator.SetBool("isHealing", false);
    }
}
