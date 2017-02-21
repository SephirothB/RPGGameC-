using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Butt_PermaFrost : MonoBehaviour
{

    int spellname;//Spellname of the spell this class is attatched too. 

    #region ButtonLocationVariables
    public Button button;
    public float x = -0.217f;
    public float y = -0.4f;
    #endregion
    GameObject Player;
    // Use this for initialization
    void Start()
    {
        spellname = (int)enum_spell.PermaFrost;//This Class is specifically for handling the button of this spell.
        this.GetComponentInChildren<Text>().text = "R";  //Set the Text overlayed(children) on top of this button to Keycode of PermaFrost.
        button = GetComponent<Button>();
        button.transform.localPosition = new Vector3(Screen.width * x, Screen.height * y, 0);
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneralSpell.spellarray[spellname].available)
        {
            this.GetComponentInChildren<Text>().text = "R";//Set the Text to R. Primarily Used for cooldown after its cool down timer is over.
            //button.transform.localPosition = new Vector3(Screen.width * x, Screen.height * y, 0);//Used for setting position commented out after.
            if (Input.GetKeyDown(KeyCode.R))
            {
                down();
            }
            else if (Player.GetComponent<Player_controller>().b_casting)
            {
                disable();
            }
            else
            {
                up();
            }
        }
        else
        {//Spell on CoolDown
            cooldown();
        }
    }
    void disable()
    {
        button.image.color = button.colors.disabledColor;
    }
    void down()
    {
        button.image.color = button.colors.pressedColor;
        //button.colors.pressedColor = true;
    }
    void up()
    {
        button.image.color = button.colors.normalColor;
    }

    void cooldown()
    {
        string text = string.Format("{0}", (int)(GeneralSpell.spellarray[spellname].cooldown +
            (GeneralSpell.spellarray[spellname].initCD - Time.time)));
        //String which displays the cool down remains of a spell this is the (SpellCoolDown - (Currenttime - SpellCoolDownStartTime(initCD) ))
        this.GetComponentInChildren<Text>().text = text;
    }
}
