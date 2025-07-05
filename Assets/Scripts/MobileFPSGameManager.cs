using System.Collections;
using System.Collections.Generic;
using Obvious.Soap;
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
	public GameObject CAMERA;
	public GameObject UIPANEL;
	public Transform[] spawnPoints;
	public IntVariable MaxPlayer;
	private int maxplayer;
	private int spawnIndex = 0;

	private void Start()
	{
		

		maxplayer = MaxPlayer.Value;
		
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(DelayedCameraHide(2f)); // 3 soniyadan keyin
		}
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(AssignRolesAfterDelay());
			
		}
	}
	private IEnumerator DelayedCameraHide(float delay)
	{
		
		yield return new WaitForSeconds(delay);

		photonView.RPC("HideCamera", RpcTarget.All);
		
		yield return new WaitForSeconds(0.1F);
		
		photonView.RPC("HideUI", RpcTarget.All);
	}
	
	[PunRPC]
	private void HideUI()
	{
		if (UIPANEL != null)
		{
			UIPANEL.SetActive(true);
			Debug.Log("CAMERA o‘chirildi: " + PhotonNetwork.NickName);
		}
	}
	
	[PunRPC]
	private void HideCamera()
	{
		if (CAMERA != null)
		{
			CAMERA.SetActive(false);
			Debug.Log("CAMERA o‘chirildi: " + PhotonNetwork.NickName);
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
		int spawnIndex = 0;
		photonView.RPC("SpawnPlayerWithRole", playerList[0], "Mafia", spawnIndex++);
		photonView.RPC("SpawnPlayerWithRole", playerList[1], "Doctor", spawnIndex++);
		photonView.RPC("SpawnPlayerWithRole", playerList[2], "Komissar", spawnIndex++);

		for (int i = 3; i < playerList.Count; i++)
		{
			photonView.RPC("SpawnPlayerWithRole", playerList[i], "Citizen", spawnIndex++);
		}
	}


	[PunRPC]
	private void SpawnPlayerWithRole(string role, int spawnIndex)
	{
		Transform spawnPoint = spawnPoints[spawnIndex];

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
