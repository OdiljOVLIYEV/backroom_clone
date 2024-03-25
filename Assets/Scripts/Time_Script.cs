using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Time_Script : MonoBehaviour
{
    public Text text;
    public Text WaveText;
    public GameObject timeotext;
    public GameObject wavetext;


    private float WaveNumber = 1;
    public float Maxtime;
    public float TIME_minus;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(timeWave());
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Maxtime.ToString();
        WaveText.text = "Wave" + WaveNumber.ToString();

        SpawnEnemy spawnEnemy = FindObjectOfType<SpawnEnemy>();
        if (spawnEnemy != null)
        if (spawnEnemy.allkill == true)
        {

            TIME_time();
        }
        if (spawnEnemy.allkill == false)
        {

            timeotext.SetActive(false);
            wavetext.SetActive(true);

        }


        // StartCoroutine(timeWave());


    }
    IEnumerator timeWave()
    {
        TIME_minus = Maxtime;
        while (Maxtime > 0)
        {


            yield return new WaitForSeconds(1f);
            Maxtime -= 1;
           

            if (Maxtime == 0)
            {
                timeotext.SetActive(false);
                wavetext.SetActive(true);


                    SpawnEnemy spawnEnemy = FindObjectOfType<SpawnEnemy>();     
                    spawnEnemy.StartCoroutine(spawnEnemy.Enemydrop());
                
       


               // Invoke("TIME_time", 1f);
              
            }
            else
            {

                timeotext.SetActive(true);
                wavetext.SetActive(false);
            }
        }
    }
   public void TIME_time()
    {
        SpawnEnemy spawnEnemy = FindObjectOfType<SpawnEnemy>();
        if (spawnEnemy != null)
            spawnEnemy.allkill = false;
                WaveNumber += 1;
        Maxtime = TIME_minus;
        StartCoroutine(timeWave());
        timeotext.SetActive(false);
        wavetext.SetActive(true);
    }
}