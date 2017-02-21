using UnityEngine;
using System.Collections;

public class Properties : MonoBehaviour//General Class properties, this 
    //script is attached to all objects but is uniquely instatiated by its property instantiator at awake.
{
     
    public string Name;
    public bool attackable;
    public float aspeed_m ; //attack speed multiplier.
    public float initHP;//Initial HP initiallized to 1
    public float currentHP;//Current HP
    public float attackdmg;//Flat Attack Damage
    public float AP;//Magic damage power
    public float sightrange ; //Sight range as a multiple of capsule collider radius
    public float omega ;
    public float speed ;
    public float animspeed ;
    public bool dead;
    public GameObject lastkilledby;//Name of gameobject last killed by.
    public int deadcount= 0;
    public float haste;//Spell casting speed
    public float attacktime;//Time from start to finish of a melee attack.
    public float experience;//Current ecperience of character initialized at zero, upon level up is seet to zero again and experience cap increased.
}

