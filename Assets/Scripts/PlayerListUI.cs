using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerListUI : MonoBehaviourPunCallbacks
{
	public GameObject playerItemPrefab;
	public Transform listParent;
	//public TextMeshProUGUI phaseText;

	public float nightDuration = 30f;
	public float dayDuration = 20f;

	private static bool isNight = false;

	private void Start()
	{
		PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable { { "isAlive", true } });

		RefreshPlayerList();

		
			StartCoroutine(GameLoop());
		
	}

	IEnumerator GameLoop()
	{
		yield return new WaitForSeconds(2f);

		while (true)
		{
			// 🌙 TUN BOSHLANDI
			isNight = true;
			//UpdatePhaseText();
			RefreshPlayerList();
			yield return new WaitForSeconds(nightDuration);

			// ☀️ KUN BOSHLANDI
			isNight = false;
			ResetNightAbilities(); // barcha playerlarga
			//UpdatePhaseText();
			RefreshPlayerList();
			yield return new WaitForSeconds(dayDuration);
		}
	}

	/*private void UpdatePhaseText()
	{
		if (phaseText != null)
		{
			if (isNight)
			{
				phaseText.text = "🌙 TUN REJIMI";
				phaseText.color = Color.cyan;
			}
			else
			{
				phaseText.text = "☀️ KUN REJIMI";
				phaseText.color = Color.yellow;
			}
		}
	}*/

	private void ResetNightAbilities()
	{
		foreach (var pr in FindObjectsOfType<PlayerRole>())
		{
			pr.ResetNightAbilities();
		}
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
				Debug.LogError("⚠️ PlayerListItemUI topilmadi.");
				Destroy(item);
				continue;
			}

			ui.nameText.text = player.NickName;

			// === ROLE HOLATI ===
			PlayerRole pr = System.Array.Find(allRoles, r => r.photonView.Owner == player);
			if (pr != null)
			{
				ui.statusText.text = pr.isAlive ? "🟢 TIRIK" : "☠️ O‘LIK";

				if (!pr.isAlive)
				{
					ui.nameText.color = Color.gray;
					ui.roleText.color = Color.gray;
					ui.statusText.color = Color.gray;
				}

				if (player == PhotonNetwork.LocalPlayer)
					ui.roleText.text = myRole.role.ToUpper();
				else if (!pr.isAlive)
				{
					ui.roleText.text = pr.role.ToUpper();
					ui.roleText.color = Color.red;
				}
				else
					ui.roleText.text = "NOMALUM";
			}

			// === BUTTON YASHIRISH ===
			ui.MafiakillButton.gameObject.SetActive(false);
			ui.killButton.gameObject.SetActive(false);
			ui.investigateButton.gameObject.SetActive(false);
			ui.protectButton.gameObject.SetActive(false);

			// === TUNDA KO‘RSATISH ===
			if (isNight && myRole != null && myRole.isAlive && pr != null && pr.isAlive && player != PhotonNetwork.LocalPlayer)
			{
				switch (myRole.role.Trim().ToLower())
				{
					case "mafia":
						ui.MafiakillButton.gameObject.SetActive(true);
						ui.MafiakillButton.onClick.RemoveAllListeners();
						ui.MafiakillButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "kill"));
						break;

					case "komissar":
						ui.killButton.gameObject.SetActive(true);
						ui.killButton.onClick.RemoveAllListeners();
						ui.killButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "kill"));

						ui.investigateButton.gameObject.SetActive(true);
						ui.investigateButton.onClick.RemoveAllListeners();
						ui.investigateButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "investigate"));
						break;

					case "doctor":
						ui.protectButton.gameObject.SetActive(true);
						ui.protectButton.onClick.RemoveAllListeners();
						ui.protectButton.onClick.AddListener(() => myRole.UseAbility(pr.gameObject, "protect"));
						break;
				}
			}

			// === Doctor o‘zini tunda himoya qilsin ===
			if (isNight && myRole != null && myRole.role == "Doctor" && player == PhotonNetwork.LocalPlayer)
			{
				ui.protectButton.gameObject.SetActive(true);
				ui.protectButton.onClick.RemoveAllListeners();
				ui.protectButton.onClick.AddListener(() => myRole.UseAbility(myRole.gameObject, "protect"));
			}
		}
	}

	private PlayerRole GetMyPlayerRole()
	{
		foreach (var r in FindObjectsOfType<PlayerRole>())
			if (r.photonView.IsMine)
				return r;
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

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)

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
