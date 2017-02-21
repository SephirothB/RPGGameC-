using UnityEngine;
using System.Collections;

public class InstantiateGuard : MonoBehaviour {

    float spawntime = 2.0f;
    public static int called;//Static variables holds how many times the script has been called.
    bool creating;//Checking whether coroutine is done.
    public GameObject current;
    float spawnradius = 30f;
    public static int maxobject = 3;
    // Use this for initialization
    void Awake()
    {
        called = 0; 
        creating = false;
    }
    void Start()
    {

    }
    IEnumerator instantiate(GameObject NPC)
    {
        Vector3 position = this.transform.position + Random.insideUnitSphere * spawnradius; position.y = this.transform.position.y;
        // Debug.Log("Creating:  "); Debug.Log(called);
        creating = true;
        GameObject IC = GameObject.Instantiate(NPC, position, Quaternion.identity) as GameObject;
        called++;
        yield return new WaitForSeconds(spawntime);
        creating = false; //done creating
    }
    // Update is called once per frame
    void Update()
    {

        if (!creating && (called < maxobject))
        {
            StartCoroutine(instantiate(current));
        }//Only Instantiate if done creating, and there are less than 
        //maxobject skeletons out.
        else
        {
            //Debug.Log("Creating:  "); Debug.Log(creating);
            //Vector3 position = this.transform.position + Random.insideUnitSphere * spawnradius; position.y = this.transform.position.y;
            //Debug.Log(position);
        };

    }
}
