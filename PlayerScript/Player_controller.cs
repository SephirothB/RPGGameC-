using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System.Linq;

public enum mainsubmachinstates { Moving = 0, Casting, Melee };
//Main Submachines States in baselayer, any changes made here must be updated in the spell initializing array located in this->Awake.
public enum enum_errors { NotFacingTarget = 0, NoTarget, CurrentlyCasting }
public class Player_controller : MonoBehaviour
{//Large Main Class which controls character movement and attacks.

    #region Constants
    public GameObject Target;//Target set by the 
    CapsuleCollider n_collider;//This collider
    Rigidbody n_Rigidbody;//This Rigidbody
    bool Jump;
    bool isgrounded = false;//True when we are airborne.
    float mass = 70;//70Kg
    float jumpforce = 800;//Newton force to apply to player when jumpping
    float g_dist = 1.5f; // Float distance below capsule to be considered grounded.
    Vector3 surf_norm;//Vector normal to surface we are standing on in world space
    GameObject Objecthit;//GameObject hit by ray;
    Vector3 capsulecentre;//Centre of our capsule(Vector in world space)
    float capsulheight;//Height of our capsule
    float halfc_height;
    float gravitymulti = 5f;//Gravity Force Multiplier
    #endregion
    #region Animator Constants/ Initializations
    //Animator Controlling Variables
    public string[] mainsubstates;
    //SUb StateMachines in the base Layer.
    //Animations for BaseLayer, ** Update mainsubstates array if there are any changes made ** 
    public int mainsubmachinestate;//Current Main Submachine State. 
    Animator thisanimator;
    bool Move;
    public int currentMovinganim;//Sets the State to trigger within the Moving Substate Machine.
    public int currentCastinganim;//Sets the State of trigger within the Casting SubState Machine.
    #endregion

    #region State Constants
    string[] attackkeys;//Keys which are used to attack/cast
    public bool attacking;//We are aggro

    //Following two bools are set false or true in mellee and general spell classes which handle combat
    public bool b_casting;// currently casting a spell.
    public bool b_melee;// true if Currently in mellee combat.
                        //

    string[] meleekeys;
    string attackkey;//Currently pressed attack key updated whenever an attack key is pressed.
    #endregion


