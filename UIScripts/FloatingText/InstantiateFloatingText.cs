using UnityEngine;
using System.Collections;

//A class which is responsible for holding function that call floating text is world or screen space.
public class InstantiateFloatingText : MonoBehaviour {
    public GameObject PopUpText;//General Floating Text Appears Where Specified
    public OnScreenText onScreenFloatingText;//Floating Text Only Appears On Screen When Called.
    public GameObject onScreenDamage;

    public static GameObject WorldSpaceCanvas;
    public static GameObject ScreenSpaceCanvas;

    public static GameObject Floatingtext;
    //General Floating text this text appears above NPC or other players.
    public static OnScreenText OnScreenFloatingText;
    //General Floating Text this text appears on screen and is a child on ScreenSpaceCanvas
    public static GameObject OnScreenDamage;

    void Start()
    {
 
        //Set the static variables
        WorldSpaceCanvas = GameObject.Find("WorldSpaceCanvas");
        ScreenSpaceCanvas = GameObject.Find("ScreenSpaceCanvas");

        Floatingtext = PopUpText;

        OnScreenFloatingText = onScreenFloatingText;

        OnScreenDamage = onScreenDamage;
    }
    //A function which displays damage on NPC's
    public static void DisplayDamage(GameObject DamagedObj, GameObject DamagedBy, float damage) {
        //damaged = c_damage(damage,DamagedBy);
        Vector3 TextPosition = (DamagedObj.transform.position + 
            (DamagedObj.transform.up * (DamagedObj.transform.lossyScale.y * DamagedObj.GetComponent<CapsuleCollider>().height)));
        GameObject damagetext = Instantiate(Floatingtext, TextPosition, DamagedBy.transform.rotation) as GameObject;
        //Instantiate the floating damage text at target location and give it a rotation of caster. 
        damagetext.transform.SetParent(WorldSpaceCanvas.transform, true);
        //damagetext.transform.position = DamagedObj.transform.position;
        damagetext.GetComponent<FloatingText>().DisplayText(damage.ToString());
        //Pass the damaget to the popup damge script attatced to floatingdamagetext gameobject.
    }

    //A function which displays text one screen
    public static void DisplayTextOnScreen(string Text)
    {
        OnScreenText floatingtext = Instantiate(OnScreenFloatingText);
        //GameObject floatingtext = Instantiate(OnScreenFloatingText) as GameObject;
        //Instantiate the floating damage text at target location and give it a rotation of caster. 
        floatingtext.transform.SetParent(ScreenSpaceCanvas.transform, false);
        //damagetext.transform.position = DamagedObj.transform.position;
        floatingtext.transform.position = 
            new Vector3(floatingtext.transform.position.x * 1.3f,floatingtext.transform.position.y * 0.4f, floatingtext.transform.position.z);
        floatingtext.GetComponent<OnScreenText>().DisplayText(Text);
        //Pass the damaget to the popup damge script attatced to floatingdamagetext gameobject.
    }


    //A function which displays player damage taken on screen
    public static void DisplayDamageOnScreen(string Text)
    {
        Debug.Log("Displaing Damage");
        GameObject floatingtext = Instantiate(OnScreenDamage) as GameObject;
        //GameObject floatingtext = Instantiate(OnScreenFloatingText) as GameObject;
        //Instantiate the floating damage text at target location and give it a rotation of caster. 
        floatingtext.transform.SetParent(ScreenSpaceCanvas.transform, false);
        floatingtext.transform.position =
            new Vector3(floatingtext.transform.position.x * 0.8f, floatingtext.transform.position.y * 0.4f, floatingtext.transform.position.z);
        floatingtext.GetComponent<OnScreenText>().DisplayText(Text);
        //Pass the damaget to the popup damge script attatced to floatingdamagetext gameobject.
    }
}
