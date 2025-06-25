using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhaseManager : MonoBehaviourPun
{
	public static bool isNight = false;

	public float nightDuration = 30f;
	public float dayDuration = 20f;

	private void Start()
	{
		
		
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(GameLoop());
		}
		
	}

	IEnumerator GameLoop()
	{
		yield return new WaitForSeconds(2f);

		while (true)
		{
			// 🌙 TUN
			photonView.RPC(nameof(RPC_StartNight), RpcTarget.All);
			yield return StartCoroutine(RunTimer(nightDuration));
			photonView.RPC(nameof(RPC_EndNight), RpcTarget.All);

			// ☀️ KUN
			yield return StartCoroutine(RunTimer(dayDuration));
		}
	}

	IEnumerator RunTimer(float seconds)
	{
		float timer = seconds;
		while (timer > 0)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				photonView.RPC(nameof(RPC_LogMessage), RpcTarget.All, $"⏱️ {Mathf.Ceil(timer)} soniya qoldi.");
			}
			yield return new WaitForSeconds(1f);
			timer -= 1f;
		}
	}

	[PunRPC]
	void RPC_StartNight()
	{
		PhaseManager.isNight = true;
		StartCoroutine(DelayedRefreshUI());
	}

	[PunRPC]
	void RPC_EndNight()
	{
		PhaseManager.isNight = false;

		// 🔁 Barcha o'yinchilarni reset qilish
		foreach (var role in FindObjectsOfType<PlayerRole>())
		{
			role.ResetNightAbilities();  // Bu allaqachon sizda mavjud
		}
		

		StartCoroutine(DelayedRefreshUI());
	}



	IEnumerator DelayedRefreshUI()
	{
		yield return new WaitForSeconds(0.1f);
		var ui = FindObjectOfType<PlayerListUI>();
		if (ui != null) ui.RefreshPlayerList(); // <== MUHIM
	}


	[PunRPC]
	public void RPC_LogMessage(string message)
	{
		Debug.Log(message);
	}

	public void PrintAlivePlayersCount()
	{
		int alive = 0;
		foreach (var pr in FindObjectsOfType<PlayerRole>())
		{
			if (pr.isAlive) alive++;
		}
		Debug.Log("📊 Tiriklik soni: " + alive);
	}
}