    void Awake()
    {
        Rigidbody n_Rigidbody = GetComponent<Rigidbody>();
        n_Rigidbody.mass = mass;
        n_Rigidbody.drag = 1;
        CapsuleCollider n_collider = this.GetComponent<CapsuleCollider>();
        capsulecentre = n_collider.center;
        capsulheight = n_collider.height;
        halfc_height = capsulheight / 2;
        n_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //Animator Initializers
        thisanimator = this.GetComponent<Animator>();
        mainsubstates = new string[3] { "Moving", "Casting", "Melee" };//Submachines states in Base Layer
        //
        //State Initializers 
        attacking = b_casting = b_melee = false;//Initially not attacking, of any type
        attackkeys = new string[4] { "q", "e", "r", "f" };//Keys Responsible for attacking

        meleekeys = new string[1] { "f" };//melee Keys
        //

    }
    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<Properties>().dead)
        {//Only active when character is not dead.
            checkgrounded(); //Check whether the player is grounded or airborne.
            setstate();//A function which chooses actions based on what state the character currently is in 
        }
    }

    //A function which takes in user input and is responsible for moving the character in plane
    void rotate()
    {
        // read inputs
        float omega = this.GetComponent<Properties>().omega;
        float h = CrossPlatformInputManager.GetAxis("Horizontal");//Horizontal tied to a,d (-1 to 1)
        float v = CrossPlatformInputManager.GetAxis("Vertical");//Vertical tied to w,s (-1 to 1)  
        Vector3 m_to = this.transform.right * h + this.transform.forward * v;
        float angle_move = (Vector3.Angle(this.transform.forward, m_to)) * h;
        //Move to angle provides an angle between current forwards vector and the move towards vector. Move towards vector is the summation of forwards and 
        //horizontal directions toward which the character walks.
        //Debug.Log("Move to Vector"+m_to); Debug.Log("Move to angle" + angle_move);
        transform.Rotate(0, angle_move * omega * Time.deltaTime, 0);//Rotate the target along its own upwards axis, frame by frame.
    }
    void move()//Function handles movement of character if it is grounded
    {
        n_Rigidbody = GetComponent<Rigidbody>();
        if (isgrounded)//Handles functions which can only be done while grounded. 
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {//If the target is jumping handle it in a different function
                jump();
            }
            else
            {
                rotate();//A function responsible for rotating the target towards the direction the players wants to move in
                translate();//A function responsible for setting teh velocity of the object along its forwards direction.
            }
        }
        else
        {
            airmovement();
        }
    }
    void translate()//Transalate/Rotates the Character
    {
        n_Rigidbody = this.GetComponent<Rigidbody>();

        float velocity = this.GetComponent<Properties>().speed;
        float h = (CrossPlatformInputManager.GetAxis("Horizontal"));
        //Horizontal tied to a,d (-1 to 1).
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        //Vertical tied to w,s (-1 to 1).
        if (v > 0)
        {//Handles Forward Movement
            v = v / 2; //Divide forward speed by half, run faster if left shift is pressed.
            if (Input.GetKey(KeyCode.LeftShift))
            {//Running, speed is doubled. 
                Debug.Log("Running");
                v *= 2f;
            }
            this.GetComponent<Animator>().SetFloat("Forward", v);//Sets the run animation blend ratio
        }
        else if (v < 0)
        {//Handles backwards movement
            this.GetComponent<Animator>().SetFloat("Forward", v);//Sets the run animation blend ratio
        }
        else
        {//Not Moving
            this.GetComponent<Animator>().SetFloat("Forward", v);//Sets the run animation blend ratio
        }
        this.GetComponent<Animator>().SetFloat("Turn", h);//Sets the Turning animation blend ratio
        //Vertical tied to w,s (-1 to 1) this.transform.Translate(Vector3.forward * v * Time.deltaTime * velocity, Space.Self); 
        n_Rigidbody.velocity = v * velocity * this.transform.forward;
        //Give the player a certian velocity based on properties translate the target along its current forward direction based on input from v.
        mainsubmachinestate = (int)mainsubmachinstates.Moving;
        currentMovinganim = (int)enum_moving.Walking;//Walking/Running/Rotating/Idle are handled in the same blend tree(Walking)
    }
    void jump()//Responsible for the Jump of a Character
    {
        //Debug.Log("Jumping");
        n_Rigidbody = GetComponent<Rigidbody>();
        n_Rigidbody.AddForce((this.transform.up) * jumpforce, ForceMode.Impulse);
    }
    void checkgrounded()//Checks whether the Object is Grounded
    {
        CapsuleCollider thiscollider = GetComponent<CapsuleCollider>();
        RaycastHit info;
        Vector3 capsulecentre = this.GetComponent<CapsuleCollider>().center;//Centre of the capsule along y-axis, with y-axis = capsule height
        capsulheight = thiscollider.height;//Sphere collider;
        capsulecentre = transform.TransformPoint(capsulecentre);//Local to world space
        isgrounded = Physics.Raycast(capsulecentre, Vector3.down, out info, capsulheight + g_dist, Physics.AllLayers);
        //-1 * this.transform.up
        //Debug.Log("isgrounded: " + isgrounded); Debug.Log("Hit:" + info.collider); Debug.Log("capsule centre: " + capsulecentre);
        //A ray is cast from the top of thise object down wards till a distance of ( collider heigt + g_dist) to check whether we are grounded (if hit = true)
        //info stores the info of the collider hit. 

        if (info.collider != null)//If we are grounded on a gameobject
        {
            Objecthit = info.collider.gameObject;
            surf_norm = info.normal;
        }
        else { };
    }
    void airmovement()//If we are not grounded 
    {
        mainsubmachinestate = (int)mainsubmachinstates.Moving;
        //Main Submachine state is moving This is used to transition to a new main submachine state after exiting one.
        currentMovinganim = (int)enum_moving.Jumping;
        //Withing the Moving Substate go into Jumping.
        n_Rigidbody = GetComponent<Rigidbody>();
        Vector3 gravity = Physics.gravity * gravitymulti - Physics.gravity;
        //Additional gravity force.
        n_Rigidbody.AddForce(gravity);
    }
    void setstate()//Sets the current state of the character, whether we are attacking or just moving.
    {
        b_melee = false;//Currently mellee class is not set up;
        b_casting = this.GetComponent<GeneralSpell>().b_casting;//If we are currently casting this will be set true within GeneralSpell Class.
        if (!b_casting && !b_melee)
        {//If attacking is set true we cannot currently move;
            move();//Let the motion function handle if we are not attacking, or casting
        }
        else//Case when we are currently attacking check, approporate attack coroutine cancels attacks based on conditions here.
        {
            //Debug.Log("Dont Do Any Thing Currently Attacking");
        }
    }


}
