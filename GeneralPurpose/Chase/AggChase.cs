using UnityEngine;
using System.Collections;
using System;

public class AggChase : MonoBehaviour
{//Chasing and attacking of a melle NPC. The NPC will attack the nearest target till its dead, and it will
    //Prioritise Player
    #region GeneralInitializers
    GameObject[] arr_GO;//Initializers of gameobjects used by the check enemies function.
    public struct objval
    {
        public GameObject obj;
        public float dist;
    }//A struct which stores game objects within a viscinty of this character and the manitude of there distances to this char.
    float defaultspeed; //Animation default speed.
    int i;//Common iterator value
    Vector3 direction;
    GameObject target; //Game Object being attacked
    static Animator anim;//Our Animator.
    Vector3 forward = new Vector3(0, 0, 1);//General Purpose forward vector
    public GameObject lastenemy;
    //Game object which stores which player last attacked this character, primarily used to set the target as player if they attack from outside the sight
    //radius of this NPC.

    #region Animator/State Constants
    public bool animattacking = false;//Bool to check if we are attacking
    public bool animidle;
    public bool animwalking;
    public bool damaged;//Damaged called by player spell abilities/and set by them.
    public bool immobilized;//When Immobilized Target cannot move
    public bool stunned;//When Stunned Target cannot move or attack
    bool attacking; //Currently attacking
    #endregion
    CapsuleCollider thiscollider;//Our collider
    objval[] attackablecollider;//Initialize Attackable Colliders Struct Array 
    public IEnumerator attackcoroutine;//Attack coroutine.

    #endregion


    void Awake()
    {
        thiscollider = this.GetComponent<CapsuleCollider>();
        anim = this.GetComponent<Animator>();
        damaged = false;
        immobilized = false;//Initially free to move;
    }
    void Start()
    {
        defaultspeed = this.GetComponent<Properties>().animspeed;//Default animation speed initializer.
        target = prioritytarget(checkenemies());//Initialize target of this object
        anim = GetComponent<Animator>();

        // Debug.Log("Start "); Debug.Log(thiscollider);
    }
    void Update()
    {
        checkstate();
    }
    void checkstate()
    {
        if (!this.GetComponent<Properties>().dead)
        {
            if (!immobilized && !stunned)
            {
                if (lastenemy != null && lastenemy.GetComponent<Properties>().currentHP >= 0f)
                {//If the Last enemy is still allive go after it.
                    if (lastenemy.name == "Player")
                    {
                        target = lastenemy;//Primarily used when attacked by player. Damaging Player spells will set last enemy to Player. 
                        movetotarg(target);//Move towards attacking player 
                    }
                    else
                    {
                        target = prioritytarget(checkenemies());//Target is closest attackable object or player.
                        movetotarg(target);//Move towards the closest attackable target 
                    }
                }
                else
                {//Last enemy is null or dead find new target.
                    target = prioritytarget(checkenemies());//Target is closest attackable object or player.
                    movetotarg(target);//Move towards the closest attackable target 
                }

            }
            else//If immobilized set state to idle and do nothing, attacking, moving etc.
            {
                setanimstatefalse();
                animidle = true;                
            }
        }
        else
        {
            //ToDo when Dead, Target is set to dead from the death function
            setanimstatefalse();
        }
    }


    //Function Used for melee attack of an NPC. Structured to attack within a given time() 
    //This Class is responsible for damaging enemy, updating enemy health, setting attack animation
    IEnumerator attack(GameObject target)
    {

        float time = this.GetComponent<Properties>().attacktime;//Time to finish an attack. Set to one for general.
        float multiplier = GetComponent<Properties>().aspeed_m; //Attack speed multiplier.
        float damage = GetComponent<Properties>().attackdmg;//Our Current Attack damage. 

        yield return new WaitForSeconds(time * multiplier);//Damage the target after a set ammount of time which is equal to attack time/multiplier 

        if (this.gameObject != null)
        {//If this game object dies during attacking dont excecute the damage coroutine.
            if (!this.GetComponent<Properties>().dead)
            {
                if (target.name == "Player")
                {
                    target.GetComponent<PlayerHealth>().damage(damage, this.gameObject);
                    attacking = false;
                }
                else
                //Damage the NPC.
                {
                    float currentHP = target.GetComponent<Properties>().currentHP;//Current HP of target
                    target.GetComponent<Properties>().currentHP = currentHP - damage;//Damage target
                    currentHP = target.GetComponent<Properties>().currentHP;//Current HP of target
                    if (currentHP <= 0)
                    {
                        target.GetComponent<Properties>().currentHP = 0;
                        target.GetComponent<Properties>().lastkilledby = this.gameObject;
                        target.GetComponent<Properties>().deadcount++;
                        target.GetComponent<Properties>().dead = true;//Char is dead
                        target.GetComponent<Properties>().attackable = false;//Char is not attackable anymore
                        target.GetComponent<DeadCheck>().enabled = true;
                    }
                    attacking = false;
                }
            }
            else
            {
                attacking = false;
            }
        }

    }

    //If the NPC collides with current target start attacking.
    void OnTriggerStay(Collider col)
    {
        if (target != null)//If there are no targets we are not attacking return
        {
            if (col.gameObject.name != target.name)
            {
                return;
            }//If the object is not the primary target ignore it
             //object is untagged or it is unattackable return nothing from this funtion(break out);
            else if (!attacking && !target.GetComponent<Properties>().dead && !stunned)
            {//If not attacking and colliding with alive target and not stunned start attacking
                //Debug.Log("Attacking Collision");
                attacking = true;
                attackcoroutine = attack(target);
                StartCoroutine(attackcoroutine);
            }
            else { return; }//If nothing hits do nothing.
        }
        else { return; }


    }

