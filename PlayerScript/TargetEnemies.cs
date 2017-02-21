using UnityEngine;
using System.Collections;

public class TargetEnemies : MonoBehaviour {
    RaycastHit hit;
    // Use this for initialization
    GameObject l_target;//Holds the last target, used to deselect the last target
    GameObject c_target;//Holds the current target
    void Start () {
        c_target = null;
        l_target = c_target;
	}
	
	// Update is called once per frame
	void Update () {
        target();
    }
    void target() {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Cast a ray from the Plane of screen starting at the current position of the mouse.
            Physics.Raycast(ray, out hit, 100);
            if (hit.transform != null)
            {//For skys
                if (hit.transform.name != "Terrain" && hit.transform.gameObject != null && hit.transform.name != "Player")
                {//Dont select terrain or if gameobject is a null(ie sky)
                    GameObject clicked = hit.transform.gameObject;
                    if (c_target == null)//Only used once for initialization
                    {
                        selecttarget(clicked);//Select a target
                        l_target = c_target;//Set last and current target equal on first target.
                    }
                    else if (clicked.name != c_target.name)
                    {//Select the new target if player clicks on gameobject not registered as c_target
                        selecttarget(clicked);//Select new target 
                        deselecttarget(l_target);//Deselect old target
                    }
                }
            }
        }
    }//Function responsible for targeting enemies utilizing the ray cast function.
    void selecttarget(GameObject select)
        //Select the current target located by raycast hit.
    { 
        l_target = c_target;//Update last target to current target
        c_target = select;//Update the current target to what we are the player has clicked on in this frame.
        //Debug.Log(hit.transform.name); 
        this.GetComponent<Player_controller>().Target = select;//Set player target to one clicked on.
        Transform Projector = select.transform.FindChild("Target");
        if (Projector != null)
        {
            Projector.gameObject.SetActive(true);//Set projecter to active.
        }
    }
    void deselecttarget(GameObject deselect)
    { //Turns off the target projector ovver last target, new target is up dated within the selecttarget function.
        Transform Projector = deselect.transform.FindChild("Target");//The projector gameobject name
        Projector.gameObject.SetActive(false);//Set the attack indicator to false
    }//Deselect the last target, called when new target is selected.
}
