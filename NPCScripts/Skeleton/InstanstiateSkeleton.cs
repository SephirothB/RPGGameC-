using UnityEngine;
using System.Collections;

public class InstanstiateSkeleton : MonoBehaviour {
    float spawntime =2.0f;
    //public static int called;//Static variables holds number of times this script has been called
    bool creating;//Checking whether coroutine is done.
    public GameObject current;
    float spawnradius = 30f;
    public static int maxobject = 4;//Max of this object to be created
	// Use this for initialization
    void Awake()
    {
        //called = 0;
        ///current = GameObject.FindGameObjectWithTag("Skeleton");
        creating = false;
    }
	void Start () {
	
	}
    IEnumerator instantiate(GameObject NPC)
    {
        Vector3 position = this.transform.position + Random.insideUnitSphere * spawnradius; position.y = this.transform.position.y;
       // Debug.Log("Creating:  "); Debug.Log(called);
        creating = true;
        GameObject IC = GameObject.Instantiate(NPC, position, Quaternion.identity) as GameObject;
        //called++;
        yield return new WaitForSeconds(spawntime);
        SkeletonProperties.count++;//Update Skeleton count in skeleton properties.
        creating = false; //done creating
    }
    // Update is called once per frame
    void Update () {
     //count to called
        if (!creating && (SkeletonProperties.count  < maxobject) ) {  StartCoroutine(instantiate(current));
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
