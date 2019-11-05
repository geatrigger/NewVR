using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAttactScript : StateMachineBehaviour
{
    float waitTime = 0.0f;
    public float maxWaitTime = 1.05f;
    float fake;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fake = Random.Range(0.0f, 1.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime += Time.deltaTime;
        if(fake < 0.2f)
        {
            waitTime = 0.0f;
            animator.SetInteger("Attack", 2);
        }



        if (waitTime >= maxWaitTime)
        {
            
            waitTime = 0.0f;
            animator.SetTrigger("Guard");
            

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
