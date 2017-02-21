using UnityEngine;
using System.Collections;

public class CubeProperties : MonoBehaviour {

    void Awake()
    {
        this.name = "Cube";
        GetComponent<Properties>().attackable = true;
        GetComponent<Properties>().aspeed_m = 1f;
        GetComponent<Properties>().initHP = GeneralProperties.playerHP;
        GetComponent<Properties>().currentHP = GetComponent<Properties>().initHP;
        GetComponent<Properties>().attackdmg = 1f;
        GetComponent<Properties>().AP = 1f;
        GetComponent<Properties>().sightrange = 400f;//Sight range as a multiple of this.capsule.radius
        GetComponent<Properties>().omega = 4.0f;
        GetComponent<Properties>().speed = 1f;
        GetComponent<Properties>().animspeed = 1f;
        GetComponent<Properties>().dead = false;
    }
}
