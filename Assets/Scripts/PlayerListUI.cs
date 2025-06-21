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

			TMP_Text nameText = texts[0];   // PlayerNameText
			TMP_Text roleText = texts[1];   // RoleText
			TMP_Text statusText = texts[2]; // StatusText (Tirik/O‘lik)

			nameText.text = player.NickName;
			roleText.text = "NOMALUM";
			statusText.text = "HOLAT: ?";

			foreach (PlayerRole pr in allRoles)
			{
				if (pr.photonView.Owner == player)
				{
					// ROL: Faqat o‘ziga ko‘rsatiladi
					if (player == PhotonNetwork.LocalPlayer)
					{
						roleText.text = pr.role.ToUpper(); // O‘z rolini ko‘rsatadi
					}
					else
					{
						roleText.text = "NOMALUM"; // Boshqalar yashirin
					}

					// HOLAT: Hamma uchun ko‘rsatiladi
					statusText.text = pr.isAlive ? "🟢 TIRIK" : "☠️ O‘LIK";

					// Agar o‘lgan bo‘lsa, rangini kulga o‘zgartirish
					if (!pr.isAlive)
					{
						nameText.color = Color.gray;
						roleText.color = Color.gray;
						statusText.color = Color.gray;
					}
					break;
				}
			}
		}
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
