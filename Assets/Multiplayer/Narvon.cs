using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Narvon : MonoBehaviour
{
    private float Speed = 1f;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // float up=Speed*Time.deltaTime;

       

    }

    public  void OnTriggerStay(Collider other)
    { 
        
        if(other.gameObject.tag=="NARVON") {
        Debug.Log("NARVON");
            

       if(Input.GetKey(KeyCode.W)) {

                player.GetComponent<PlayerMovment>().enabled= false;
                transform.position += Vector3.up * Time.deltaTime * Speed;
            
            }
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NARVON")
        {
            player.GetComponent<PlayerMovment>().enabled = true;

        }
    }
}
