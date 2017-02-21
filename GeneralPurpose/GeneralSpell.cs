using UnityEngine;
using System.Collections;
public enum enum_AoESpells { PermaFrost = 0 }//Enum which stores the aoe spells, ANYCHANGES made here must be updated within aoespellarray
public enum enum_spellerrors { YouMustFaceTheTarget = 0, NoTarget, CurrentlyCasting, SpellOnCooldown };
//Enum which stores the errors which could occure while casting, ANYCHANGES made here must be updated within spell errors string.

enum enum_spell { fireball = 0, frostbolt, PermaFrost };
//Use to recognize which spell to cast within the switch statement, any changes made here must be updated in the 
//spell initializing array(spell Array) located in Awake.


/// <summary>
/// A Class which creates different types of spells and stores there information, cast locations, and GameObjects. 
/// The class is also responsible for instantiating spells, casting, and displaying castbars/errors.
/// </summary>
public class GeneralSpell : MonoBehaviour
{
    #region SpellInformation and Initializers
    public Transform CastLocationFireBall;//Transforms of cast locations of spells, set in unity.
    public Transform CastLocationFrostBolt;
    public Transform CastLocationPermaFrost;
    public GameObject fireballanim;//Cast animation objects
    public GameObject frostboltanim;
    public GameObject PermaFrostGameObject;

    ///General information of various spells, speed, omega, base damage, castime etc.
    public struct spells
    {//Struct which holds properties of spells(initialized at awake)
        public string Name;//Name of this spell
        public float casttime;//The cast  time of the spell
        public int castanim;//The cast animation location of the spell
        public GameObject spell;//The GameObject of the spell
        public Transform s_location;//Location to instantiate the spell
        public float b_damage;//base Damage of spell
        public float APratio;//Ratio of AP added to base damage of spell
        public float cooldown;//Cooldown of this spell
        public float initCD;//The Time when CD was started.
        public bool available;//Spell is not availabe until set true after cool down 
        public spells(string name,float castime, int anim, GameObject spellobject, Transform location, float dmg, float r_AP, float cd,float intcd, bool avail)
        {
            Name = name;
            casttime = castime;
            spell = spellobject;
            castanim = anim;
            s_location = location;
            b_damage = dmg;
            APratio = r_AP;
            cooldown = cd;
            available = avail;
            initCD = intcd;
        }
    }
    public static spells[] spellarray = new spells[3];//Array of spells, the array index corresponds to the enum_spell
    //

    //Struct for AoE spells, contains specialized data which pretains only to AoE spells and not single target. The rest of the properties of the
    //general properties like cast time are located in the spell array.
    public struct AoEspell
    {
        public float CD;//Cool down of Spell.
        public float radius;//Radius of effect from origin
        public float speed;//The rate at which the spell expands from origin.
        public AoEspell(float cooldown, float radius_, float speed_)
        {
            CD = cooldown;
            radius = radius_;
            speed = speed_;
        }
    }
    public static AoEspell[] aoespellarray = new AoEspell[1];

    public struct spellinfo
    {//Stores the movement characteristics of spells.
        public int speed;//Velocity
        public int omega;//Rotation speed
        public spellinfo(int vel, int rot)
        {
            speed = vel;
            omega = rot;
        }
    }//Store the dynamics characteristic of spells
    public static spellinfo[] spellarr;
    public static spellinfo t_spell = new spellinfo(20, 3);//General speed of targeted spells.
    public static float f_time = 3.0f;//General freeze duration in seconds

    #endregion

    #region SpellCastFunctions
    IEnumerator cast_permafrost;//Used for starting the perma frost spell cast routine.
    IEnumerator casttargspell;//Used for starting the targetted spell coroutine.
    #endregion

    #region SpellCastKeysInitializers
    string[] castkeys;//Array which stores the keys binded to spells
    string castkey; //Stores the current key pressed
    #endregion

    #region CurrentTarget and State Variables
    int spell_casting; //The Current spell being cast, its integer location within the spell array function.
    GameObject Target;//Target of spells, obtained from the player controller.
    float s_starttime;// current Spell cast time.
    public bool b_casting;//Boolean to check if we are currently casting.
    #endregion

    #region Spell Cast Bar and error locations.
    public Vector2 castbarsize;
    public Vector2 castbarpos;
    public Texture2D castbarempty;
    public Texture2D castbarfull;
    public float castbarp_y;//Distance the cast bar is placed from the middle(pos)
    public float castbarp_x;
    #endregion

