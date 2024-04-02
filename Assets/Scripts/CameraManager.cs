using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviourPunCallbacks
{
	public GameObject[] playerCameras; // Array to hold the camera prefabs
	private List<GameObject> playerObjects = new List<GameObject>(); // List to hold player GameObjects
	private List<Camera> cameras = new List<Camera>(); // List to hold instantiated cameras
	private Vector3 offset = new Vector3(0f, 2f, -3f); // Offset from player position

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (!PhotonNetwork.IsMasterClient) return;

		// If a new player enters the room, instantiate a camera for them
		GameObject newPlayerCamera = PhotonNetwork.Instantiate(playerCameras[0].name, Vector3.zero, Quaternion.identity);
		playerObjects.Add(newPlayerCamera);
		Camera newCamera = newPlayerCamera.GetComponent<Camera>();
		cameras.Add(newCamera);

		// Set the camera target to the new player's transform
		Transform playerTransform = newPlayerCamera.transform.GetChild(0); // Assuming player is the first child
		newCamera.GetComponent<CameraFollow>().SetTarget(playerTransform);

		// Set camera position relative to player
		newCamera.transform.localPosition = offset;
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		if (!PhotonNetwork.IsMasterClient) return;

		int index = playerObjects.FindIndex(player =>
		{
			var cameraFollow = player.GetComponent<CameraFollow>();
			if (cameraFollow != null && cameraFollow.target != null && otherPlayer.TagObject is GameObject)
			{
				return cameraFollow.target == (otherPlayer.TagObject as GameObject).transform;
			}
			return false;
		});

		if (index != -1)
		{
			PhotonNetwork.Destroy(playerObjects[index]);
			playerObjects.RemoveAt(index);
			cameras.RemoveAt(index);
		}
	}

	void Update()
	{
		// Rotate cameras around players
		for (int i = 0; i < cameras.Count; i++)
		{
			if (cameras[i] != null)
			{
				cameras[i].transform.RotateAround(playerObjects[i].transform.position, Vector3.up, 50f * Time.deltaTime);
			}
		}
	}
}