    //If the current target(the one being attacked) leaves this NPC's collider cancel the attack coroutine.
    void OnTriggerExit(Collider col)
    {
        if (target != null && attackcoroutine != null)
        {
            if (col.gameObject.name != target.name) { return; }//If some other object leaved collider do nothing.
            else if (col.gameObject.name == target.name && attacking)
            {
                StopCoroutine(attackcoroutine);
                attacking = false;// Debug.Log("stop Attacking");
            }//If the target leaves collider and the attack coroutine has not completed (attacking -> true) set attacking false and stop the 
            //attacking co-routine.

            //Debug.Log("stop Attacking");
        }
        else { return; }//if target is null and something leaves collider do nothing.
    }

    //Checks for enemies in a radius of (collider radius * sightrange) and returns an array of alive attackable objects and there distances to this char.
    objval[] checkenemies()

    {
        int j = 0;
        //Vector3 pos = this.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, thiscollider.radius * this.GetComponent<Properties>().sightrange);
        //Search objects with colliders within our sightrange.

        attackablecollider = new objval[hitColliders.Length];//Initialize the attackable collider array to the ammount of colliders hit.

        if (hitColliders.Length != 0)
        {

            i = 0;
            if (hitColliders[i] == null) { return null; }//If first collider is null that means no colliders were hit.

            while (i < hitColliders.Length)
            {

                if (hitColliders[i].gameObject.tag == "Terrain" || hitColliders[i].gameObject.name == this.name || hitColliders[i].gameObject.tag == this.tag)
                //GameObject is self, or terrain or its tag(type) is self. Only Further Check NPC's or Player.
                {
                    i++;//Do Nothing,and increment through this object 
                }
                else if (hitColliders[i].gameObject.GetComponent<Properties>().attackable && !hitColliders[i].gameObject.GetComponent<Properties>().dead)//Aggressive will attack anything it can
                {

                    attackablecollider[j].obj = hitColliders[i].gameObject;//An attackable collider.                                                                            
                    float dist = Vector3.SqrMagnitude(hitColliders[i].gameObject.transform.position - this.transform.position);//Magnitude of distance from 
                    attackablecollider[j].dist = dist;
                    i++;//Iterate The collider array
                    j++;//Update The attackable collider array

                }
                else
                {
                    i++;
                }//Current Collider is  tagged but Not attackable/or dead Skip It

            }
            return attackablecollider;//Return the attackable collider arrays.

        }

        else
        {
            return null;
        };//If no Colliders present attackable enemy array is a null.

    }

    //Takes in struct array of alive,attackable objects in our viscinity and there distances. Returning the 
    //closest attackable object/or player(Highest Priority)
    GameObject prioritytarget(objval[] arr)
    {
        i = 0;//iterator

        if (arr[i].obj != null)//If the first object is a null than there are no attackable alive objects in our viscinity.
        {
            objval h_priority = arr[i];//Instantiate the highest priority target to to first. First Object in

            while (i < arr.Length)
            {
                if (arr[i].obj != null)
                {

                    if (arr[i].obj.name == "Player")//Player highest priority target unless dead
                    {
                        target = arr[i].obj;
                        return target;
                    }
                    else if (arr[i].dist < h_priority.dist)// Else Choose target closest object.
                    {
                        h_priority = arr[i];//Update priority to the object closest to our viscinity, if its not dead.
                        i++;
                    }
                    else { i++; }; //This Object is not closer, nor is it player iterate through it
                }
                else { i++; }//Iterate through null objects
            }
            target = h_priority.obj;//If no player in viscinity attack nearest attackable target.
            return target;
        }
        else { return null; }
    }

    //Move to the target provided by the check enemies function.
    void movetotarg(GameObject target)
    { 
        if (target != null && !damaged)//If target is null idle, and not damaged. Damaged animation is handled seperately.
        {
            direction = target.transform.position - this.transform.position;//Vector from target to NPC
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(direction), this.GetComponent<Properties>().omega * Time.deltaTime);
            //Transform the rotation of the NPC this script is attateched to towards the player using linear interpolation.
            //transform.rotation is rotation relative to world axi. Quaternion.LookRotation returns the rotation using provided vector which will 
            //act to rotate the forward axis (z) of the NPC to face the vector provided. Debug.Log(Vector3.SqrMagnitude(direction));
            if (!attacking)//Move towards target if its defined and we are not attacking.
            {//Start moving towards target. 
                setanimstatefalse();
                animwalking = true;
                this.transform.Translate(forward * Time.deltaTime * this.GetComponent<Properties>().speed);
                // Debug.Log("Moving To Player"); 

            }
            else if (attacking) //If we are currently in an attack state
            {
                setanimstatefalse();
                animattacking = true;//Bool to check if we are attacking              
                //Do nothing here, controll is in the attack function.
            }
            else//Default Idle State
            {
                // Debug.Log("Setting IDLE");
                setanimstatefalse();
                animidle = true;
            }

        }
        else
        {
            //Debug.Log("Setting IDLE");
            setanimstatefalse();
            animidle = true;
        }
    }

    void setanimstatefalse()
    {
        animattacking = false;//Bool to check if we are attacking
        animidle = false;
        animwalking = false; 
    }
}

