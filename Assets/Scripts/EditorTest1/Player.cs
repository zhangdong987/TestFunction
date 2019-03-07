using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int id;
    public string playerName;
    public string backStroy;
    public float health;
    public float damage;
    public float weapondamage1, weapondamage2;
    public string shoeName;
    public int shoeSize;
    public string shoeType;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
    }

   
}
