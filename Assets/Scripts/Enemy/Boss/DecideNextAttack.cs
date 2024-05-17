using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideNextAttack : StateMachineBehaviour
{
    public float minLoopsBetweenAttacks;
    public float maxLoopsBetweenAttacks;
    float nextAttackTime;
    Boss bossScript;
    Animator ani;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossScript = animator.GetComponent<Boss>();
        ani = animator;
        nextAttackTime = Random.Range(minLoopsBetweenAttacks, maxLoopsBetweenAttacks);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= nextAttackTime)
        {
            DecideAttack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ani.ResetTrigger("WalkToEdge");
    }

    public void DecideAttack()
    {
        if (ani.transform.position.x > bossScript.minX + bossScript.spaceFromEdge && ani.transform.position.x < bossScript.maxX - bossScript.spaceFromEdge)
        {
            ani.SetTrigger("WalkToEdge");
        }
        else
        {
            ani.SetTrigger("Attack");
        }
    }
}
