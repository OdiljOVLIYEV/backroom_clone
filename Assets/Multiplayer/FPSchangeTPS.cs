using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FPSchangeTPS : MonoBehaviourPunCallbacks
{
    public GameObject[] Fpsview;
    public GameObject[] Tpsview;
    public Camera fpsCAMERA;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            foreach(GameObject gameObject in Fpsview)
            {
                gameObject.SetActive(true);
            }

            foreach (GameObject gameObject in Tpsview)
            {
                gameObject.SetActive(false);
            }

            fpsCAMERA.enabled = true;

        }
        else
        {
            foreach (GameObject gameObject in Fpsview)
            {
                gameObject.SetActive(false);
            }

            foreach (GameObject gameObject in Tpsview)
            {
                gameObject.SetActive(true);
            }
            fpsCAMERA.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
