using UnityEngine;
using System.Collections;

//A class which handles damage done to players.
public class PlayerHealth : MonoBehaviour
{

    private IEnumerator damaged;//damaged coroutine.
    float d_time;
    //Time spent revovering from getting damaged.
    // Use this for initialization
    void Start()
    {
        d_time = 0.5f;//Recovery time after getting hit, increased due to fatigue.
    }

    public void damage(float damage, GameObject DamagedBy)
    {//Function responsible for handling damage, and setting the appropriate animators and display text.
        string todisplay = string.Format(" - {0} ", damage);
        InstantiateFloatingText.DisplayDamageOnScreen(todisplay);
        //Display damage on screen of player.

        StartCoroutine(c_damage(damage, DamagedBy));
    }

    //Coroutine responsible for updating the health of this character after it has been damaged.
    IEnumerator c_damage(float damage, GameObject damagedby)
    {


        GetComponent<Properties>().currentHP = GetComponent<Properties>().currentHP - damage;

        if (GetComponent<Properties>().currentHP <= 0)
        {//Component this script is attached to is dead.
            GetComponent<Properties>().currentHP = 0;
            GetComponent<Properties>().lastkilledby = damagedby;//Game object which casted the spell
            GetComponent<Properties>().deadcount++;
            GetComponent<Properties>().dead = true;//Char is dead
            GetComponent<Properties>().attackable = false;//Char is not attackable anymore
            GetComponent<DeadCheck>().enabled = true;
        }
        float c_hp = this.GetComponent<Properties>().currentHP;
        float maxhp = this.GetComponent<Properties>().initHP;
        
        yield return new WaitForSeconds((d_time) * (2f - c_hp / maxhp));//Wait for recover time before setting damaged to false.
        
    }
}
