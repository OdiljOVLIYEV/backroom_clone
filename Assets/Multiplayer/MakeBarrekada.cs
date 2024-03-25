using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MakeBarrekada : MonoBehaviour
{
    
    bool barrikamake = false;
    private MeshRenderer msh;
    private MeshCollider mC;
    public GameObject SLIDER;
    private float MakeSpeed = 0.1f;
    public float MaxTime = 30f;
    public Slider slider; 
    public float barrikadaHealt = 100f;
    public GameObject alpha;
    private BoxCollider BX;
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        msh = GetComponent<MeshRenderer>();
        mC = GetComponent<MeshCollider>();
        BX = GetComponent<BoxCollider>();
        PV = GetComponent<PhotonView>();
        msh.enabled = false;
       mC.enabled = false;
        barrikamake = false;

        slider.minValue = 0f;
        slider.maxValue = MaxTime;
        slider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (barrikadaHealt <= 0)
        {

            BX.enabled= false;
        }*/


    }

    private void OnTriggerStay(Collider other)
    {
      
       

        if (other.gameObject.tag == "Player")
        {
                alpha.SetActive(true);
             
                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("TEGDI");
                    if (barrikamake == false)
                {
                    SLIDER.SetActive(true);
                    timemake();
                    if (slider.value == MaxTime)
                    {
                        PV.RPC("BarricadeBuild", RpcTarget.All);
                        //BarricadeBuild();
                    }

                }
                if (barrikamake == true)
                {


                }
                  

                    Debug.Log("salom");
            }
            else
            {
                SLIDER.SetActive(false);
                slider.value = 0f;
            }
            }
            else
            {
              
            }
    }

    private void OnTriggerExit(Collider other)
    {
        alpha.SetActive(false);
        SLIDER.SetActive(false);
        slider.value = 0f;
        
    }

    void timemake()
    {


        slider.value += MakeSpeed;

    }

    public void axeDamage(float amout)
    {

        barrikadaHealt -= amout;

        Debug.Log("ATTACK");
        if (barrikadaHealt <= 0)
        {

            PV.RPC("BarricadeDestroy", RpcTarget.All);

        }

    }

    [PunRPC]
    public void BarricadeBuild()
    {
        barrikadaHealt = 100f;
        barrikamake = true;
        SLIDER.SetActive(false);
        msh.enabled = true;
        mC.enabled = true;
        BX.isTrigger = false;


    }
    [PunRPC]
    public void BarricadeDestroy()
    {
        barrikamake = false;
        msh.enabled = false;
        mC.enabled = false;
        BX.isTrigger = true;


    }
}
