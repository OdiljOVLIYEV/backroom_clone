using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerHealt : MonoBehaviourPunCallbacks
{
	public float Healt=10f;
	public  TMP_Text text;
	public Animator dead;
	public GameObject CAMERA,CAMERA2;
	public CapsuleCollider cs;
	public CapsuleCollider cstwo;
	public CharacterController CHAC;
	public PhotonView pw;
    // Start is called before the first frame update
    void Start()
    {
	    cs=GetComponent<CapsuleCollider>();
	    CHAC=GetComponent<CharacterController>();
	    pw=GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
	{
		if(Healt<0){
		
			Healt=0f;
			
		}
		text.text=Healt.ToString();
		
	}
    
    
	
	
    
	[PunRPC]
	public void damage(float amount){
		Healt-=amount;
		if(Healt<=0f){
			
			
			
			if (photonView.IsMine)
			
			{ CAMERA.SetActive(false);
				CAMERA2.SetActive(true);
				pw.RPC("EnemyAnim", RpcTarget.All);
				
		
				
				Debug.Log("dead");
			}
			
			
		}
	}
	
	[PunRPC]
	public void EnemyAnim() {
		transform.GetComponent<PlayerMovment>().enabled = false;
		
		dead.enabled=false;
		cs.enabled=false;
		cstwo.enabled=false;
		CHAC.enabled=false;
	}
	
}
