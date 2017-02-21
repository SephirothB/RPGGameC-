using UnityEngine;
using System.Collections;

public class Defaul_Idle :StateMachineBehaviour {

    //public override void OnStateUpdate(Animator animator, stateInfo AnimatorStateInfo, int layerIndex)
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
            var player_controller = animator.gameObject.GetComponent<Player_controller>();
            int current_mainstate = player_controller.mainsubmachinestate;
            string trigger = player_controller.mainsubstates[current_mainstate];
            animator.SetTrigger(trigger);
        
    }
    //Transtions from default idle layer to current mainstate.
   
    
}
