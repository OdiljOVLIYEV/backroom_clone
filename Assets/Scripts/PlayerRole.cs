using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerRole : MonoBehaviourPun
{
	public string role; // Mafia, Doctor, Komissar, Citizen
	public string playerName; // O'yinchi ismi
	public bool isAlive = true;
	public bool isProtected = false;
	bool hasHealed = false; 

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		// Player spawn qilgandan so'ng
		

	}

	public void UseAbility(GameObject target)
	{
		if (!isAlive)
		{
			Debug.Log($"{playerName} o‘lgani uchun qobiliyat ishlamaydi.");
			return;
		}

		PlayerRole targetRole = target.GetComponent<PlayerRole>();
		if (targetRole == null) return;

		switch (role)
		{
		case "Mafia":
			Debug.Log($"🔫 {playerName} (Mafia) {targetRole.playerName} ni nishonga oldi.");
			targetRole.Kill();
			break;

		case "Doctor":
			Debug.Log($"🛡️ {playerName} (Doctor) {targetRole.playerName} ni himoya qilmoqda.");
			if (hasHealed)
			{
				Debug.Log("Doctor allaqachon davolagan.");
				return;
			}
			
			targetRole.Protect();
			hasHealed = true;
			break;

		case "Komissar":
			Debug.Log($"🔍 {playerName} (Komissar) {targetRole.playerName} roli: {targetRole.role}");
			break;

		case "Citizen":
			Debug.Log($"📛 {playerName} (Citizen) da qobiliyat yo‘q.");
			break;
		}
	}

	public void Protect()
	{
		photonView.RPC("RPC_Protect", RpcTarget.AllBuffered);
	}

	[PunRPC]
	public void RPC_Protect()
	{
		isProtected = true;
		Debug.Log($"{playerName} himoyaga olingan!");
	}


	public void Kill()
	{
		// Bu local funksiyani chaqirish emas, balki RPC orqali barcha clientlarga yuborish kerak
		// Faqat masterclient yoki sender emas, butun tarmoqqa
		photonView.RPC("RPC_DoKill", RpcTarget.AllBuffered);
	}

	[PunRPC]
	public void RPC_DoKill()
	{
		if (isProtected) {
			isProtected = false;
			Debug.Log($"{playerName} himoyalangan.");
			return;
		}

		isAlive = false;
		Debug.Log($"☠️ {playerName} o‘ldirildi.");

		if (photonView.IsMine)
		{
			// Photon CustomProperties orqali ham yangilaymiz
			Hashtable props = new Hashtable
			{
				{ "isAlive", false }
			};
			PhotonNetwork.LocalPlayer.SetCustomProperties(props);
		}

		// UI-ni yangilash
		PlayerListUI ui = FindObjectOfType<PlayerListUI>();
		if (ui != null)
		{
			ui.RefreshPlayerList();
		}
	}


}