using UnityEngine;
using Photon.Pun;

public class PlayerRole : MonoBehaviourPun
{
	public string role; // Mafia, Doctor, Komissar, Citizen
	public string playerName; // O'yinchi ismi
	public bool isAlive = true;
	public bool isProtected = false;

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
			targetRole.Protect();
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
		isProtected = true;
	}

	public void Kill()
	{
		if (isProtected)
		{
			Debug.Log($"🛡️ {playerName} himoyalangan — o‘lmaydi.");
			isProtected = false;
			return;
		}

		isAlive = false;
		Debug.Log($"☠️ {playerName} o‘ldirildi.");
		photonView.RPC("RPC_OnKilled", RpcTarget.All, playerName);
		// UI, animatsiya, disable qilishni bu yerda qo‘shing
		//FindObjectOfType<PlayerListUI>().RefreshPlayerList();
	}
	
	[PunRPC]
	public void RPC_OnKilled(string killedName)
	{
		isAlive = false; // bu playerning o‘zi uchun ham to‘g‘rilanadi

		Debug.Log($"☠️ {killedName} o‘ldirildi (barchaga).");

		PlayerListUI listUI = FindObjectOfType<PlayerListUI>();
		if (listUI != null)
		{
			listUI.RefreshPlayerList();
		}
	}


}