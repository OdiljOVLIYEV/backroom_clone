using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
    public GameObject playerItemPrefab;
    public Transform listParent;
    public TMP_Text timerText;
    public Image nightImage;
    public Image dayImage;
    public IntVariable MaxPlayer;
    public int requiredPlayerCount ;
     // 🔹 Yangi prefab: har bir xabar uchun

    public bool enableNight = true;
    public bool enableDay = true;
    [SerializeField] private BoolVariable CAMERALOCK;
    
    public float nightDuration = 30f;
    public float dayDuration = 20f;
    private bool gameStarted = false;

    private static bool isNight = false;
    private bool hasVoted = false;
    private Dictionary<string, int> voteCounts = new Dictionary<string, int>();
    private Dictionary<string, string> voteLogs = new Dictionary<string, string>(); // voter -> voted

    private List<Player> pendingKills = new List<Player>();
    private List<Player> pendingSaves = new List<Player>();
    private List<string> nightEvents = new List<string>();
    private void Start()
    {
        requiredPlayerCount = MaxPlayer.Value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable { { "isAlive", true } });
        RefreshPlayerList();
        

        if (!photonView.IsMine)
        {
            timerText.gameObject.SetActive(false);
            nightImage.gameObject.SetActive(false);
            dayImage.gameObject.SetActive(false);
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitForAllPlayersThenStart()); // faqat master kutadi
        }
        
    }

    IEnumerator WaitForAllPlayersThenStart()
    {
        Debug.Log("⏳ Barcha o‘yinchilar kelishini kutyapmiz...");

        while (PhotonNetwork.CurrentRoom.PlayerCount < requiredPlayerCount)
        {
            timerText.text = $"Kutilyapti: {PhotonNetwork.CurrentRoom.PlayerCount}/{requiredPlayerCount}";
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("✅ Barcha o‘yinchilar keldi. O‘yin boshlanyapti.");

        photonView.RPC(nameof(RPC_StartGame), RpcTarget.All);
    }
    [PunRPC]
    void RPC_StartGame()
    {
        StartCoroutine(GameLoop());
    }


    IEnumerator GameLoop()
    {
        
        
        yield return new WaitForSeconds(2f);
        gameStarted = true;
        while (true)
        {
            // 🌙 TUN
            // 🌙 TUN
            if (enableNight)
            {
                CAMERALOCK.Value = false;
                isNight = true;
                hasVoted = false;
                nightEvents.Clear();
                pendingKills.Clear();
                pendingSaves.Clear();

                if (photonView.IsMine && MessageDisplayer.Instance != null)
                {
                    MessageDisplayer.Instance.ClearMessagesForAll();
                }

                // 🔹 UI-ni yangilash
                if (photonView.IsMine)
                {
                    nightImage.gameObject.SetActive(true);
                    dayImage.gameObject.SetActive(false);
                }
                
                RefreshPlayerList();

                float timeLeft = nightDuration;
                while (timeLeft > 0f)
                {
                    timerText.text = Mathf.CeilToInt(timeLeft).ToString("00");
                    yield return new WaitForSeconds(1f);
                    timeLeft -= 1f;
                }
            }


            // ☀️ KUN
            if (enableDay)
            {
                CAMERALOCK.Value = true;
                isNight = false;
                hasVoted = false;
                voteCounts.Clear();
                voteLogs.Clear();
                ResetNightAbilities();

                if (photonView.IsMine && MessageDisplayer.Instance != null)
                {
                    MessageDisplayer.Instance.ClearMessagesForAll();
                }

                if (photonView.IsMine)
                {
                    nightImage.gameObject.SetActive(false);
                    dayImage.gameObject.SetActive(true);
                }

                yield return StartCoroutine(ShowNightResultsCoroutine());

                float timeLeft = dayDuration;
                while (timeLeft > 0f)
                {
                    timerText.text = Mathf.CeilToInt(timeLeft).ToString("00");
                    yield return new WaitForSeconds(1f);
                    timeLeft -= 1f;
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    Player mostVoted = GetMostVotedPlayer();
                    if (mostVoted != null)
                    {
                        photonView.RPC(nameof(EliminatePlayer), RpcTarget.All, mostVoted.ActorNumber);
                    }
                }
            }



            yield return null;
        }
    }





    IEnumerator ShowNightResultsCoroutine()
    {
        PlayerRole[] allRoles = FindObjectsOfType<PlayerRole>();
        Debug.Log("📜 [TUN NATIJALARI] PendingKill lar:");
        foreach (Player target in pendingKills)
        {
            PlayerRole role = allRoles.FirstOrDefault(r => r.photonView.OwnerActorNr == target.ActorNumber);
            if (role == null) continue;

            bool isSaved = pendingSaves.Any(p => p.ActorNumber == target.ActorNumber);

            if (isSaved)
            {
                role.ReceiveProtect();
                MessageDisplayer.Instance?.ShowMessageToAll($"🛡️ {target.NickName} doctor tomonidan saqlandi!");
            }
            else
            {
                role.ReceiveKill();
                MessageDisplayer.Instance?.ShowMessageToAll($"☠️ {target.NickName} o‘ldirildi!");
            }
        }

        // 📜 Qo‘shimcha tun xabarlari
        foreach (var msg in nightEvents)
        {
            MessageDisplayer.Instance?.ShowMessageToAll(msg);
        }

        // ⏳ Bir soniya kutish
        yield return new WaitForSeconds(1f);

        // 👮 KOMISSAR UCHUN TERGOV NATIJALARI
        PlayerRole myRole = GetMyPlayerRole();
        if (myRole != null && myRole.role == "Komissar" && myRole.investigatedActorNumbers.Count > 0)
        {
            foreach (var actorNum in myRole.investigatedActorNumbers)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNum);
                if (player == null) continue;

                PlayerRole targetRole = allRoles.FirstOrDefault(r => r.photonView.OwnerActorNr == actorNum);
                if (targetRole == null) continue;

                // Natijani faqat Komissar ko‘radi
                ShowInvestigationResult(player, targetRole.role);
            }
        }

        RefreshPlayerList();

        // 🔄 Tozalash
        pendingKills.Clear();
        pendingSaves.Clear();
        nightEvents.Clear();

        if (PhotonNetwork.IsMasterClient)
        {
            CheckForGameEnd();
        }
    }








   



    

    public void AddNightEvent(string msg)
    {
        nightEvents.Add(msg);
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

        Player[] allPlayers = PhotonNetwork.PlayerList;
        PlayerRole[] allRoles = FindObjectsOfType<PlayerRole>();
        PlayerRole myRole = GetMyPlayerRole();

        foreach (Player player in allPlayers)
        {
            GameObject item = Instantiate(playerItemPrefab, listParent);
            PlayerListItemUI ui = item.GetComponent<PlayerListItemUI>();
            if (ui == null) continue;

            if (player.CustomProperties.TryGetValue("playerName", out object nameObj))
            {
                ui.nameText.text = nameObj.ToString();
            }
            else
            {
                ui.nameText.text = player.NickName;
            }

            PlayerRole pr = allRoles.FirstOrDefault(r => r.photonView.OwnerActorNr == player.ActorNumber);

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
                    ui.roleText.text = myRole?.role.ToUpper();
                else if (!pr.isAlive)
                {
                    ui.roleText.text = pr.role.ToUpper();
                    ui.roleText.color = Color.red;
                }
                else if (myRole != null && myRole.role == "Komissar" && myRole.investigatedActorNumbers.Contains(player.ActorNumber))
                {
                    ui.roleText.text = pr.role.ToUpper();
                    ui.roleText.color = Color.yellow;
                }
                else
                {
                    ui.roleText.text = "NOMALUM";
                }

            }
            else
            {
                Debug.LogWarning($"⚠️ PlayerRole topilmadi: {player.NickName} | {player.ActorNumber}");
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
            if (isNight && myRole != null && myRole.isAlive && myRole.role == "Doctor" && player == PhotonNetwork.LocalPlayer)
            {
                ui.protectButton.gameObject.SetActive(true);
                ui.protectButton.onClick.RemoveAllListeners();
                ui.protectButton.onClick.AddListener(() => myRole.UseAbility(myRole.gameObject, "protect"));
            }

            // KUN REJIMI OVOZ BERISH
            // ✅ Ovoz berish tugmasini faqat o‘yin boshlangan bo‘lsa va kun bo‘lsa ko‘rsatamiz
            if (gameStarted && !isNight && enableDay && myRole != null && myRole.isAlive && pr != null && pr.isAlive && player != PhotonNetwork.LocalPlayer)


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
    private void CheckForGameEnd()
    {
        var allRoles = FindObjectsOfType<PlayerRole>();
        int mafiaAlive = allRoles.Count(r => r.role.ToLower() == "mafia" && r.isAlive);
        int othersAlive = allRoles.Count(r => r.role.ToLower() != "mafia" && r.isAlive);

        if (mafiaAlive == 0)
        {
            StopAllCoroutines();
            timerText.gameObject.SetActive(false);
            nightImage.gameObject.SetActive(false);
            dayImage.gameObject.SetActive(false);
            GameObject item = Instantiate(playerItemPrefab, listParent);
            PlayerListItemUI ui = item.GetComponent<PlayerListItemUI>();
            if (ui == null) ui.MafiakillButton.gameObject.SetActive(false);
            ui.killButton.gameObject.SetActive(false);
            ui.investigateButton.gameObject.SetActive(false);
            ui.protectButton.gameObject.SetActive(false);
            ui.voteButton.gameObject.SetActive(false);
            
           
            Debug.Log("Shahar g‘alaba qildi!");
            if (MessageDisplayer.Instance != null)
            {
                MessageDisplayer.Instance.ShowWinnerMessageToAll("Shahar g‘alaba qildi!", Color.green);
            }
           
            StartCoroutine(LoadSceneAfterDelay("LobbyScene", 5f));// O‘yin to‘xtaydi
        }
        else if (mafiaAlive >= othersAlive)
        {   
            StopAllCoroutines();
            timerText.gameObject.SetActive(false);
            nightImage.gameObject.SetActive(false);
            dayImage.gameObject.SetActive(false);
            GameObject item = Instantiate(playerItemPrefab, listParent);
            PlayerListItemUI ui = item.GetComponent<PlayerListItemUI>();
            if (ui == null) ui.MafiakillButton.gameObject.SetActive(false);
            ui.killButton.gameObject.SetActive(false);
            ui.investigateButton.gameObject.SetActive(false);
            ui.protectButton.gameObject.SetActive(false);
            ui.voteButton.gameObject.SetActive(false);
            
            if (MessageDisplayer.Instance != null)
            {
                MessageDisplayer.Instance.ShowWinnerMessageToAll("Mafiya g‘alaba qildi!", Color.red);
            }
           
            StartCoroutine(LoadSceneAfterDelay("LobbyScene", 5f));// O‘yin to‘xtaydi
        }
    }
    
    

    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public Player GetPhotonPlayerByName(string name)
    {
        foreach (var p in PhotonNetwork.PlayerList)
            if (p.NickName == name)
                return p;
        return null;
    }
    void CastVote(Player votedPlayer)
    {
        if (hasVoted || votedPlayer == null) return;

        var myRole = GetMyPlayerRole();
        var targetRole = FindObjectsOfType<PlayerRole>().FirstOrDefault(r => r.photonView.Owner == votedPlayer);

        if (myRole == null || !myRole.isAlive || targetRole == null || !targetRole.isAlive) return;

        hasVoted = true;

        string votedKey = votedPlayer.ActorNumber.ToString();
        string voterName = PhotonNetwork.LocalPlayer.NickName;
        string votedName = votedPlayer.NickName;

        Debug.Log($"🗳️ {voterName} ovoz berdi ➡️ {votedName}");

        // ✅ Barcha o‘yinchilarga xabar ko‘rsatish
        if (MessageDisplayer.Instance != null)
        {
            MessageDisplayer.Instance.ShowMessageToAll($"🗳️ {voterName} ➡️ {votedName} ga ovoz berdi.");
        }

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
                Debug.Log($"[EliminatePlayer] ❌ {pr.photonView.Owner.NickName} o‘ldirildi va status o‘zgartirildi.");
                break;
            }
        }

        RefreshPlayerList();

        if (PhotonNetwork.IsMasterClient)
            CheckForGameEnd(); // ✅ BU YERGA QO‘SHILADI
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

    [PunRPC]
    public void RPC_AddPendingKill(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p != null && !pendingKills.Contains(p))
        {
            pendingKills.Add(p);
            Debug.Log($"[RPC_AddPendingKill] ☠️ {p.NickName} pending kill listga qo‘shildi.");
        }
    }


    [PunRPC]
    public void RPC_AddPendingSave(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        if (p != null && !pendingSaves.Contains(p))
        {
            pendingSaves.Add(p);
            Debug.Log($"✅ [Doctor Save] 🛡️ {p.NickName} saqlanmoqda");
        }
        else
        {
            Debug.LogWarning($"❌ [Doctor Save] topilmadi yoki allaqachon saqlangan: {actorNumber}");
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