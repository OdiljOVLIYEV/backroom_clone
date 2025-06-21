using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
	[Header("Role Prefabs")]
	public GameObject mafiaPrefab;
	public GameObject doctorPrefab;
	public GameObject komissarPrefab;
	public GameObject citizenPrefab;
	
   
	public Transform[] spawnPoints;
	public int maxplayer;

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(AssignRolesAfterDelay());
		}
	}

	IEnumerator AssignRolesAfterDelay()
	{
		yield return new WaitForSeconds(1f);

		Player[] players = PhotonNetwork.PlayerList;
		List<Player> playerList = new List<Player>(players);

		if (playerList.Count < maxplayer)
		{
			Debug.LogWarning("Kamida 4 ta player kerak!");
			yield break;
		}

		// Shuffle players
		for (int i = 0; i < playerList.Count; i++)
		{
			Player temp = playerList[i];
			int rand = Random.Range(i, playerList.Count);
			playerList[i] = playerList[rand];
			playerList[rand] = temp;
		}

		// 1-mafia, 2-doctor, 3-komissar, qolgan citizen
		photonView.RPC("SpawnPlayerWithRole", playerList[0], "Mafia");
		photonView.RPC("SpawnPlayerWithRole", playerList[1], "Doctor");
		photonView.RPC("SpawnPlayerWithRole", playerList[2], "Komissar");

		for (int i = 3; i < playerList.Count; i++)
		{
			photonView.RPC("SpawnPlayerWithRole", playerList[i], "Citizen");
		}
	}

	[PunRPC]
	private void SpawnPlayerWithRole(string role)
	{
		Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

		switch (role)
		{
		case "Mafia":
			PhotonNetwork.Instantiate(mafiaPrefab.name, spawnPoint.position, Quaternion.identity);
			Debug.Log("Siz MAFIA bo‘ldingiz");
			break;
		case "Doctor":
			PhotonNetwork.Instantiate(doctorPrefab.name, spawnPoint.position, Quaternion.identity);
			Debug.Log("Siz DOCTOR bo‘ldingiz");
			break;
		case "Komissar":
			PhotonNetwork.Instantiate(komissarPrefab.name, spawnPoint.position, Quaternion.identity);
			Debug.Log("Siz KOMISSAR bo‘ldingiz");
			break;
		case "Citizen":
			PhotonNetwork.Instantiate(citizenPrefab.name, spawnPoint.position, Quaternion.identity);
			Debug.Log("Siz FUQARO bo‘ldingiz");
			break;
		}
	}

	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("LobbyScene");
	}

	public void LeaveRoomAndReturnToLobby()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void levelload()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("levelloading", RpcTarget.All);
		}
	}

	[PunRPC]
	private void levelloading()
	{
		PhotonNetwork.LoadLevel("level2");
	}
}
