using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;  // PhotonNetwork uchun kerak

public class PhaseManager : MonoBehaviourPun
{
	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(PrintAliveCountRoutine());
		}
	}

	IEnumerator PrintAliveCountRoutine()
	{
		while (true)
		{
			PrintAlivePlayersCount();
			yield return new WaitForSeconds(5f);
		}
	}

	public void PrintAlivePlayersCount()
	{
		PlayerRole[] allPlayers = FindObjectsOfType<PlayerRole>();
		int aliveCount = 0;

		foreach (PlayerRole pr in allPlayers)
		{
			if (pr.isAlive)
				aliveCount++;
		}

		Debug.Log("Hozircha tirik o'yinchilar soni: " + aliveCount);
	}

	public void StartNightPhase()
	{
		// Har bir rol o‘z harakatini qiladi
		Debug.Log("Night phase started — mafia, doctor, komissar harakat qiladi.");
	}

	public void EndNightPhase()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(PrintAliveCountRoutine());
		}
		// Natijalarni qayta ishlash
		Debug.Log("Night phase ended.");
	}

	public void StartVotingPhase()
	{
		// Ovozlar ochiladi
		Debug.Log("Voting started — kimdir chiqariladi.");
	}

	public void EndVotingPhase()
	{
		// Eng ko‘p ovoz olgan o‘yinchi chiqariladi
		Debug.Log("Voting ended.");
	}
}