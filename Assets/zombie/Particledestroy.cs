﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particledestroy : MonoBehaviour
{
    public bool TPS_EFFECT = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
            Destroy(gameObject, 0.4f);
    }
}
