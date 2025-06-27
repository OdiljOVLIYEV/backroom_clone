using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Text name;
    public bool enableNight = true;
    public bool enableDay = true;

    public float nightDuration = 30f;
    public float dayDuration = 20f;

    private static bool isNight = false;
    private bool hasVoted = false;
    private Dictionary<string, int> voteCounts = new Dictionary<string, int>();
    private Dictionary<string, string> voteLogs = new Dictionary<string, string>(); // voter -> voted

    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable { { "isAlive", true } });
        RefreshPlayerList();

        if (name != null)
        {
            name.text = PhotonNetwork.LocalPlayer.NickName;
        }
        
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            // 🌙 TUN
            if (enableNight)
            {
                isNight = true;
                hasVoted = false;
                RefreshPlayerList();
                yield return new WaitForSeconds(nightDuration);
            }
            else
            {
                yield return null; // agar tun o‘chirilgan bo‘lsa kutadi
            }

            // ☀️ KUN
            if (enableDay)
            {
                isNight = false;
                hasVoted = false;
                voteCounts.Clear();
                voteLogs.Clear();
                ResetNightAbilities();
                RefreshPlayerList();
                yield return new WaitForSeconds(dayDuration);

                // 📊 Ovozlar
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("📊 Ovoz natijalari:");
                    foreach (var kvp in voteCounts)
                    {
                        var player = PhotonNetwork.CurrentRoom.GetPlayer(int.Parse(kvp.Key));
                        string name = player != null ? player.NickName : "???";
                        Debug.Log($"➡️ {name}: {kvp.Value} ovoz");
                    }

                    Player mostVoted = GetMostVotedPlayer();
                    if (mostVoted != null)
                    {
                        photonView.RPC(nameof(EliminatePlayer), RpcTarget.All, mostVoted.ActorNumber);
                    }
                    else
                    {
                        Debug.Log("❌ Hech kim o‘ldirilmaydi (teng yoki yo‘q ovoz).");
                    }
                }
            }
            else
            {
                yield return null; // agar kun o‘chirilgan bo‘lsa kutadi
            }
        }
    }



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
            if (ui == null) continue;

            ui.nameText.text = player.NickName;
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

            if (!isNight && voteCounts.TryGetValue(player.ActorNumber.ToString(), out int votes))
            {
                ui.statusText.text += $" | 🗳️ {votes} ovoz";
            }

            ui.MafiakillButton.gameObject.SetActive(false);
            ui.killButton.gameObject.SetActive(false);
            ui.investigateButton.gameObject.SetActive(false);
            ui.protectButton.gameObject.SetActive(false);
            ui.voteButton.gameObject.SetActive(false);

            if (isNight && myRole != null && myRole.isAlive && pr != null && pr.isAlive && player != PhotonNetwork.LocalPlayer)
            {
                switch (myRole.role.ToLower())
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

            // Doctor o‘zini faqat tunda himoya qiladi
            if (isNight && myRole != null && myRole.role == "Doctor" && player == PhotonNetwork.LocalPlayer)
            {
                ui.protectButton.gameObject.SetActive(true);
                ui.protectButton.onClick.RemoveAllListeners();
                ui.protectButton.onClick.AddListener(() => myRole.UseAbility(myRole.gameObject, "protect"));
            }

            // KUN REJIMI OVOZ BERISH
            if (!isNight && myRole != null && myRole.isAlive && pr != null && pr.isAlive && player != PhotonNetwork.LocalPlayer)
            {
                ui.voteButton.gameObject.SetActive(true);
                ui.voteButton.onClick.RemoveAllListeners();
                ui.voteButton.onClick.AddListener(() => CastVote(player));
            }
        }

        // Debug ovoz loglari
        if (!isNight && voteLogs.Count > 0)
        {
            Debug.Log("\n📋 Ovozlar:");
            foreach (var log in voteLogs)
            {
                Debug.Log($"🗳️ {log.Key} ➡️ {log.Value}");
            }
        }
    }

    void CastVote(Player votedPlayer)
    {
        if (hasVoted || votedPlayer == null) return;

        // ❗ Faqat tiriklar ovoz berishi va tiriklarga ovoz berilishi kerak
        var myRole = GetMyPlayerRole();
        var targetRole = FindObjectsOfType<PlayerRole>().FirstOrDefault(r => r.photonView.Owner == votedPlayer);

        if (myRole == null || !myRole.isAlive || targetRole == null || !targetRole.isAlive) return;

        hasVoted = true;

        string votedKey = votedPlayer.ActorNumber.ToString();
        string voterName = PhotonNetwork.LocalPlayer.NickName;
        string votedName = votedPlayer.NickName;

        Debug.Log($"🗳️ {voterName} ovoz berdi ➡️ {votedName}");

        photonView.RPC(nameof(RegisterVote), RpcTarget.MasterClient, votedKey, voterName, votedName);
    }



    [PunRPC]
    void RegisterVote(string votedActorNumberStr, string voterName, string votedName)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("❌ RegisterVote faqat MasterClientda ishlaydi.");
            return;
        }

        if (string.IsNullOrEmpty(votedActorNumberStr))
        {
            Debug.LogError("❌ RegisterVote: votedActorNumberStr bo‘sh.");
            return;
        }

        if (!voteCounts.ContainsKey(votedActorNumberStr))
            voteCounts[votedActorNumberStr] = 0;

        voteCounts[votedActorNumberStr]++;

        voteLogs[voterName] = votedName;

        Debug.Log($"✅ MasterClient: {voterName} ➡️ {votedName} ga ovoz berdi. Jami: {voteCounts[votedActorNumberStr]}");
    }

    Player GetMostVotedPlayer()
    {
        if (voteCounts.Count == 0) return null;

        int maxVotes = voteCounts.Values.Max();
        var topVoted = voteCounts.Where(kvp => kvp.Value == maxVotes).ToList();

        if (topVoted.Count > 1)
        {
            Debug.Log("⚠️ Ovozlar teng! Hech kim o‘ldirilmaydi.");
            return null;
        }

        if (int.TryParse(topVoted[0].Key, out int actorNumber))
        {
            return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        }

        return null;
    }


    [PunRPC]
    void EliminatePlayer(int actorNumber)
    {
        foreach (var pr in FindObjectsOfType<PlayerRole>())
        {
            if (pr.photonView.OwnerActorNr == actorNumber)
            {
                pr.isAlive = false;
                Debug.Log($"❌ {pr.photonView.Owner.NickName} osildi.");
                break;
            }
        }

        RefreshPlayerList();
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