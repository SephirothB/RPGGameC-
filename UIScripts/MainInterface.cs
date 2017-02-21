using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//This Interface is responsible for updating information regarding the player(Health) which is done by showing current HP, and a ratio of HP. 
public class MainInterface : MonoBehaviour {
    public  Text text;
    public Image image;
    References Refer;
    GameObject player;
    // Use this for initialization
    void Awake()
    {
        
    }
    void Start () {
        player = GameObject.Find("Player");//After awake functions have been called, player has been defined.
    }
	
	// Update is called once per frame
	void Update () {
        updateDisplay();
    }
    void updateDisplay()
    {
        string Text = string.Format("{0}  / {1}", player.GetComponent<Properties>().currentHP, player.GetComponent<Properties>().initHP);
        text.text = Text;
        float ratio = player.GetComponent<Properties>().currentHP / player.GetComponent<Properties>().initHP;
        image.rectTransform.localScale = new Vector3(ratio, 1, 1);//Update the X component of health axis ratio. 
    }
}
