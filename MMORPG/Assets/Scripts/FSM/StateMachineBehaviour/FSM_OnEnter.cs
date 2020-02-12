using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FSM_OnEnter : StateMachineBehaviour
{

    public string MethodStr_Enter = "Enter---状态传递的参数名称";

    public string MethodStr_Update = "Update---状态传递的参数名称";

    public string MethodStr_Exit = "Exit---状态传递的参数名称";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (MethodStr_Enter == "Enter---状态传递的参数名称") { return; }
        //animator.SendMessage(MethodStr_Enter);
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (MethodStr_Enter == "Update---状态传递的参数名称") { return; }
        if (stateInfo.normalizedTime >= 0.8f)
        {
            Debug.LogError(">= 0.8f");
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (MethodStr_Enter == "Exit---状态传递的参数名称") { return; }
        GlobalParameters.attack1_Combine = 0;
        animator.SetInteger("Attack1_Combine", 0);
        //animator.SendMessage(MethodStr_Enter);
    }


}
