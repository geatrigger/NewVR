using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : StateMachineBehaviour
{
    float waitTime = 0.0f;
    public float maxWaitTime = 2.0f;
    float randomTime;
    GameObject sword;
    int attackdirection;
    Animator myself;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomTime = (float)Random.Range(1, 3);
        
        attackdirection = (int)Random.Range(0.0f, 2.99f);
        myself = animator;
        //find sword
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime += Time.deltaTime;
        if (waitTime >= randomTime)
        {
            //sword의 위치에 기반해서 반대 방향으로 공격
            //sword가 업스니 대충 랜덤하게따
            waitTime = 0.0f;
            animator.SetInteger("Attack", attackdirection);

        }
    }

    //sword에 추가할 함수들
    void OnCollisionEnter(Collision collision)
    {
        //가드중인데 공격을 받았을때. 이게 맞나?
        
        if (collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Guard"))
        {
            myself.SetInteger("Attack", attackdirection);

        }

    }
    //+검 놓쳤을때 상대 공격 켜주기 SetTrigger("Attack", 0|1|2);



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
