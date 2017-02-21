using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public Score score;//Defighn Score
    public float timer = 3f;
    public float speed = 20f;

   void Awake()
    {
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();//Assign score to the Score class attached to score text object
    }

    void Start()
    {
        Invoke("Die", timer);
    }

    void Update()
    {
        this.transform.position += this.transform.right * speed * Time.deltaTime;
        //Initial Axis of projectile is set on creation/
        
    }

    void OnTriggerEnter(Collider other)//Other is the reference to the object we have collided.
    {
        if (other.tag == "Enemy")
        {
         
            CancelInvoke();
            Destroy(other.gameObject);
            //Debug.Log("Score");
            score.Add(100);
            Die();//The game object atateched to the script dies.

        }
    }
    void Die()
    {
        Destroy(gameObject);//Little 'g' implies the game object 
        //attatched to this script.
    }
}
