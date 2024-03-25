using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class Photon_PunTimer: MonoBehaviourPunCallbacks
{
    private float timer = 0f;
    public int speed = 60;
    public Text text;
    public Text WaveText;
    //public GameObject timeotext;
    //public GameObject wavetext;
    private void Start()
    {

       //pv= GetComponent<PhotonView>();

    }
    void Update()
    {
        text.text = timer.ToString();
       
        if (PhotonNetwork.IsMasterClient)
        {
            // Increment timer on the Master Client
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);

            // Send timer value to all other clients
           // pv.RPC("UpdateTimer", RpcTarget.OthersBuffered, timer);
            photonView.RPC("UpdateTimer", RpcTarget.OthersBuffered, timer);
        }
    }

    [PunRPC]
    void UpdateTimer(float timerValue)
    {
        // Update timer value on all clients except the Master Client
        timer = timerValue;
        Debug.Log("Timer: " + timer);

    }
}
