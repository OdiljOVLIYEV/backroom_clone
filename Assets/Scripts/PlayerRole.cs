using System;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class PlayerRole : MonoBehaviourPun
{
    public string role;
    public string playerName;
    public Text nameText;
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

            // Ismni hammaga yuboramiz
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props["playerName"] = playerName;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        UpdateNameUI();
    }

    private void Update()
    {
        // Agar ism hali olinmagan bo‘lsa, custom property orqali olamiz
        if (string.IsNullOrEmpty(playerName) && photonView.Owner.CustomProperties.ContainsKey("playerName"))
        {
            playerName = photonView.Owner.CustomProperties["playerName"].ToString();
            UpdateNameUI();
        }
    }

    private void UpdateNameUI()
    {
        if (nameText != null && !string.IsNullOrEmpty(playerName))
        {
            nameText.text = playerName;
        }
    }
    public void UseAbility(GameObject target, string type)
{
    if (!isAlive) return;

    PlayerRole targetRole = target.GetComponent<PlayerRole>();
    if (targetRole == null) return;

    var playerListUI = FindObjectOfType<PlayerListUI>();

    switch (role)
    {
        case "Mafia":
            if (type == "kill" && !hasAttacked)
            {
                photonView.RPC("MafiaMessage", RpcTarget.All);
                // ❗ KILL ENDI PENDINGDA QO‘SHILADI
                PhotonView playerListView = playerListUI.GetComponent<PhotonView>();
                playerListView.RPC("RPC_AddPendingKill", RpcTarget.MasterClient, targetRole.photonView.OwnerActorNr);

                hasAttacked = true;
            }
            break;

        case "Doctor":
            if (type == "protect" && !hasHealed)
            {
                photonView.RPC("DoctorMessage", RpcTarget.All);
                PhotonView playerListView = playerListUI.GetComponent<PhotonView>();
                playerListView.RPC("RPC_AddPendingSave", RpcTarget.MasterClient, targetRole.photonView.OwnerActorNr);

                hasHealed = true;
            }
            break;

        case "Komissar":
            if (!hasUsedAbilityThisNight)
            {
                if (type == "kill")
                {
                    photonView.RPC("KomissarKillMessage", RpcTarget.All);
                    // Kill qilmang – pending qiling
                    PhotonView playerListView = playerListUI.GetComponent<PhotonView>();
                    playerListView.RPC("RPC_AddPendingKill", RpcTarget.MasterClient, targetRole.photonView.OwnerActorNr);
                }
                else if (type == "investigate")
                {
                    photonView.RPC("KomissarInvestigateMessage", RpcTarget.All);
                    if (photonView.IsMine)
                        playerListUI.ShowInvestigationResult(targetRole.photonView.Owner, targetRole.role);
                }

                hasUsedAbilityThisNight = true;
            }
            break;
    }
}


   
  

   
  

   
    public void ReceiveProtect()
    {
        isProtected = true;
        Debug.Log($"🛡️ {playerName} doctor tomonidan himoyalandi.");
    }



    [PunRPC]
    void RPC_SetProtected()
    {
        isProtected = true;
        Debug.Log($"[RPC] 🛡️ {playerName} ga himoya status berildi.");
    }

    public void ReceiveKill()
    {
        
        Debug.Log($"[{role}] ☠️ {playerName} ga hujum qilinmoqda...");
        photonView.RPC(nameof(RPC_DoKill), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_DoKill()
    {
        if (isProtected)
        {
            Debug.Log($"[RPC] 🛡️ {playerName} himoyalangani sababli o‘lmagan.");
            isProtected = false;
            return;
        }

        isAlive = false;
        Debug.Log($"[RPC] ❌ {playerName} o‘ldi.");

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
        // ❌ isProtected ni bu yerda reset QILMAYMIZ!
    }

    
    [PunRPC]
    public void MafiaMessage()
    {
        if (PhotonNetwork.IsMasterClient) // faqat xonadorga ko‘rsatiladi
            MessageDisplayer.Instance.ShowMessageToAll("mafiya nishonga oldi");
    }


    [PunRPC]
    public void DoctorMessage()
    {
        if (PhotonNetwork.IsMasterClient)
            MessageDisplayer.Instance.ShowMessageToAll("doctor himoya qildi");
    }

    [PunRPC]
    public void KomissarKillMessage()
    {
        if (PhotonNetwork.IsMasterClient)
            MessageDisplayer.Instance.ShowMessageToAll("komissar pistoletni o`qladi");
    }

    [PunRPC]
    public void KomissarInvestigateMessage()
    {
        if (PhotonNetwork.IsMasterClient)
            MessageDisplayer.Instance.ShowMessageToAll("komissar tergovda");
    }


    
   
}
