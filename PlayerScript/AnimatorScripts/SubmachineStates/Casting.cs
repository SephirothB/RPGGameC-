using UnityEngine;
using System.Collections;
public enum enum_casting { CastSpell = 0, StraightMagicShot, BuffA,PermaFrost };
public class Casting : StateMachineBehaviour {
    
    //Script for the moving substate machine.
    // Use this for initialization
    string[] Casting_animations;
    int enter_state;//State when entering the Casting submachine
    int enter_mainstate;//Main submachines State when entering the submachines state
    void Awake()
    {

        Casting_animations = new string[4] { "CastSpell", "StraightMagicShot","BuffA" , "PermaFrost" };//General spell animations

    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        enter_mainstate = animator.gameObject.GetComponent<Player_controller>().mainsubmachinestate;//Update main state to current mainsubmachines state
        enter_state = animator.gameObject.GetComponent<Player_controller>().currentCastinganim;//Update state to current moving state.
        //Within the Moving sub-state machine there are states which are triggered when entering this machine.
        animator.SetTrigger(Casting_animations[enter_state]);//Trigger a Specific animation within the casting submachine state.
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var Player_Controller = animator.gameObject.GetComponent<Player_controller>();
        int currentmainstate = Player_Controller.mainsubmachinestate;//Current main submachines state(In the Main Layer)
        int currentstate = animator.gameObject.GetComponent<Player_controller>().currentCastinganim;//Update state to current moving state.
        string c_castinganimation = Casting_animations[currentstate];//Current casting animation 
        if (currentmainstate != enter_mainstate)//If we are not in the same main submachines state as when we entered exit this submachine.
        {
            string c_mainstate = Player_Controller.mainsubstates[currentmainstate]; //The string of the next main state(current)
            string e_mainstate = Player_Controller.mainsubstates[enter_mainstate];//Enter Main SubState.
            animator.ResetTrigger(Casting_animations[enter_state]);//Reset the Current state
            animator.ResetTrigger(e_mainstate); //Reset the Trigger of enter main state
            animator.SetTrigger(c_mainstate);//Set trigger to the current main state. Current Main State trigger is reset upon exit.
        }
        if (enter_state != currentstate)//Move arround within the submachine state.
        {
            animator.SetTrigger(c_castinganimation);
            animator.ResetTrigger(Casting_animations[enter_state]);//Reset original trigger.
            enter_state = currentstate;//Update Enter_State to the state transtioned to within the moving submachine state.
        }
        else
        {
            //Do Nothing.
        }

    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {//Upon exiting the casting mainsubmachine reset reset the current main state trigger, and casting trigger.
        var Player_Controller = animator.gameObject.GetComponent<Player_controller>();
        int currentmainstate = Player_Controller.mainsubmachinestate;//Current main submachines state(In the Main Layer)
        int currentstate = Player_Controller.currentCastinganim;//Update state to current moving state.
        string c_castinganimation = Casting_animations[currentstate];//Current casting animation 
        string c_mainsubstatemahine = Player_Controller.mainsubstates[currentmainstate];
        animator.ResetTrigger(c_mainsubstatemahine);//Reset the trigger of current main state (Leaving)
        animator.ResetTrigger(c_castinganimation);//Reset the trigger of the current casting state

    }
}
