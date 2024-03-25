using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class zombieattack : MonoBehaviourPunCallbacks
{
	public BoxCollider bx;
	public float  damage=10f;
	public bool attack;
	private Transform playertransform;
    // Start is called before the first frame update
    void Start()
	{
    	
		playertransform=GameObject.FindGameObjectWithTag("Player").transform;
	    
	    bx= FindObjectOfType<BoxCollider>();
		attack=false;
	    
    }
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		
		
		
		
	}
	// Update is called once per frame


	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{   attack=true;
			
			other.gameObject.GetComponent<PhotonView>().RPC("damage", RpcTarget.All,damage);
			//PlayerHealt healt = FindObjectOfType<PlayerHealt>();
			//healt.damage(damage);
			//detector det=FindObjectOfType<detector>();
			//det.Wander();
			/*if (attack == true)
			{
                
				

			}*/


		}

		if (other.gameObject.tag == "taxta")
		{




			if (attack == true)
			{

				MakeBarrekada damageS = other.gameObject.GetComponent<MakeBarrekada>();
				damageS.axeDamage(damage);





			}
		
	
		}
		
	  
    }
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		

        if (other.gameObject.tag == "taxta")
        {


            attack = false;


        }
    }
	
	
	public void meshenabled(){
		
		bx.enabled=true;
	}
	
	public void meshdisabled(){
		
		bx.enabled=false;
	}

	
	[PunRPC]
	public void attackoff(){
		
		attack=false;
	}
	
	
}
