using UnityEngine;
using System.Collections;

public class SkeletonProperties : MonoBehaviour {
    //Function which instantiates skeletons
    public int test;
    public static int count = 0;
    //Number of times the skeleton has been spawned incremented within the spawn skeleton script, deincremented once a skeleto has died.
    void Awake()
    {
        this.name = "Skeleton" + count;//InstanstiateSkeleton.called;
        GetComponent<Properties>().attackable = true;
        GetComponent<Properties>().aspeed_m = 1f;//Attack Speed
        GetComponent<Properties>().initHP = GeneralProperties.skeletonHP;//Initial HP
        GetComponent<Properties>().currentHP = GetComponent<Properties>().initHP;//Current HP
        GetComponent<Properties>().attackdmg = 100f;//Attack DMG
        GetComponent<Properties>().AP = 1f;
        GetComponent<Properties>().sightrange = 500f;
        GetComponent<Properties>().omega = 4.0f;//Rotaion Speed.
        GetComponent<Properties>().speed = 4f;//Walk speed.
        GetComponent<Properties>().animspeed = 1f;
        GetComponent<Properties>().dead = false;
        GetComponent<Properties>().attacktime = 2.8f;
    }
}
