using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCountManager : MonoBehaviourPunCallbacks
{
	void Start()
	{
		// Subscribe to the event when a player enters the room
		PhotonNetwork.AddCallbackTarget(this);
	}

	void OnDestroy()
	{
		// Unsubscribe from the event when the script is destroyed
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		// Called when a new player enters the room
		Debug.Log("Player entered. Total players: " + PhotonNetwork.CurrentRoom.PlayerCount);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// Called when a player leaves the room
		Debug.Log("Player left. Total players: " + PhotonNetwork.CurrentRoom.PlayerCount);
	}

	void Update()
	{
		// Check current player count (just for demonstration, can be removed)
		Debug.Log("Current player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
	}
}
