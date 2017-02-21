using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//A class responsible for handling floating text objects. 
public class FloatingText : MonoBehaviour {
    public Animator animator;//Animator attatched to this component
    public Text PopUpText;//Damage to print.
    float cliplength;

    // Use this for initialization
    void Start()
    {
        //When the floating damage pop up is initialized.

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        cliplength = clipInfo[0].clip.length;
        //Array of Clip Infos scince there is one animator attatched to this object we need only the firt element in the array.
        PopUpText = animator.gameObject.GetComponent<Text>();
        //Sets the the text displayed to the gameobject which has the text components attatched to it (the animator).
        Destroy(this.gameObject, cliplength);
        //Destroy the floating text once its duration is over.
    }

    //The function display text is set by the object which instantiated the floating text gameobject.
    public void  DisplayText(string text)
    {
        PopUpText.text = text;//Set the text displayed to one passed to this function by damage or other.
    }

}
