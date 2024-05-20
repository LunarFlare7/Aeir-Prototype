using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : StateMachineBehaviour
{

    public float speed;
    public float moveDir;
    Boss bossScript;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossScript = animator.GetComponent<Boss>();
        rb = animator.GetComponent<Rigidbody2D>();
        moveDir = Mathf.Sign(bossScript.target.position.x - animator.transform.position.x);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
        if (Mathf.Sign(bossScript.target.position.x - animator.transform.position.x) != moveDir)
        {
            animator.SetTrigger("ChargedPastPlayer");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity /= 4;
        animator.ResetTrigger("ChargedPastPlayer");
    }
}
