using UnityEngine;
using System.Collections;

public class PermaFrost : MonoBehaviour
{//Class Responsible for the PermaFrost Spells.
    public GameObject castedby;//Person who casted this spell.
    public float damage;
    public float radius;
    public float speed;
    Collider thisspherecol;
    IEnumerator castAoE;

    GeneralSpell.spells s_permafrost;//Struct used to store perma frost spell properties
    // Use this for initialization
    void Start()
    {
        thisspherecol = this.GetComponent<SphereCollider>();
        this.GetComponent<SphereCollider>().radius = 0f;
        s_permafrost = GeneralSpell.spellarray[(int)enum_spell.PermaFrost];
    }

    // Update is called once per frame
    void Update()
    {
        rad_expantion(this.GetComponent<SphereCollider>(), radius, speed);
        //Increases the radius of the collider attatched to this spell till a maximum with a provided speed.
    }

    //A function responsible to increase the radiius of the sphere collider attatched to this game object every frame, at a rate of 
    //the time it took to complete last frame * a speed factor.
    public void rad_expantion(Collider col, float maxradius, float speed)
    {
        float c_radius = this.GetComponent<SphereCollider>().radius;
        if (c_radius < maxradius)
        {//Increase the Radius till Maximum
            c_radius = this.GetComponent<SphereCollider>().radius + (Time.deltaTime * speed);
        }
        else
        {
            Destroy(this.gameObject);
        }
        this.GetComponent<SphereCollider>().radius = c_radius;
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Terrain" && col.gameObject.tag != "Player")
        {
            Debug.Log(col.gameObject.name);
            if (col.gameObject != null)
            {
                //If Player or Terrain are hit skip this if Loop. Other wise cast a freeze on the game Object, which can only be NPC.
                col.gameObject.GetComponent<Health>().damage(s_permafrost.b_damage, castedby);
                castedby.GetComponent<spelleffect>().casteffect((int)enum_spell.PermaFrost, col.gameObject);
            } 
        }
        else
        {
            //Do Nothing
        }
    }
}