    #region CastErrors
    bool b_casterror;//There is a cast error.
    string[] spellerrors;
    int c_error; //Current Error.
    float t_error;//Time Error Is displayed on screen.
    IEnumerator seterror;//Sets error state to false after t_error time.
    #endregion


    void Awake()
    {
        spellarray[0] = new spells("FireBall",1.5f, (int)enum_casting.StraightMagicShot, fireballanim, CastLocationFireBall, 100f, 0.6f, 0f,0f, true);
        spellarray[1] = new spells("FrostBolt",1.0f, (int)enum_casting.CastSpell, frostboltanim, CastLocationFrostBolt, 10f, 0.2f, 0f,0f, true);
        spellarray[2] = new spells("PermaFrost",2.0f, (int)enum_casting.CastSpell, PermaFrostGameObject, CastLocationPermaFrost, 10f, 0f, 8f,0f, true);

        aoespellarray[0] = new AoEspell(10f, 30f, 15f);
        castkeys = new string[3] { "q", "e", "r" };//Cast Keys 

        spellerrors = new string[4] { "You Must Face The Target!", "No Target!", "Currently Casting", "Spell is On CoolDown!" };
        t_error = 0.5f;//Display error for 0.5 sec

        //Cast bar Initializers
        castbarp_y = 1.5f;//Distance the cast bar is placed from the middle(pos)
        castbarp_x = 0.75f;
        castbarsize = new Vector2(160, 16);
        castbarpos = new Vector2(Screen.width / 2, Screen.height / 2);
        //Error Initializer

    }

    void Update()
    {
        checkcasting();
    }

    //A function which is responsible for checking the key pressed this frame, if the key is one of the cast keys it logs the key in castkey
    //and than the spell is casted via the cast() function.
    void checkcasting()
    {
        ///b_casting = this.GetComponent<Player_controller>().b_casting;
        foreach (string q in castkeys)
        {
            if (Input.GetKeyDown(q))
            {
                //Check if the current key pressed is one of the cast keys. 
                //If one of the cast keys are pressed and we are currently not casting the cast function is called. 
                //this.GetComponent<Player_controller>().attacking = true;//Set Player State to Attacking.
                castkey = q;//Sets attack key
                cast();
            }
        }
    }

    //A function which calls different spells based on which attack key is logged pressed this frame (set by the checkcast function).
    void cast()
    {
        if (!b_casting)
        {//If we are currently casting cant cast.
            switch (castkey)//Latest Updated cast Key
            {
                case "q"://Tied to FireBall
                    {
                        casttargettedspell((int)enum_spell.fireball);//Sends information to the cast spell function
                        break;
                    }
                case "e":
                    {

                        casttargettedspell((int)enum_spell.frostbolt);
                        break;
                    }
                case "r":
                    {
                        //AoE spell and its subfunctions are responsible for setting b_casting/attacking to true/ false. and finishing the attack.
                        AoESpell((int)enum_AoESpells.PermaFrost, (int) enum_spell.PermaFrost);//Cast Perma Frost.
                        break;
                    }
                default:
                    {
                        Debug.Log("Nothing to Cast");
                        break;
                    }
            }
        }
        else
        {
            Debug.Log("Currently Casting");
            displayerror((int)enum_spellerrors.CurrentlyCasting);
        }
    }

