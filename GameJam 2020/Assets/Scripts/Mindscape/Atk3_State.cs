using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3_State : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Combo_Attempt", false);
        animator.GetComponent<Anim_Event>().can_combo = false;
        animator.GetComponentInParent<Player_Controller>().is_attacking = true;
        animator.GetComponent<Anim_Event>().weapon_damage = 1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Anim_Event>().can_combo && animator.GetBool("Combo_Attempt"))
        {
            Debug.Log("ATK3");
            animator.SetBool("Attack3", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Anim_Event>().can_combo = false;
        animator.SetBool("Combo_Attempt", false);
        if (animator.GetBool("Attack3"))
        {

        }
        else
        {
            animator.GetComponentInParent<Player_Controller>().is_attacking = false;
        }
        animator.SetBool("Attack3", false);
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
