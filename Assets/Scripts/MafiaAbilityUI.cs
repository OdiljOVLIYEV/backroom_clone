using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
{
	public TMP_Text nameText; // TMP Text obyektini UI dan biriktiring

	void Start()
	{
		if (photonView.IsMine)
		{
			// Bu local player bo‘lsa, ismini o‘zgartiramiz
			string playerName = PhotonNetwork.NickName;
			photonView.RPC("SetName", RpcTarget.AllBuffered, playerName);
		}
	}

	[PunRPC]
	void SetName(string playerName)
	{
		if (nameText != null)
		{
			nameText.text = playerName;
		}
	}
}

