﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")] public Transform trans;

    [Header("Stats")] 
    [Tooltip("How many units the projectile will move forward per second.")]
    public float speed = 34;

    [Tooltip("The distance the projectile will travel before it comes to a stop.")]
    public float range = 70;

    private Vector3 spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = trans.position;
    }

    // Update is called once per frame
    void Update()
    {
        //move along z-azis
        trans.Translate(0,0,speed *Time.deltaTime, Space.Self);
        if (Vector3.Distance(trans.position, spawnPoint) >= range)
        {
            Destroy(gameObject);
        }
    }
}
