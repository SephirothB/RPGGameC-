using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class DeadCheck : MonoBehaviour
{
    bool d_trigger;//A bool which is set true once a character animator death trigger is set true this ensures that the trigger is not set every frame.

    // Use this for initialization
    void Start()
    {
        d_trigger = false;//Set Death animation trigger to false
        if (this.GetComponent<Properties>().dead)
        {
            StartCoroutine(Dead(this.gameObject));
        }
        else
        {
        };

    }
    IEnumerator Dead(GameObject dyingchar)
    {
        //Set the character state to dead, and display the appropriate animation.
        if (this.name == "Player")
        {//Start player death animation.
            if (!d_trigger)
            {
                Debug.Log("Setting dead true");
                d_trigger = true;
                this.GetComponent<Animator>().SetTrigger("Dead");
            }
        }


        GameObject Killer = dyingchar.GetComponent<Properties>().lastkilledby;
        //Gameobject which killed this character.

        if (Killer.name == "Player")
        {//If killed NPC killed by Player award experience to player.
            Killer.GetComponent<Experience>().gainexperience(50f);
            //Call the gain experience function within attatched to player to increase its EXP.
        }

        yield return new WaitForSeconds(GeneralProperties.deathtime);

        if (dyingchar.tag != "Player" && (dyingchar != null))
        {//Increase the max ammount of spawned object of the type, idenetified via a tag,
            //This keeps track of dead enemies by type and ensures new ones spawn.

            switch (this.tag)
            {
                case "Creature":
                    CreatureInstantiate.maxobject++;
                    break;
                case "Skeleton":
                    InstanstiateSkeleton.maxobject++;
                    break;
                case "ForestGuard":
                    InstantiateGuard.maxobject++;
                    break;
                default:
                    Debug.Log("No Match");
                    break;
            }
            //dyingchar.GetComponent<AggChase>().att
            Destroy(dyingchar);
        }
        //Ensures that when the character has been killed by two different
        //objects at the same time the first and the first of the two destroys the character we dont reference it again.
        else
        {//Player Deaths are handled here.
            SceneManager.LoadScene(0);
        };


    }

}
