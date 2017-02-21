using UnityEngine;
using System.Collections;

public class CreatureChase : MonoBehaviour {

    //Chasing and attacking of a melle NPC. The NPC will attack the nearest target till its dead, and it will
    //Prioritise Player
    #region GeneralInitializers
    GameObject[] arr_GO;
    public struct objval
    {
        public GameObject obj;
        public float dist;
    }
    float defaultspeed; //Animation default speed.
    int i;//Common iterator value
    Vector3 direction;
    GameObject target; //Game Object being attacked
    static Animator anim;//Our Animator.
    Vector3 forward = new Vector3(0, 0, 1);//General Purpose forward vector
    public bool attacking = false;//Bool to check if we are attacking
    public bool idle;
    public bool walking;
    CapsuleCollider thiscollider;//Our collider
    objval[] attackablecollider;//Initialize Attackable Colliders Struct Array 
    private IEnumerator attackcoroutine;//Attack coroutine.

    #endregion


    void Awake() { thiscollider = this.GetComponent<CapsuleCollider>(); anim = this.GetComponent<Animator>(); }
    void Start()
    {
        defaultspeed = this.GetComponent<Properties>().animspeed;//Default animation speed initializer.
        target = prioritytarget(checkenemies());//Initialize target of this object
        anim = GetComponent<Animator>();

        // Debug.Log("Start "); Debug.Log(thiscollider);
    }
    IEnumerator attack(GameObject target)
    {//Function Used for melee attack of an NPC. Structured to attack within a given time() 
     //This Class is responsible for damaging enemy, updating enemy health, setting attack animation
        float smoother = 2f;
        float time = GeneralProperties.S_time;//Time to finish an attack.
        float multiplier = GetComponent<Properties>().aspeed_m; //Attack speed multiplier.
        float damage = GetComponent<Properties>().attackdmg;//Our Current Attack damage.  
        yield return new WaitForSeconds(time / multiplier * smoother);//Damage the target after a set ammount of time which is equal to attack time/multiplier
        float currentHP = target.GetComponent<Properties>().currentHP;//Current HP of target
        target.GetComponent<Properties>().currentHP = currentHP - damage;//Damage target
        currentHP = target.GetComponent<Properties>().currentHP;//Current HP of target
        if (currentHP <= 0) { target.GetComponent<Properties>().currentHP = 0; StartCoroutine(Dead(target)); }
        attacking = false;
    }
    void OnTriggerStay(Collider col)
    {
        if (target != null)//If there are no targets we are not attacking return
        {
            //Debug.Log("Attacking Collision"); Debug.Log(col.gameObject.tag);|| col.gameObject.GetComponent<Properties>().attackable
            if (col.gameObject.name != target.name)
            {
                return;
            }//If the object is not the primary target ignore it
             //object is untagged or it is unattackable return nothing from this funtion(break out);
            else if (!attacking && !target.GetComponent<Properties>().dead)
            {//If not attacking and colliding with alive target start attacking
             //    Debug.Log("Attacking Collision");
                attacking = true;
                attackcoroutine = attack(target);
                StartCoroutine(attackcoroutine);
            }
            else { return; }//If nothing hits do nothing.
        }
        else { return; }


    }
    void OnTriggerExit(Collider col)//other Collider Left this.collider
    {
        if (target != null && attackcoroutine != null)
        {
            if (col.gameObject.name != target.name) { return; }//If some other object leaved collider do nothing.
            else if (col.gameObject.name == target.name && attacking)
            {
                StopCoroutine(attackcoroutine); attacking = false;// Debug.Log("stop Attacking");
            }//If the target leaves collider and the attack coroutine has not completed (attacking -> true) set attacking false and stop the 
            //attacking co-routine.

            //Debug.Log("stop Attacking");
        }
        else { return; }//if target is null and something leaves collider do nothing.
    }
    objval[] checkenemies()//Checks for enemies in a radius and returns an array of alive attackable objects and there distances to this char.
    {
        int j = 0;
        //Vector3 pos = this.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, thiscollider.radius * this.GetComponent<Properties>().sightrange); //Search objects with colliders within


        attackablecollider = new objval[hitColliders.Length];//Initialize the attackable collider array
        if (hitColliders.Length != 0)
        {

            i = 0; //
            if (hitColliders[i] == null) { return null; }

            while (i < hitColliders.Length)
            {
                //Initialize attackablecollider to length of colliders.(hitColliders[i].gameObject.tag == "Terrain") ||
                if (hitColliders[i].gameObject.tag == "Terrain" || hitColliders[i].gameObject.name == this.name || hitColliders[i].gameObject.tag == this.tag)//GameObject is self, or terrain 
                                                                                                                //ignore both. Only Further Check NPC's or Player.
                {
                    // Debug.Log("Do nothing " + i);
                    i++;//Do Nothing,and increment through this object 
                }
                else if (hitColliders[i].gameObject.GetComponent<Properties>().attackable && !hitColliders[i].gameObject.GetComponent<Properties>().dead)//Aggressive will attack anything it can
                {

                    attackablecollider[j].obj = hitColliders[i].gameObject;//An attackable collider.                                                                            
                    float l = Vector3.SqrMagnitude(hitColliders[i].gameObject.transform.position - this.transform.position);//Magnitude of distance from 
                    attackablecollider[j].dist = l;
                    i++;//Iterate The collider array
                    j++;//Update The attackable collider array

                }
                else { i++; }//Current Collider is  tagged and Not attackable Skip It

            } 
            return attackablecollider;

        }

        else { return null; };//If no Colliders present attackble enemy array is a null.

    }
    // Update is called once per frame
    GameObject prioritytarget(objval[] arr)//Takes in struct array of alive,attackable objects in our viscinity and there distances. Returning the 
                                           //closest attackable object/or player(Highest Priority)
    {
        //i = 0;
        //while (i < arr.Length) { Debug.Log(arr[i].obj); i++; };
        i = 0;//iterator
              // GameObject target;
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
    void movetotarg(GameObject target)
    {
        if (target != null)//If target is null idle.
        {
            direction = target.transform.position - this.transform.position;//Vector from target to NPC
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(direction), this.GetComponent<Properties>().omega * Time.deltaTime);//Transform the rotation of the
                                                                                                        //NPC this script is attateched to towards the player using linear interpolation. transform.rotation is rotation relative to world axis
                                                                                                        //Quaternion.LookRotation returns the rotation using provided vector which will act to rotate the forward axis (z) of the NPC to face the vector provided.
                                                                                                        // Debug.Log(Vector3.SqrMagnitude(direction));
            if (!attacking)//Move towards target if its defined and we are not attacking.
            {//Start moving towards target. 
                attacking = false;//Bool to check if we are attacking
                idle = false;
                walking = true;
                this.transform.Translate(forward * Time.deltaTime * this.GetComponent<Properties>().speed);
                // Debug.Log("Moving To Player"); 

            }
            else if (attacking) //Vector3.SqrMagnitude(direction) < attackdist
            {
                attacking = true;//Bool to check if we are attacking
                idle = false;
                walking = false;
                //Do nothing here, controll is in the attack function.
            }
            else//Default Idle State
            {
                // Debug.Log("Setting IDLE");
                attacking = false;//Bool to check if we are attacking
                idle = true;
                walking = false;
            }

        }
        else
        {
            //Debug.Log("Setting IDLE");
            attacking = false;//Bool to check if we are attacking
            idle = true;
            walking = false;
            //anim.speed = defaultspeed;
            //anim.SetBool("isWalking", false);
            //anim.SetBool("isAttacking", false);
            //anim.SetBool("isIdle", true);
            //Debug.Log("Idle");
        }
    }//Move to the target provided.
    IEnumerator Dead(GameObject dyingchar)
    {
        if (dyingchar != null)
        {//Set the character state to dead
            dyingchar.GetComponent<Properties>().dead = true;//Char is dead
            dyingchar.GetComponent<Properties>().attackable = false;//Char is not attackable anymore
            Debug.Log(dyingchar.name);
        }
        yield return new WaitForSeconds(GeneralProperties.deathtime);
        if (dyingchar.tag != "Player" && (dyingchar != null)) { Destroy(dyingchar); }//Ensures that when the character has been killed by two different
        //objects at the same time the first and the first of the two destroys the character we dont reference it again.
        else { };//Player Deaths are handled differently.
    }
    void Update()
    {
        if (!this.GetComponent<Properties>().dead)
        {
            //Debug.Log("Alive Skeletons " + this.name);
            target = prioritytarget(checkenemies());//Target is closest attackable object or player.
            movetotarg(target);//Move towards the closest attackable target
                               //if (target != null) { Debug.Log("Current Target" + target.name); } else { Debug.Log("Target is NULL "); };
        }
        else
        {
            //ToDo when Dead, Target is set to dead from the death function
        }
    }
}
