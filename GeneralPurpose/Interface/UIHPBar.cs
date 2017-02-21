using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIHPBar : MonoBehaviour {
    public Image HPBar;
    GameObject NPC;
	// Use this for initialization
	void Start () {
	    NPC = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        float ratio = NPC.GetComponent<Properties>().currentHP / NPC.GetComponent<Properties>().initHP;
        HPBar.rectTransform.localScale = new Vector3(ratio, 1, 1);//Update the X component of health axis ratio. 
    }
}
