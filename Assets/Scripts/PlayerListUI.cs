using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
	public GameObject playerItemPrefab; // Prefab ichida PlayerListItemUI component
	public Transform listParent; // UI Panel ichidagi Content

	private void Start()
	{
		PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isAlive", true } });
		RefreshPlayerList();
	}

	public void RefreshPlayerList()
	{
		foreach (Transform child in listParent)
		{
			Destroy(child.gameObject);
		}

		PlayerRole[] allRoles = FindObjectsOfType<PlayerRole>();

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			GameObject item = Instantiate(playerItemPrefab, listParent);

			PlayerListItemUI ui = item.GetComponent<PlayerListItemUI>();

			if (ui == null)
			{
				Debug.LogError("⚠️ PlayerListItemUI component topilmadi! Prefabga qo‘shishni unutmang.");
				Destroy(item);
				continue;
			}

			ui.nameText.text = player.NickName;
			ui.roleText.text = "NOMALUM";
			//ui.statusText.text = "HOLAT: ?";

			foreach (PlayerRole pr in allRoles)
			{
				if (pr.photonView.Owner == player)
				{
					// O'z rolini ko'rsatadi
					if (player == PhotonNetwork.LocalPlayer)
						ui.roleText.text = pr.role.ToUpper();
					else
						ui.roleText.text = "NOMALUM";

					ui.statusText.text = pr.isAlive ? "🟢 TIRIK" : "☠️ O‘LIK";

					if (!pr.isAlive)
					{
						ui.nameText.color = Color.gray;
						ui.roleText.color = Color.gray;
						ui.statusText.color = Color.gray;
					}

					// ... mavjud kod ichida:
					PlayerRole myRole = GetMyPlayerRole();

					if (myRole != null && myRole.role == "Mafia" && myRole.isAlive && pr.isAlive && player != PhotonNetwork.LocalPlayer)
					{
						ui.killButton.gameObject.SetActive(true);
						ui.killButton.onClick.RemoveAllListeners();
						ui.killButton.onClick.AddListener(() => pr.Kill());
					}
					else
					{
						ui.killButton.gameObject.SetActive(false);
					}

					// 👇 👇 👇 ✅ DOCTOR uchun HEAL BUTTON logic
					// DOCTOR uchun HEAL BUTTON logic
					if (myRole != null && myRole.role == "Doctor" && myRole.isAlive && pr.isAlive)
					{
						ui.healButton.gameObject.SetActive(true);
						ui.healButton.onClick.RemoveAllListeners();
						ui.healButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject));
					}
					else
					{
						ui.healButton.gameObject.SetActive(false);
					}


					break;
				}
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
