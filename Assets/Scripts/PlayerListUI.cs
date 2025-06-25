using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
	public GameObject playerItemPrefab;
	public Transform listParent;

	private void Start()
	{
		PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isAlive", true } });
		RefreshPlayerList();
	}

	public void RefreshPlayerList()
	{
		foreach (Transform child in listParent)
			Destroy(child.gameObject);

		PlayerRole[] allRoles = FindObjectsOfType<PlayerRole>();
		PlayerRole myRole = GetMyPlayerRole();

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			GameObject item = Instantiate(playerItemPrefab, listParent);
			PlayerListItemUI ui = item.GetComponent<PlayerListItemUI>();

			if (ui == null)
			{
				Debug.LogError("⚠️ PlayerListItemUI component topilmadi.");
				Destroy(item);
				continue;
			}

			ui.nameText.text = player.NickName;

			// === ROLE KO‘RINISHI QOIDASI ===
			PlayerRole pr = System.Array.Find(allRoles, r => r.photonView.Owner == player);
			if (pr != null)
			{
				// TIRIKLIK HOLATI
				ui.statusText.text = pr.isAlive ? "🟢 TIRIK" : "☠️ O‘LIK";

				if (!pr.isAlive)
				{
					ui.nameText.color = Color.gray;
					ui.roleText.color = Color.gray;
					ui.statusText.color = Color.gray;
				}

				// === ROLNI KO‘RSATISH QOIDASI ===
				if (player == PhotonNetwork.LocalPlayer)
				{
					ui.roleText.text = myRole.role.ToUpper(); // o‘zingiz uchun har doim
				}
				else if (!pr.isAlive)
				{
					ui.roleText.text = pr.role.ToUpper(); // o‘lgach — rol ochiladi
					ui.roleText.color = Color.red;
				}
				else
				{
					ui.roleText.text = "NOMALUM"; // tiriklar yashirin
					ui.roleText.color = Color.white;
				}
			}

			
			// === QOBILIYATLAR BUTTONLARI ===
			ui.killButton.gameObject.SetActive(false);
			ui.investigateButton.gameObject.SetActive(false);
			ui.protectButton.gameObject.SetActive(false);

			// ➤ Faqat tunda tugmalar ko‘rinadi
			if (PhaseManager.isNight && myRole != null && myRole.isAlive && pr != null && pr.isAlive && player != PhotonNetwork.LocalPlayer)
			{
				if (myRole.role == "Mafia")
				{
					ui.killButton.gameObject.SetActive(true);
					ui.killButton.onClick.RemoveAllListeners();
					ui.killButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "kill"));
				}
				else if (myRole.role == "Komissar")
				{
					ui.killButton.gameObject.SetActive(true);
					ui.killButton.onClick.RemoveAllListeners();
					ui.killButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "kill"));

					ui.investigateButton.gameObject.SetActive(true);
					ui.investigateButton.onClick.RemoveAllListeners();
					ui.investigateButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "investigate"));
				}
				else if (myRole.role == "Doctor")
				{
					ui.protectButton.gameObject.SetActive(true);
					ui.protectButton.onClick.RemoveAllListeners();
					ui.protectButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "protect"));
				}
			}

			// ➤ Doctor o‘zini faqat tunda himoya qilsin
			if (PhaseManager.isNight && myRole != null && myRole.role == "Doctor" && player == PhotonNetwork.LocalPlayer)
			{
				ui.protectButton.gameObject.SetActive(true);
				ui.protectButton.onClick.RemoveAllListeners();
				ui.protectButton.onClick.AddListener(() => myRole.UseAbility(myRole.gameObject, "protect"));
			}

		}
	}


	private PlayerRole GetMyPlayerRole()
	{
		PlayerRole[] roles = FindObjectsOfType<PlayerRole>();
		foreach (var r in roles)
		{
			if (r.photonView.IsMine)
				return r;
		}
		return null;
	}
	
	public void ShowInvestigationResult(Player investigatedPlayer, string revealedRole)
	{
		foreach (Transform child in listParent)
		{
			PlayerListItemUI ui = child.GetComponent<PlayerListItemUI>();
			if (ui == null) continue;

			if (ui.nameText.text == investigatedPlayer.NickName)
			{
				PlayerRole myRole = GetMyPlayerRole();
				if (myRole != null && myRole.role == "Komissar" && myRole.photonView.IsMine)
				{
					ui.roleText.text = revealedRole.ToUpper();
					ui.roleText.color = Color.yellow;
				}
				break;
			}
		}
	}







	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		RefreshPlayerList();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshPlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayerList();
	}
}
