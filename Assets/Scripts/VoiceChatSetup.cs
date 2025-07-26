using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceChatSetup : MonoBehaviourPun
{
    private void Start()
    {
        Debug.Log("VoiceChatSetup: Start called on " + gameObject.name);

        if (!photonView.IsMine)
        {
            Debug.Log("VoiceChatSetup: Not local player, disabling voice components on " + gameObject.name);

            var voiceView = GetComponent<PhotonVoiceView>();
            var recorder = GetComponent<Recorder>();

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
        var recorderLocal = GetComponent<Recorder>();

        if (voiceViewLocal == null)
        {
            Debug.LogError("VoiceChatSetup: PhotonVoiceView not found on local player!");
            return;
        }

        if (recorderLocal == null)
        {
            Debug.LogError("VoiceChatSetup: Recorder not found on local player!");
            return;
        }

        // Configure Recorder
        recorderLocal.TransmitEnabled = true;
        recorderLocal.VoiceDetection = true;

        Debug.Log("VoiceChatSetup: Recorder TransmitEnabled = " + recorderLocal.TransmitEnabled);
        Debug.Log("VoiceChatSetup: Recorder VoiceDetection = " + recorderLocal.VoiceDetection);
    }
}