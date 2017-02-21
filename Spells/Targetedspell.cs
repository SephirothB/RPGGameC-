using UnityEngine;
using System.Collections;

public class Targetedspell : MonoBehaviour
{
    public int c_spell;//Spell that is being casted set by the spell instantiator.
    public GameObject target;//Target of this spell set by instantiator of spell Use this for initialization
    public GameObject castedby;//Object which casted this spell.
    int f_omega = GeneralSpell.t_spell.omega;//Fire ball rotation speed
    int f_speed = GeneralSpell.t_spell.speed; //Fire Ball Speed
    public float damage;
    public float t_radius;//radius of target

    void Start()
    { 
        t_radius = target.GetComponent<CapsuleCollider>().radius;// target.transform.position.y/10;
    }

    // Update is called once per frame
    void Update()
    {
        transalate();
    }
    void transalate()
    {
        if (target == null)
        {       //Our target is dead
            Destroy(this.gameObject);
        }
        else
        {
            //Ensure the target reaches its destination
            Vector3 targ = target.transform.position - this.transform.position +
                (target.transform.up * (target.transform.lossyScale.y * target.GetComponent<CapsuleCollider>().height) * 0.5f);
            //Vector from target to our location, the extra Y component is added to ensure the spell hits at the middle of NPC.
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    Quaternion.LookRotation(targ), f_omega * Time.deltaTime);
            //Adjust our rotation to face the Target
            this.transform.Translate(Vector3.forward * Time.deltaTime * f_speed);
            //Move in the forwards direction (already facing NPC), at the speed of the spell.
        }
    }
    void OnTriggerEnter(Collider col)
    {

        castedby.GetComponent<spelleffect>().casteffect(c_spell, target);
        if (col.gameObject.name == target.name)
        {//We have reached target
            target.GetComponent<AggChase>().lastenemy = castedby;
            //Set the enemy of the NPC to who ever casted this spell.

            target.GetComponent<Health>().damage(damage,castedby);
            //Call function responsible for health, located on all npc's. 

            Destroy(this.gameObject);//Destroy the spell
        }
    }
}