    //A function which handles instantiating and setting the variables of targetted spells.
    void casttargettedspell(int spellname)
    {
        if (spellarray[spellname].available)
            //If the spell is available, cast it other wise call the error function and display the "spell on cooldown" error in the else statement.
        {
            Target = this.GetComponent<Player_controller>().Target;//Get current target stored within player controller.
            if (Target == null)
            {
                b_casting = false;//Set Attacking to false
                displayerror((int)enum_errors.NoTarget);//No Target error.
                return;//Stop execution of function.
            }
            if (!b_casting)//If we are currently casting cant cast another spell;
            {
                Vector3 v_ptot = Target.transform.position - this.transform.position; //Vector from player to target.
                float a_ptot = Vector3.Angle(this.transform.forward, v_ptot);
                //Angle between the targets forward direction and the vector from player to target
                if (a_ptot > 135f)
                {//If the target is not within a 45 degrees from either side of the players front vector, you cant cast, must face them.
                    displayerror((int)enum_spellerrors.YouMustFaceTheTarget);
                    b_casting = false;
                    //Display error of not facing target
                }
                else
                {//Go ahead with cast we are facing target. 
                    casttargspell = targettedspell(spellname);//Current Spell Coroutine
                    StartCoroutine(casttargspell);
                }
            }
            else
            {
                displayerror((int)enum_spellerrors.CurrentlyCasting);
            }
        }
        else
        {
            displayerror((int)enum_spellerrors.SpellOnCooldown);
        }
    }
    IEnumerator targettedspell(int spellname)
    {//Spell Coroutine which generates a spell provided with a cast time, cast anim, and object which is defined via the caller. 
     //this.GetComponent<GameObject> 
        b_casting = true;
        this.GetComponent<Animator>().speed = 0.5f;//This speed works well for all spells.

        spells c_spell = spellarray[spellname];
        //Current spell struct which contains the information regarding the spell
        float casttime = c_spell.casttime * (1f - this.GetComponent<Properties>().haste / GeneralProperties.hastecap);
        //Cast time * fraction(of haste cap the player has achieved) 
        float dmg = c_spell.b_damage + (GetComponent<Properties>().AP * c_spell.APratio);//Base Damage of spell + Ability Power * AP ratio
        GameObject castobject = c_spell.spell;//Cast Object of Spell
        this.GetComponent<Player_controller>().mainsubmachinestate = (int)mainsubmachinstates.Casting;//Change Main SubState to Casting.
        this.GetComponent<Player_controller>().currentCastinganim = c_spell.castanim;//Set the currentcasting anim to spell casting animation.
        Transform castlocation = c_spell.s_location;//The Location where the Spell will be instantiated.

        spell_casting = spellname;//Update current spell being casted.
        s_starttime = Time.time;//Start timer of current spell

        yield return new WaitForSeconds(casttime);//Wait for cast time to completed

        StartCoroutine(CoolDown(spell_casting));//Start cooldown coroutine and pass it the current spell being casted.

        b_casting = false;
        this.GetComponent<Animator>().speed = 1f;//Set Speed back to 1

        if (Target != null)
        {//Ensure that spell doesnt get instantiated if target is dead.
            GameObject spell = Instantiate(castobject, castlocation.position, this.transform.rotation) as GameObject;
            //Cast a fire ball and keep a variable of its gameobject.
            spell.GetComponent<Targetedspell>().target = Target;//Set the target of our spell to current target.
            spell.GetComponent<Targetedspell>().damage = dmg;//Damage of Spell
            spell.GetComponent<Targetedspell>().castedby = this.gameObject;//The Game Object which casted the spell;
            spell.GetComponent<Targetedspell>().c_spell = spellname;//Notify Target spell of the current spell being casted
        }

    }

    //A function which is responsible for handling AoE spells. Takes in 2 parameters ,aoespellname, which AoE spell to cast this is used 
    //for locating its AoE special properties, the other is spellname which is its general properties.
    public void AoESpell(int aoespellname, int spellname)
    {//Function which handles AoE spells, takes in param spell name which corresponds the the location of spell information with in aoespellarray.
        if (spellarray[spellname].available)
        //If the spell is available, cast it other wise call the error function and display the "spell on cooldown" error in the else statement.
        {
            if (!b_casting) {
                switch (aoespellname)
                {
                    case (int)enum_AoESpells.PermaFrost:
                        cast_permafrost = castPermaFrost();//Start the casting routine.
                        StartCoroutine(cast_permafrost);
                        break;
                    default:
                        break;
                }
            }else
            {
                displayerror((int)enum_spellerrors.CurrentlyCasting);
            }
        }
        else
        {
            displayerror((int)enum_spellerrors.SpellOnCooldown);
        }
    }

    public IEnumerator castPermaFrost()
    {
        spells c_spells = spellarray[(int)enum_spell.PermaFrost];//General Spell Values of permafrost.

        b_casting = true;
        this.GetComponent<Animator>().speed = 0.5f;//This speed works well for all spells.

        spell_casting = (int)enum_spell.PermaFrost;//Update current spell being cast.

        this.GetComponent<Player_controller>().mainsubmachinestate = (int)mainsubmachinstates.Casting;
        this.GetComponent<Player_controller>().currentCastinganim = (int)enum_casting.PermaFrost;
        AoEspell c_spell = aoespellarray[(int)enum_AoESpells.PermaFrost];//AoE specific values

        GameObject castobject = c_spells.spell;//Game Object to cast.
        float dmg = c_spells.b_damage;
        float radius = c_spell.radius;
        float speed = c_spell.speed;
        Transform origin = c_spells.s_location;
        float casttime = c_spells.casttime;

        s_starttime = Time.time;//Log the spell start time

        yield return new WaitForSeconds(casttime * (1f - this.GetComponent<Properties>().haste / GeneralProperties.hastecap));

        this.GetComponent<Animator>().speed = 1f;//Set Speed Back to one

        StartCoroutine(CoolDown(spell_casting));//Start cooldown coroutine and pass it the current spell being casted.

        GameObject spell = Instantiate(castobject, origin.position, this.transform.rotation) as GameObject;
        spell.GetComponent<PermaFrost>().castedby = this.gameObject;
        spell.GetComponent<PermaFrost>().radius = radius;//Max Radius of spell
        spell.GetComponent<PermaFrost>().damage = dmg;//Base Damage of Spell
        spell.GetComponent<PermaFrost>().speed = speed;//Speed of Expantion.
        b_casting = false;//Done Casting


    }

