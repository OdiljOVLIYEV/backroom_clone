using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Player_Setup : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject FPScamera;
	/* [SerializeField]
    GameObject attack;
    public GameObject axedamage;
    public GameObject WEAPON_PICK;
    public GameObject[] Fpsview;
	public GameObject[] Tpsview;*/
    
 
   // public Camera fps;
    /* [SerializeField]
     GameObject zoompis;*/

    void Start()
    {


	     if (photonView.IsMine)
        {
          
            
           
            transform.GetComponent<PlayerMovment>().enabled = true; 
            FPScamera.GetComponent<Camera>().enabled = true;
		    transform.GetComponent<flashOn>().enabled = true; 
		     FPScamera.GetComponent<MouseLook>().enabled = true;
		     transform.GetComponent<PlayerHealt>().enabled = true; 
		    
            
            
        }
        else
        {

            transform.GetComponent<PlayerMovment>().enabled = false;
            FPScamera.GetComponent<Camera>().enabled = false;
	        transform.GetComponent<flashOn>().enabled = false;
	        FPScamera.GetComponent<MouseLook>().enabled = false;
	        transform.GetComponent<PlayerHealt>().enabled = false; 
            
        }
	 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
