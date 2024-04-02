using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class CameraController : MonoBehaviourPunCallbacks
{
	private List<Transform> players = new List<Transform>();
	private int currentPlayerIndex = 0;
	private CameraRotation cameraRotationScript;

	void Start()
	{
		// Find all players in the scene
		FindPlayersWithTag();

		// Get the CameraRotation script component attached to the camera
		cameraRotationScript = GetComponent<CameraRotation>();

		if (players.Count > 0 && cameraRotationScript != null)
		{
			// Assign the first player found to the CameraRotation script
			cameraRotationScript.player = players[currentPlayerIndex];
			transform.position = players[currentPlayerIndex].position;
		}
	}

	void Update()
	{   FindPlayersWithTag();
		// Check if the current player has left the scene
		if (players.Count == 0)
		{
			// If no players, do nothing
			return;
		}

		// Switch to the next player in the list
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SwitchToNextPlayer();
		}

		// Rotate the camera using CameraRotation script
		if (cameraRotationScript != null)
		{
			// No need to call RotateCamera method
			// CameraRotation script will handle rotation in its Update method
		}
	}

	void SwitchToNextPlayer()
	{
		// Increment index to switch to the next player
		currentPlayerIndex++;
		Debug.Log("Current Player Index: " + currentPlayerIndex);

		// Wrap around to the first player if at the end of the list
		if (currentPlayerIndex >= players.Count)
		{
			currentPlayerIndex = 0;
		}

		// Assign the new player to the CameraRotation script
		if (cameraRotationScript != null)
		{
			cameraRotationScript.player = players[currentPlayerIndex];
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		// Called when a new player enters the room
		Debug.Log("New player entered: " + newPlayer.NickName);

		// Find all players in the scene again
		FindPlayersWithTag();

		// Assign the new player to the CameraRotation script
		if (players.Count > 0 && cameraRotationScript != null)
		{
			currentPlayerIndex = players.Count - 1;
			cameraRotationScript.player = players[currentPlayerIndex];
		}
	}

	void FindPlayersWithTag()
	{
		// Find all players in the scene with the "Player" tag
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
		players.Clear(); // Clear the list before adding again
		foreach (GameObject playerObject in playerObjects)
		{
			if (playerObject != null && playerObject.CompareTag("Player"))
			{
				players.Add(playerObject.transform);
			}
		}
	}
}
