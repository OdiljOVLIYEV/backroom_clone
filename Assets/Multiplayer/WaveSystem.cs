using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class WaveSystem : MonoBehaviour
{
    public Text text;
    public float Maxtime;
    public Transform[] spawnPoints;
    public GameObject enemy;
    public float MaxEnemy;
    public float timespawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timeWave());
    }
    
    IEnumerator timeWave()
    {

        while (Maxtime > 0)
        {

            
            yield return new WaitForSeconds(1f);
            Maxtime -= 1;
            text.text=Maxtime.ToString();

            if (Maxtime == 0)
            {

                StartCoroutine(Enemydrop());
            }
        }
        

    }
    IEnumerator Enemydrop()
    {

        while (MaxEnemy > 0)
        {

            
     
        yield return new WaitForSeconds(timespawn);
            MaxEnemy -= 1;
        }


    }
    // Update is called once per frame
    void Update()
    {
        text.text = Maxtime.ToString();
    }

    public void spawn()
    {

       // Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
       // PhotonNetwork.Instantiate(enemy.name, spawnPosition, Quaternion.identity);
    }

}
