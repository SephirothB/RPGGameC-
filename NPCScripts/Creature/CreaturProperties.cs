using UnityEngine;
using System.Collections;

public class CreaturProperties : MonoBehaviour
{
    void Awake()
    {
        this.name = this.name + CreatureInstantiate.called;
        GetComponent<Properties>().attackable = true;
        GetComponent<Properties>().aspeed_m = 1f;//Attack Speed
        GetComponent<Properties>().initHP = GeneralProperties.CreatureHP;//Initial HP
        GetComponent<Properties>().currentHP = GetComponent<Properties>().initHP;//Current HP
        GetComponent<Properties>().attackdmg = 20f;//Attack DMG
        GetComponent<Properties>().AP = 1f;
        GetComponent<Properties>().sightrange = 50f;//Creature radius 10x larger than that of skeleton
        GetComponent<Properties>().omega = 4.0f;//Rotation Speed.
        GetComponent<Properties>().speed = 4f;//Walk speed.
        GetComponent<Properties>().animspeed = 1f;
        GetComponent<Properties>().dead = false;
        GetComponent<Properties>().attacktime = 0.9f;
    }
}
