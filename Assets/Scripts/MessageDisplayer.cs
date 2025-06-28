using UnityEngine;
using TMPro;
using Photon.Pun; // <--- ADD THIS

public class MessageDisplayer : MonoBehaviourPun
{
    public static MessageDisplayer Instance;

    [Header("UI komponentlar")]
    public Transform messageListParent;
    public GameObject messageItemPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ShowMessageToAll(string message)
    {
        photonView.RPC(nameof(RPC_ShowMessage), RpcTarget.All, message);
    }

    [PunRPC]
    public void RPC_ShowMessage(string message)
    {
        if (messageListParent == null || messageItemPrefab == null)
        {
            Debug.LogWarning("[MessageDisplayer] ❌ UI komponentlar yo‘q.");
            return;
        }

        GameObject msgItem = Instantiate(messageItemPrefab, messageListParent);
        TMP_Text txt = msgItem.GetComponentInChildren<TMP_Text>();

        if (txt != null)
        {
            txt.text = message;
        }
        else
        {
            Debug.LogWarning("[MessageDisplayer] ⚠️ TMP_Text topilmadi!");
        }
    }
}