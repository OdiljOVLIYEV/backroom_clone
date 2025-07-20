using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity; // Recorder va Speaker uchun
using Photon.Voice.PUN;   // PhotonVoiceNetwork uchun

public class VoiceChatSetup : MonoBehaviourPun
{
    private Recorder recorder;
    private bool wasTransmitting = false;

    void Start()
    {
        if (photonView.IsMine)
        {
            recorder = GetComponent<Recorder>();
            if (recorder != null)
            {
                recorder.TransmitEnabled = false; // Dastlab mic o‘chirilgan
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine || recorder == null) return;

        bool isTalking = Input.GetKey(KeyCode.T);
        recorder.TransmitEnabled = isTalking;

        // Debug: faqat holat o'zgarganda yoz
        if (isTalking && !wasTransmitting)
        {
            Debug.Log("📢 VoiceChat: Transmitting started.");
            wasTransmitting = true;
        }
        else if (!isTalking && wasTransmitting)
        {
            Debug.Log("🔇 VoiceChat: Transmitting stopped.");
            wasTransmitting = false;
        }
    }
}