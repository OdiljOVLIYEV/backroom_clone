using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
	GameObject playerPrefab;
	
	public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {

       
            if (playerPrefab!=null)
            {
            	if (spawnPoints != null && spawnPoints.Length > 0 && playerPrefab != null)
            	{
	            	// Bir spawn nuqtasini tanlash
	            	Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

	            	// Yangi obyekt spawn qilinadi
	            	PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
            	}
            	else
            	{
	            	Debug.LogWarning("Spawn pointlar yoki obyekt aniqlanmagan!");
            	}
	            //int randomPoint = Random.Range(-40, -30);
	            // PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, -15.2f, randomPoint), Quaternion.identity);
            }
            else
            {
                Debug.Log("Place playerPrefab!");
            }





        
	   

        
    }
    
	
    // Update is called once per frame
    void Update()
    {
	   
    }
    
	//#region Public Methods
    
	/*public void LeaveRoom() 
	{
		PhotonNetwork.LeaveRoom();
	}
	#endregion
	
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("LobbyScene");
	}*/
	
	

	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("LobbyScene");
		

	
		
	}


	public void LeaveRoomAndReturnToLobby()
	{
		
		if (PhotonNetwork.IsMasterClient)
		{  Debug.Log("SERVER EGASI");
			//PhotonNetwork.CurrentRoom.IsOpen = false; // makes room close 
			//PhotonNetwork.CurrentRoom.IsVisible = false;
			//photonView.RPC("LeaveRoom", RpcTarget.Others);
			PhotonNetwork.LeaveRoom();
			//SceneManager.LoadScene("LobbyScene");
			//photonView.RPC("hammachiqsin", RpcTarget.All);
			
		}else{
			Debug.Log("SERVER EGASI BOSHQA");
			//PhotonNetwork.InRoom
			//PhotonNetwork.Disconnect();
			PhotonNetwork.LeaveRoom();
			
			
		}
		
		}	

	[PunRPC]
	private void LeaveRoom()
	{
		// leave the room
		PhotonNetwork.LeaveRoom();
	}
	
	public void levelload(){
		
		if (PhotonNetwork.IsMasterClient){
			photonView.RPC("levelloading", RpcTarget.All);
		}
	}
	
	[PunRPC]
	private void levelloading(){
		
		PhotonNetwork.LoadLevel("level2");
		
	}
	
	
}