    void OnGUI()
    {//ONGUI to display cast bar

        float casttime = spellarray[spell_casting].casttime * (1f - this.GetComponent<Properties>().haste / GeneralProperties.hastecap);
        if (b_casting)
        {
            //Only draw cast bar if casting.
            //draw background: pos is set at middle of screen, and size is defined.
            GUI.BeginGroup(new Rect(castbarpos.x * castbarp_x, castbarpos.y * castbarp_y, castbarsize.x, castbarsize.y));
            //Begins a group defind at pos.x, and pos.y*(factor from middle), all nested images are centered at this position.

            GUI.DrawTexture(new Rect(0, 0, castbarsize.x, castbarsize.y), castbarempty);
            //Draw the BackGround of cast.

            GUI.Label(new Rect(castbarsize.x / 2, 0, castbarsize.x, castbarsize.y), 
                (spellarray[spell_casting].casttime - (Time.time - s_starttime)).ToString("F2"));
            //Lable displays long we have till cast is over.

            //draw filled in part over the back ground:
            float ratio = (Time.time - s_starttime) / casttime;//Percent through cast
            GUI.BeginGroup(new Rect(0, 0, castbarsize.x * ratio, castbarsize.y));
            //This Layer is cast over the parent layer its size is inherited by the box nested within.

            //GUI.Box(new Rect(0, 0, size.x , size.y ), fulltex);
            GUI.DrawTexture(new Rect(0, 0, castbarsize.x, castbarsize.y), castbarfull);//This is drawn within the above specified group.
            //GUI.Label(new Rect(castbarsize.x / 2, 0, castbarsize.x, castbarsize.y), (Time.time - s_starttime).ToString("F2"));//Displays cast time.

            GUI.BeginGroup(new Rect(0, 0, castbarsize.x, castbarsize.y));
            //Draw the label on top of all prior GUI which shows the cast timer and the current spell being casted.
            //Its Location is centered at the cast background location (Top Group).

            //GUI.Label(new Rect(castbarsize.x, 0, castbarsize.x, castbarsize.y), spellarray[spell_casting].Name);//Displays cast name.
            GUI.Label(new Rect(castbarsize.x / 2, 0, castbarsize.x, castbarsize.y),
                (spellarray[spell_casting].casttime - (Time.time - s_starttime)).ToString("F2"));//Displays cast time.

            GUI.EndGroup();//End the cast timer display group
            GUI.EndGroup();//End the cast filling display group
            GUI.EndGroup();//End the castbar back ground group.
        }

        //If there is currently an error present display it.
        if (b_casterror)
        {
            GUI.Label(new Rect( 0.8f * Screen.width / 2, Screen.height / 2, 240, 240), spellerrors[c_error]);
            //Display the latest error
        }
    }
    void displayerror(int err)
    {//Function takes in value of error and sets it as current error and displays error on screen.
        b_casterror = true;//Display error to true.
        c_error = err;
        seterror = SetErrorFalse(t_error);//Set error to false after the display time is over
        StartCoroutine(seterror);
    }

    //Sets the error to false after a display time.
    IEnumerator SetErrorFalse(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        b_casterror = false;
    }

    //IEnumerator Responsible for setting the availability of current spell, upon call the spells availabity is set to false and is set true upon 
    //the completion of its cooldown.
    IEnumerator CoolDown(int spellname)
    { 
        spellarray[spellname].available = false;//Cooldown started avilability = false.
        spellarray[spellname].initCD = Time.time;//Time When Cool Down Started.
        yield return new WaitForSeconds(spellarray[spellname].cooldown);
        Debug.Log("Available = true" + spellname);
        spellarray[spellname].available = true;//Cooldown finished avilability = true.
    }
}
