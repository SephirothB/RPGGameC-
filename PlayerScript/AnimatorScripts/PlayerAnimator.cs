using UnityEngine;
using System.Collections;

public class PlayerAnimator : StateMachineBehaviour{ 
    Animator thisanimator;
    public string[] main_animations;//Substate Machines within main layer.
    public int movecurrentanim;
    //public enum enum_main { Rotating = 0, Walking, Attacking, Running, Casting, Idle, Dead, Jumping };
    static int Move;
    static int walk;
    void Awake()
    {
        
        main_animations = new string[8] { "Rotating", "Walking", "Attacking", "Running", "Casting", "Idle", "Dead", "Jumping" };
        //thisanimator = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
           Move = Animator.StringToHash("Move");
            walk = Animator.StringToHash("Walking");

    }
	
	// Update is called once per frame
	void Update () {
       // SetAnimator(currentanim);
	}
    /*override public void OnStateEnter( Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
        //animator.SetTrigger("Jump");
        //Debug.Log(;
        /*Debug.Log(animatorStateInfo.fullPathHash);
        
        Debug.Log(layerIndex);
        Debug.Log("State Enter");
    }*/
    
   /* public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)//Entering th Moving State Machine
    {
        animator.SetTrigger(animations[movecurrentanim]);//Set By The Player controller function.
    }
    void SetAnimator(int anim)
    {
        
        //animator.SetTrigger(animations[anim]);
    }*/
}
