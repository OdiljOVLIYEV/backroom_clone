using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerRole : MonoBehaviourPun
{
	public string role;
	public string playerName;
	public bool isAlive = true;
	public bool isProtected = false;

	private bool hasAttacked = false;
	private bool hasHealed = false;
	private bool hasUsedAbilityThisNight = false;

	private void Start()
	{
		if (photonView.IsMine)
		{
			playerName = PhotonNetwork.NickName;
		}
	}

	public void UseAbility(GameObject target, string type)
	{
		if (!isAlive || !PhaseManager.isNight) return;

		PlayerRole targetRole = target.GetComponent<PlayerRole>();
		if (targetRole == null) return;

		switch (role)
		{
		case "Mafia":
			if (type == "kill" && !hasAttacked)
			{
				targetRole.ReceiveKill();
				hasAttacked = true;
			}
			break;

		case "Doctor":
			if (type == "protect" && !hasHealed)
			{
				targetRole.ReceiveProtect();
				hasHealed = true;
			}
			break;

		case "Komissar":
			if (!hasUsedAbilityThisNight)
			{
				if (type == "kill")
				{
					targetRole.ReceiveKill();
					hasUsedAbilityThisNight = true;
				}
				else if (type == "investigate")
				{
					if (photonView.IsMine)
					{
						FindObjectOfType<PlayerListUI>()?.ShowInvestigationResult(targetRole.photonView.Owner, targetRole.role);
					}
					hasUsedAbilityThisNight = true;
				}
			}
			break;
		}
	}

	public void ReceiveProtect()
	{
		photonView.RPC(nameof(RPC_SetProtected), RpcTarget.AllBuffered);
	}

	[PunRPC]
	void RPC_SetProtected()
	{
		isProtected = true;
	}

	public void ReceiveKill()
	{
		photonView.RPC(nameof(RPC_DoKill), RpcTarget.AllBuffered);
	}

	[PunRPC]
	void RPC_DoKill()
	{
		if (isProtected)
		{
			isProtected = false;
			return;
		}

		isAlive = false;
		if (photonView.IsMine)
		{
			PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isAlive", false } });
		}

		FindObjectOfType<PlayerListUI>()?.RefreshPlayerList();
	}

	public void ResetNightAbilities()
	{
		hasAttacked = false;
		hasHealed = false;
		hasUsedAbilityThisNight = false;
		isProtected = false;
	}
}
