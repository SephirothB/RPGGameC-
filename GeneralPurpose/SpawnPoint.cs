using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public GameObject prefab;
    public float repeatTime = 3f;
   //ameObject g = GameObject.FindGameObjectWithTag ("SpawnPoint");
   
    void Start()
    {
        InvokeRepeating("Spawn", 2f, repeatTime);
        
    }
    void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
