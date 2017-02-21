using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Transform Tip;
    public GameObject prefab;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        //Instantiate(prefab, Tip.position, Quaternion.identity);
        Instantiate(prefab, Tip.position, Tip.rotation);//Axis of projectile alligned with tip.
    }

}
