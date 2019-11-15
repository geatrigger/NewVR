using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frontguardScript : StateMachineBehaviour
{
    GameObject weaponSystem;
    GameObject Sword1, Sword2;
    float attackDefZ;
    float dirx, diry;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponSystem = GameObject.Find("weaponSystem");
        Hand[] hand = weaponSystem.GetComponents<Hand>();
        Sword1 = hand[0].weaponObject;
        Sword2 = hand[1].weaponObject;
        attackDefZ = 0.7f;
        dirx = 0.65f; diry = 1.5f;
    }


    Vector3 sword1pos;
    Vector3 sword2pos;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sword1pos = Sword1.transform.position;
        sword2pos = Sword2.transform.position; // x,y is screen, z is depth
        float x, y, maxz;
        x = sword2pos.x; y = sword2pos.y;
        maxz = sword2pos.z;
        if (sword1pos.z > sword2pos.z)
        {
            x = sword1pos.x;
            y = sword1pos.y;
            maxz = sword1pos.z;
        }

        //for(int i = 0; i<100000; i++) { }

        if (maxz > attackDefZ)
        {

            animator.SetInteger("attack", 3); // don't attack
            //define guard direction base on x,y,z
            if (x > dirx)
            {
                //right
                animator.SetInteger("guard", 2);
            }
            else if (x < -1.0f * dirx)
            {
                //left
                animator.SetInteger("guard", 1);
            }
            else if (y > diry)
            {
                //up
                animator.SetInteger("guard", 0);
            }
            else
            {
                //front
                //animator.SetInteger("guard", 3);
            }



        }
        else
        {
            //attack
            animator.SetInteger("guard", 4); // don't guard
            animator.SetTrigger("idle");

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
