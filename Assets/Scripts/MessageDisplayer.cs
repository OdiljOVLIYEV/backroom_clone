using UnityEngine;
using TMPro;
using Photon.Pun; // <--- ADD THIS

public class MessageDisplayer : MonoBehaviourPun
{
    public static MessageDisplayer Instance;

    [Header("UI komponentlar")]
    public Transform messageListParent;
    public GameObject messageItemPrefab;
    public TMP_Text gameOverText; 
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

    public void ShowWinnerMessageToAll(string message, Color color)
    {
        photonView.RPC(nameof(RPC_ShowWinnerMessage), RpcTarget.All, message, new Vector3(color.r, color.g, color.b));
    }

    [PunRPC]
    private void RPC_ShowWinnerMessage(string message, Vector3 colorVec)
    {
        if (gameOverText != null)
        {
            gameOverText.text = message;
            gameOverText.color = new Color(colorVec.x, colorVec.y, colorVec.z);
            gameOverText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[MessageDisplayer] ❌ gameOverText yo‘q");
        }
    }


    [PunRPC]
    public void RPC_ShowMessage(string message)
    {
        if (messageListParent == null || messageItemPrefab == null)
        {
            Debug.LogWarning("[MessageDisplayer] ❌ UI komponentlar yo‘q.");
            return;
        }

        if (messageListParent.childCount >= 9)
        {
            Destroy(messageListParent.GetChild(0).gameObject);
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
    
    public void ClearMessagesForAll()
    {
        photonView.RPC(nameof(RPC_ClearMessages), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_ClearMessages()
    {
        foreach (Transform child in messageListParent)
        {
            Destroy(child.gameObject);
        }
    }


}