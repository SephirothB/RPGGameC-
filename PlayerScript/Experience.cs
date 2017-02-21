using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Experience : MonoBehaviour {
    public Vector2 pos;//Position of the Experience Bar as a factor of screen height and width.
    public Vector2 size;//Size of the Experience Bar
    public Texture2D background;//EXP empty(Background)
    public Texture2D full;//Exp full
    public float exp_cap;//The experience cap, a function of level of this character.

    public Vector2 lvl_pos;
    public Vector2 lvl_size;//Size and location of the level displayer
    int c_lvl;

    bool expgainGUI;

	// Use this for initialization
	void Start () {
        c_lvl = 1;//Initialize level at 1
        exp_cap = c_lvl * 100f;//exp_cap is current lvl * 100
        pos.x = 0.6f;
        pos.y = 1.6f;
        size.x = 404f;
        size.y = 18f;

        lvl_pos.x = -.13f;
        lvl_pos.y = 0.07f;
        lvl_size.x = 196f;
        lvl_size.y = 20f;
	}
	
	// Update is called once per frame
	void Update () {
        lvlup();
	}
    public void gainexperience(float exp)
    {
        string todisplay = string.Format("Experience: {0}", exp);
        InstantiateFloatingText.DisplayTextOnScreen(todisplay);
        GetComponent<Properties>().experience = GetComponent<Properties>().experience + exp;
        
    }
    void lvlup()
    {
        float c_exp = this.GetComponent<Properties>().experience;
        if(c_exp >= exp_cap)
        {
            float extraxp = c_exp - exp_cap;//Log the extra exp if player gains more exp than needed to level
            this.GetComponent<Properties>().experience = extraxp;//Reward player with the overflow, and reset exp count
            c_lvl++;//Increase Level
            exp_cap = c_lvl * 100;//Update exp cap, = current level * 100;
        }

    }
    void OnGUI()
    {
        GUI.BeginGroup(new Rect((Screen.width / 2) * lvl_pos.x, (Screen.height / 2) * lvl_pos.y, lvl_size.x, lvl_size.y));
        string lvl = string.Format("Current Level : {0}", c_lvl);
        GUI.Label(new Rect(lvl_size.x / 2, 0, lvl_size.x, lvl_size.y), lvl);
        GUI.EndGroup();

        float ratio = this.GetComponent<Properties>().experience / exp_cap;//Ratio of current experience and the cap
        GUI.BeginGroup(new Rect((Screen.width / 2) * pos.x, (Screen.height / 2) * pos.y, size.x, size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), background);//Background rect centred at the nested group
        GUI.BeginGroup(new Rect(0, 0, size.x, size.y));//Overlay current experience
        GUI.DrawTexture(new Rect(0, 0, size.x * ratio, size.y), full);//Background rect centred at the nested group
        string exp = string.Format("{0} / {1}", this.GetComponent<Properties>().experience, exp_cap);
        GUI.Label(new Rect(size.x/2, 0, size.x, size.y), exp);//At the middle of exp bar.
        GUI.EndGroup();
        GUI.EndGroup();
    }
    void setbool(ref bool boolean, bool setstate)
    {
        boolean = setstate;
    }
}
