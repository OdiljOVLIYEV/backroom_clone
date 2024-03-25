using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class SpawnEnemy : MonoBehaviour
{
    
    public Transform[] spawnPoints;
    public GameObject enemy;
    public float MaxEnemy;
    private float Max_zombie;
    public float timespawn;
    public float MAX_ZOMBIE;
    private PhotonView photonView;
    public bool allkill;
    void Start()
    {


        
        photonView = GetComponent<PhotonView>();
        allkill = false;





    }
   public IEnumerator Enemydrop()
    {
        MAX_ZOMBIE = MaxEnemy;

        while (MaxEnemy > 0)
        {
            
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            PhotonNetwork.Instantiate(enemy.name, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timespawn);
            MaxEnemy -=1;
            
        }
       

    }

    // Update is called once per frame
    private void Update()
    {
        if (MaxEnemy == 0)
        {

            if (MAX_ZOMBIE == 0)
            {

                allkill = true;

            }
        }



        /* if(PhotonNetwork.IsMasterClient==false || PhotonNetwork.CurrentRoom.PlayerCount !=2)
         {
             return;

         }*/

    }
    public void spawn()
    {
        
        
    }
    
}
