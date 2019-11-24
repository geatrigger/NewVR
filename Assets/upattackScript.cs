using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upattackScript : StateMachineBehaviour
{
    float waitTime = 0.0f;
    float waitTime2 = 0.0f;
    float maxWaitTime = 1.74f;
    bool collision;
    bool check = true;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        check = true;
        collision = false;
        waitTime = 0.0f;
        waitTime2 = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime += Time.deltaTime;
        waitTime2 += Time.deltaTime;
        collision = animator.GetBool("collision");
        
        if (collision == true && check == true)
        {
            check = false;
            waitTime2 = 0.0f;
            animator.SetFloat("speed", 0.0f);
        }
        if (check == false && waitTime2 > 0.2f)
        {
            check = true;

            animator.SetBool("collision", false);
            animator.SetFloat("speed", 1.0f);
            animator.SetTrigger("idle");

        }

        if (waitTime >= maxWaitTime)
        {
            waitTime = 0.0f;
            animator.SetTrigger("idle");

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetBool("attacknow", false);
        animator.SetBool("collision", false);
        animator.SetFloat("speed", 1.0f);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
