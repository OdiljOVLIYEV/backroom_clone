using UnityEngine;
using Photon.Pun;

public class GameManagers : MonoBehaviourPunCallbacks
{
	public Transform[] spawnPoints;
	public CameraSwitcher cameraSwitcher;

	void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			// Player is spawned when they join the room
			SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber - 1);
		}
		else
		{
			Debug.LogError("Photon is not connected!");
		}
	}

	void SpawnPlayer(int playerIndex)
	{
		if (playerIndex < 0 || playerIndex >= spawnPoints.Length)
		{
			Debug.LogError("Invalid spawn point index!");
			return;
		}

		// Instantiate the player prefab at the spawn point
		GameObject playerGO = PhotonNetwork.Instantiate("PlayerPrefab", spawnPoints[playerIndex].position, Quaternion.identity);
        
		// Inform the CameraSwitcher to switch to this player's camera
		cameraSwitcher.SwitchCamera(playerIndex);
	}

	// Called when a player joins the room
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		Debug.Log("Player entered: " + newPlayer.NickName);
        
		// Get the index of the new player (0-indexed)
		int playerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;

		// Spawn the new player
		SpawnPlayer(playerIndex);
	}
}
