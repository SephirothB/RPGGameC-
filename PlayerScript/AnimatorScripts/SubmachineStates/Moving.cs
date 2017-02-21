using UnityEngine;
using System.Collections;
public enum enum_moving {  Walking = 0, Jumping };
public class Moving : StateMachineBehaviour {
    //Script for the moving substate machine.
    // Use this for initialization
    string[] moving_animations;
    int enter_substate;//subState when entering the moving submachine
    int enter_mainstate;//main-submachine State(In main Layer) when entering the moving submachines state
    void Awake()
    {
        
        moving_animations = new string[2] { "Walking", "Jumping" };
        
    }
	 
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        enter_mainstate = animator.gameObject.GetComponent<Player_controller>().mainsubmachinestate;//Update main state to current mainsubmachines state
        enter_substate = animator.gameObject.GetComponent<Player_controller>().currentMovinganim;//Update state to current moving state.
        //Within the Moving sub-state machine there are states which are triggered when entering this machine.
        animator.SetTrigger(moving_animations[enter_substate]);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        var Player_Controller = animator.gameObject.GetComponent<Player_controller>();
        int currentmainstate = Player_Controller.mainsubmachinestate;//Current main submachine state
        int currentstate = animator.gameObject.GetComponent<Player_controller>().currentMovinganim;//Update state to current moving state.
        string c_movinganimation = moving_animations[currentstate];//Current Trigget
        //Debug.Log("Moving State: " + currentmainstate);
        if (currentmainstate != enter_mainstate)//If we are not in the same main submachines state as when we entered exit this submachine.
        { 
            string c_mainstate = Player_Controller.mainsubstates[currentmainstate]; //The string of the next main state(current)
            animator.ResetTrigger(Player_Controller.mainsubstates[enter_mainstate]);//Reset Main Sub Machine State
            animator.ResetTrigger(moving_animations[enter_substate]);//Reset Current State Within Attacking
            animator.SetTrigger(c_mainstate);//Set trigger to the current main state which will cause us to leave this one.
            //Debug.Log("State Machine Exit : Moving");
        }if (enter_substate != currentstate)//Move arround within the submachine state.
        { 
            animator.SetTrigger(c_movinganimation);
            animator.ResetTrigger(moving_animations[enter_substate]);//Reset original trigger.
            enter_substate = currentstate;//Update Enter_State to the state transtioned to within the moving submachine state.
        }else
        {
            //Do Nothing.
        }

    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Exiting Moving State Machine");
        //Upon exiting the current mainsubmachine reset the current main state trigger, and current submachine trigger.
        var Player_Controller = animator.gameObject.GetComponent<Player_controller>();
        int currentmainstate = Player_Controller.mainsubmachinestate;//Current main submachines state(In the Main Layer)
        int currentstate = Player_Controller.currentMovinganim;//Update state to current moving state.
        string c_movinganimation = moving_animations[currentstate];//Current submachine animation 
        string c_mainsubstatemahine = Player_Controller.mainsubstates[currentmainstate];
        animator.ResetTrigger(c_mainsubstatemahine);//Reset the trigger of current main state (Leaving)
        animator.ResetTrigger(c_movinganimation);//Reset the trigger of the current casting state
        Debug.Log("Reseting submachine(walk)"  +c_movinganimation);
    }
}
