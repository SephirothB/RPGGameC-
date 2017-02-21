using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Score : MonoBehaviour {

    public int score = 0;
    private Text text;
    string textt;
    void Start()
    {
        score = 0;
         text = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        //Debug.Log("In start");
        
        UpdateDisplay();

    }
   public  void Add(int amt)
    {
        score += amt;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        text.text = "Score : " + score;
    }
}
