using Obvious.Soap;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.EventSystems;

public class MessageDisplayer : MonoBehaviourPun
{
    public static MessageDisplayer Instance;

    [Header("UI komponentlar")]
    public Transform messageListParent;
    public GameObject messageItemPrefab;
    public TMP_Text gameOverText;

    [Header("Xabar yozish paneli")]
    public TMP_InputField messageInputField;

 
    [SerializeField] private BoolVariable UIPanel;
    [SerializeField] private GameObject UI;
    private void Awake()
    {
        if (messageInputField != null)
        {
            messageInputField.characterLimit = 30;
        }
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        Debug.Log("Kun: " + UIPanel.Value);

        if (UIPanel.Value==true)
        {
            Debug.Log("UI ON");
            UI.SetActive(true);
        }
        else
        {
            
            Debug.Log("UI OFF");
            UI.SetActive(false);
        }
        
        if (messageInputField != null && Input.GetKeyDown(KeyCode.Return) && messageInputField.isFocused)
        {
            string message = messageInputField.text.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                string playerName = PhotonNetwork.NickName; // Foydalanuvchi nomi
                ShowMessageToAll($"{playerName}: {message}");
                messageInputField.text = "";
                EventSystem.current.SetSelectedGameObject(null); // Fokusdan chiqarish
            }
        }
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
    public void RPC_ShowWinnerMessage(string message, Vector3 colorVec)
    {
        if (gameOverText != null)
        {
            gameOverText.text = message;
            gameOverText.color = new Color(colorVec.x, colorVec.y, colorVec.z);
            gameOverText.gameObject.SetActive(true);
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
