using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayButton : MonoBehaviour {
    void Start()
    {

        //Button Button = GameObject.FindGameObjectWithTag("MMB1").GetComponent<Button>();
        Button Button = this.GetComponent<Button>();
        Button.onClick.AddListener( () => { PlayGame(1); });//
            
        
    }

    public void PlayGame(int level) {
        SceneManager.LoadScene(level);//Play game at Level One;
}

}
