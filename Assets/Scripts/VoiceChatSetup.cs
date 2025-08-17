using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceChatSetup : MonoBehaviourPun
{
    private Recorder recorder;

    private void Start()
    {
        Debug.Log("VoiceChatSetup: Start called on " + gameObject.name);

        if (!photonView.IsMine)
        {
            Debug.Log("VoiceChatSetup: Not local player, disabling voice components on " + gameObject.name);

            var voiceView = GetComponent<PhotonVoiceView>();
            recorder = GetComponent<Recorder>();

            if (voiceView != null)
                voiceView.enabled = false;
            else
                Debug.LogWarning("VoiceChatSetup: PhotonVoiceView not found on " + gameObject.name);

            if (recorder != null)
                recorder.enabled = false;
            else
                Debug.LogWarning("VoiceChatSetup: Recorder not found on " + gameObject.name);

            return;
        }

        Debug.Log("VoiceChatSetup: Local player detected, configuring voice...");

        var voiceViewLocal = GetComponent<PhotonVoiceView>();
        recorder = GetComponent<Recorder>();

        if (voiceViewLocal == null)
        {
            Debug.LogError("VoiceChatSetup: PhotonVoiceView not found on local player!");
            return;
        }

        if (recorder == null)
        {
            Debug.LogError("VoiceChatSetup: Recorder not found on local player!");
            return;
        }

        // Bosilganda ovoz yuborish uchun boshlang'ich sozlama
        recorder.TransmitEnabled = false; // default = o'chirilgan
        recorder.VoiceDetection = false;  // faqat T bosilganda yoqiladi
    }

    private void Update()
    {
        if (!photonView.IsMine || recorder == null) return;

        // T tugmasi bosilganda ovoz yuborilsin
        if (Input.GetKey(KeyCode.T))
        {
            recorder.TransmitEnabled = true;
        }
        else
        {
            recorder.TransmitEnabled = false;
        }
    }
}
