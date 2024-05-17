using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FaceTarget : StateMachineBehaviour
{
    Boss bossScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossScript = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool facingRight = animator.GetBool("FacingRight");
        if ((bossScript.target.transform.position.x - animator.transform.position.x) > 0)
        {
            //face right
            if(!facingRight)
            {
                animator.SetTrigger("Turn");
                animator.SetBool("FacingRight", true);
            }
        }
        else if ((bossScript.target.transform.position.x - animator.transform.position.x) < 0)
        {
            //face left
            if (facingRight)
            {
                animator.SetTrigger("Turn");
                animator.SetBool("FacingRight", false);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Turn");
        Debug.Log("turn");
    }
}
