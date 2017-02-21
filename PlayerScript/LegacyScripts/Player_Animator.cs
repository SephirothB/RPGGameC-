using UnityEngine;
using System.Collections;
//Player Controller calls anim(x) and sets which ever animation on, anim(x) ensures that if there is a change in previous and current
//animation states we set transition to true, this triggers the checktransition to go towards a transition state from which it moves towards any state which is 
//set true (current animation). When switching state previous anim ID is set to false so it is turned off.
public enum enum_anim { isRotating = 0, isWalking, isAttacking, isRunning, isCasting, isIdle, isDead, isJumping };
public class Player_Animator : MonoBehaviour {
    Animator thisanim;//Animator attached to this object.
    //Animation Constants, numbered numerically for identification.
    bool rotating;//0
    bool walking;//1
    bool attacking;//2
    bool running;//3
    bool casting;//4
    bool idle;//5
    bool dead;//6
    bool transition;
    public int currentanim;//The enum number of current animation playing
    public int lastanim;
    public string[] animations;
    string currentanimID;//Name of current Animation
    string previousanimID; 
    //
    // Use this for initialization
    void Awake()
    {
        //Animations Array Declaration//
        animations = new string[8] { "isRotating", "isWalking", "isAttacking", "isRunning", "isCasting", "isIdle", "isDead", "isJumping" };
        //End Animations Array Declaration
    }
    void Start () {
        transition = false; 
        thisanim = this.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update () {
        Checktransition();//Set Transition to true or false based on whether the animations are changing or not.
        animator();
        Debug.Log("current Anim: " + animations[currentanim]);
        Debug.Log("Last Anim: " + animations[lastanim]);
    }
    void Checktransition()
    {
        thisanim = this.GetComponent<Animator>();
        if (transition)//Updates value of transition in animator every frame as required.
        {
            
            previousanimID = animations[lastanim];
            //Debug.Log("Setting False:  "); Debug.Log(animations[lastanim]); 
            thisanim.SetBool(previousanimID, false);
            //Debug.Log("Setting Transition True");
            thisanim.SetBool("Transition", true);

        }
        else
        {
            thisanim.SetBool("Transition", false);
        }
    }
    public void anim(int x)//Functions which sets the current animations variables, and also sets transition variable to true if there is a change in animation.
    {
        if (x != currentanim)//Only change animations if current animations is not equal to one asked to set
        {
            //Debug.Log("Transitioning");
            transition = true;
            lastanim = currentanim;//Whenever this function is called set last anim to current.
            currentanim = x; //Current Animation Playing
            
        }
        else
        {

        }
        /* bool rotating;//0
         bool walking;//1
         bool attacking;//2
         bool running;//3
         bool casting;//4
         bool idle;//5
         bool dead;//6*/ 
        /*rotating = false;
        attacking = false;
        running = false;
        casting = false;
        idle = false;
        currentanimID = animations[x];
        this.GetComponent<Animator>().SetBool(currentanimID, true);
        switch (x)
        {
            case 0:
                rotating = true;
                
                break;
            case 1:
                walking = true;
                break;
            case 2:
                attacking = true;
                break;
            case 3:
                running = true;
                break;
            case 4:
                casting = true;
                break;
            case 5:
                idle = true;
                break;
            case 6:
                dead = true;
                break;
            default:
                idle = true;
                break;
        }*/
    }
    void animator()
    {
        
        if (transition)//Only switch state if animations change
        {
            transition = false;//Transtion is now complete.
            //thisanim.SetBool("Transition", false);
            currentanimID = animations[currentanim];
            //string of current animID based on integer given in anim(x), where x corresponds to iterators of the animations array and its enumarator.
            thisanim.SetBool(currentanimID, true);

        }
        else
        {

        }
        /*thisanim.SetBool("isIdle", idle);
        thisanim.SetBool("isWalking", walking);
        thisanim.SetBool("isAttacking", attacking);
        thisanim.SetBool("isCasting", casting);
        thisanim.SetBool("isDead", this.GetComponent<Properties>().dead);*/

    }
}
