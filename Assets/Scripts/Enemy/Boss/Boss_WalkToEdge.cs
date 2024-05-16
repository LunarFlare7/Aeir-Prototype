using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_WalkToEdge : StateMachineBehaviour
{
    public Boss bossScript;
    public Rigidbody2D rb;
    public float spaceFromEdge;
    public float speed;
    private float moveDir;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossScript = animator.GetComponent<Boss>();
        rb = animator.GetComponent<Rigidbody2D>();
        moveDir = Mathf.Sign((bossScript.target.position - animator.transform.position).x);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(animator.transform.position.x > bossScript.minX + spaceFromEdge && animator.transform.position.x < bossScript.maxX - spaceFromEdge)
        {
            animator.SetBool("AtEdge", false);
            rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
        } else
        {
            animator.SetBool("AtEdge", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }
}
