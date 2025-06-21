using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
	public GameObject playerItemPrefab; // Prefab ichida TMP_Text va Kick Button
	public Transform listParent; // UI Panel ichidagi Content

	private void Start()
	{
		RefreshPlayerList();
	}

	// Barcha o'yinchilar ro'yxatini UIga chiqaradi
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

			TMP_Text[] texts = item.GetComponentsInChildren<TMP_Text>();
			TMP_Text nameText = texts[0];
			TMP_Text roleText = texts[1];
			TMP_Text statusText = texts[2];

			Button killButton = item.GetComponentInChildren<Button>(); // KillButton

			nameText.text = player.NickName;
			roleText.text = "NOMALUM";
			statusText.text = "HOLAT: ?";

			foreach (PlayerRole pr in allRoles)
			{
				if (pr.photonView.Owner == player)
				{
					// Faqat o‘z rolini ko‘rsatadi
					if (player == PhotonNetwork.LocalPlayer)
					{
						roleText.text = pr.role.ToUpper();
					}
					else
					{
						roleText.text = "NOMALUM";
					}

					statusText.text = pr.isAlive ? "🟢 TIRIK" : "☠️ O‘LIK";

					if (!pr.isAlive)
					{
						nameText.color = Color.gray;
						roleText.color = Color.gray;
						statusText.color = Color.gray;
					}

					// 👉 KILL BUTTON LOGIC
					// Faqat mafia va tirik bo‘lsa, boshqalarga tugmani ko‘rsat
					PlayerRole myRole = GetMyPlayerRole();
					if (myRole != null && myRole.role == "Mafia" && myRole.isAlive && pr.isAlive && player != PhotonNetwork.LocalPlayer)
					{
						killButton.gameObject.SetActive(true);
						Debug.Log("SIZ MAFIASIZ");

						// Targetga Kill RPC yuborish
						killButton.onClick.AddListener(() =>
						{
							pr.photonView.RPC("RPC_OnKilled", RpcTarget.All, pr.playerName);
							pr.isAlive = false; // Lokal ravishda holatni yangilaymiz
						});
					}
					else
					{
						Debug.Log("SIZ MAFIASIZ EMASSIZ");
						killButton.gameObject.SetActive(false);
					}

					break;
				}
			}
		}
	}

	// O‘z rolini olish uchun yordamchi funksiya
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





	// Kick funksiyasi faqat masterclientda ishlaydi
	void KickPlayer(int actorNumber)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Player playerToKick = null;
			foreach (var player in PhotonNetwork.PlayerList)
			{
				if (player.ActorNumber == actorNumber)
				{
					playerToKick = player;
					break;
				}
			}

			if (playerToKick != null)
			{
				PhotonNetwork.CloseConnection(playerToKick);
				Debug.Log($"Player {playerToKick.NickName} kicked.");
			}
		}
	}

	// Har doim yangi player kelsa yoki ketsa ro'yxatni yangilaymiz
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshPlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayerList();
	}
}
