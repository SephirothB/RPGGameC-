using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FireBall : MonoBehaviour {
    public Button button;
    public float x = -0.3f; 
    public float y = -0.4f;
    GameObject Player;
    // Use this for initialization
    void Start () {
        GetComponentInChildren<Text>().text = "Q";//Set the Text overlayed(children) on top of this button to Keycode of PermaFrost.
        x = -.3f;
        y = -0.4f;
        button = GetComponent<Button>();
        //button.transform.position = new Vector3(0,0,0);
        button.transform.localPosition = new Vector3(Screen.width * x, Screen.height * y, 0);
        Player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        button.transform.localPosition = new Vector3(Screen.width * x, Screen.height * y, 0);
        if (Input.GetKeyDown(KeyCode.Q))
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
 

}
