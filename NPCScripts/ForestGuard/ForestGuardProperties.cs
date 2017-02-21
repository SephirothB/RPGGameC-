using UnityEngine;
using System.Collections;

public class ForestGuardProperties : MonoBehaviour {

    void Awake()
    {
        this.name = "ForestGuard" + InstantiateGuard.called;
        GetComponent<Properties>().attackable = true;
        GetComponent<Properties>().aspeed_m = 1f;//Attack Speed
        GetComponent<Properties>().initHP = 1000f;//Initial HP
        GetComponent<Properties>().currentHP = GetComponent<Properties>().initHP;//Current HP
        GetComponent<Properties>().attackdmg = 100f;//Attack DMG
        GetComponent<Properties>().AP = 1f;
        GetComponent<Properties>().sightrange = 1.3f;
        GetComponent<Properties>().omega = 4.0f;//Rotaion Speed.
        GetComponent<Properties>().speed = 4f;//Walk speed.
        GetComponent<Properties>().animspeed = 1f;
        GetComponent<Properties>().dead = false;
        GetComponent<Properties>().attacktime = 1.6f;
    }
}
