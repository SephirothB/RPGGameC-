using UnityEngine;
using System.Collections;


public class PlayerProperties : MonoBehaviour {
    
    void Awake()
    {
        this.name = "Player";
        GetComponent<Properties>().attackable = true;
        GetComponent<Properties>().aspeed_m = 1f;
        GetComponent<Properties>().initHP = 1000f;
        GetComponent<Properties>().currentHP = GetComponent<Properties>().initHP;
        GetComponent<Properties>().attackdmg = 1f;
        GetComponent<Properties>().AP = 300f;
        GetComponent<Properties>().sightrange = 400f;//Sight range as a multiple of this.capsule.radius
        GetComponent<Properties>().omega = 2f;
        GetComponent<Properties>().speed = 15f;
        GetComponent<Properties>().animspeed = 1f;
        GetComponent<Properties>().dead = false;
        GetComponent<Properties>().haste = 0f;
        GetComponent<Properties>().experience = 0f;
    }
}
