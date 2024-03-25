using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class flashOn : MonoBehaviourPunCallbacks
{   [SerializeField] Light lightToToggle;
	public GameObject fonar;
	public bool flash;
	private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {   pv = GetComponent<PhotonView>();
	    // fonar.SetActive(false);
	    lightToToggle.enabled=false;
    }

    // Update is called once per frame
	void Update()
	{
		
		
		if(photonView.IsMine){
	if(Input.GetKeyDown(KeyCode.F)){
	    	if(flash==false){
	    		flash=true;
		    	pv.RPC("lightON", RpcTarget.All);
	    	
	    		//fonar.SetActive(true);
	    	}
	    	else{
	    		flash=false;
	    		pv.RPC("lightOff", RpcTarget.All);
	    		//fonar.SetActive(false);
	    	}
	    	
	     }
	    
		}
	}
	
	[PunRPC]
	public void lightON()
	{
		lightToToggle.enabled=true;
		
	}
	
	[PunRPC]
	public void lightOff()
	{
		lightToToggle.enabled=false;
		
	}
}
