using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Cubepath : MonoBehaviour {
    GameObject targ;
    public NavMeshAgent agent;
    public Transform target;
  
    void Start()
    {
        targ = GameObject.FindWithTag("Player");//Sets the current target as the player
       
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
    }
    
 
    void Update()
    {

        // Debug.Log(targ.transform.position);
        if (!this.GetComponent<Properties>().dead)
        {

            this.gameObject.GetComponent<NavMeshAgent>().SetDestination(targ.transform.position);// = targ.transform.position;
                                                                                                 //agent.SetDestination(target.position);//The denstination of Cube is the players location
                                                                                                 //agent.SetDestination(targ.transform.position);//The denstination of Cube is the players location
        }
        else
        {//DO nothing 
            this.gameObject.GetComponent<NavMeshAgent>().Stop();
            Debug.Log("Box is dead");
        }

    }
}
