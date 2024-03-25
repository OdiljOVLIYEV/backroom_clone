using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Security;

public class WeaponPick : MonoBehaviour
{

    public float range = 20f;
    public Camera cam;
    public GameObject TEXT;
    public bool Raycast;
    private PhotonView pv;
    //bolta uchun
    public GameObject axe;
    public GameObject axe_two;
    public GameObject axe_icon;
    public bool axepick;

    // pistolet uchun
    public GameObject pistolet;
    public GameObject pistolpick;
    public GameObject pistol_icon;
    public bool pistoletpick;

    // rifle uchun
    public GameObject rifle;
    public GameObject riflepick;
    public GameObject rifle_icon;
    public bool riflepicked;

    //  shootgun uchun
    public GameObject shootgun;
    public GameObject shootgunpick;
    public GameObject shootgun_icon;
    public bool shootgunpicked;
    public Animator anim;
    public Animator pistol;

    public GameObject fire;
    public ParticleSystem gun;
    public bool building;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        Raycast = false;
        axepick = true;
        building = false;
        pistoletpick = false;
        riflepicked = false;
        shootgunpicked = false;
        axe_two.SetActive(true);
        axe_icon.SetActive(true);
        axeanimationOn();
    }


    void Update()
    {

        /* if (Input.GetKey(KeyCode.Mouse0))
        {
            pv.RPC("fireON", RpcTarget.All);
            /* if (pistoletpick == true)
             {

             }*/
            //Invoke("fireOFF", 0.1f);

         //}
        /*if (Input.GetKeyDown(KeyCode.V))
        {

            if (building == false)
            {
                Debug.Log("qurishga ruxsat");
                building = true;
            }
            else
            {
                Debug.Log("qurishga ruxsat bekor qilindi");
                building = false;

            }


        }*/

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {




            if (hit.transform.tag == "BOLTA")
            { Raycast = true;
                ShaderChange change = hit.transform.GetComponent<ShaderChange>();
                if (change != null)
                    change.white();
                if (Input.GetKey(KeyCode.E))
                {
                    pistol_icon.SetActive(false);
                    axe_icon.SetActive(true);
                    axepick = true;
                    // AxeOn();
                    //pistoletOFF();
                    // rifleOFF();
                    // ShootgunOFF();
                    // rifleanimationff();
                    //axeanimationOn();
                    //PISTOanimationff();
                    pv.RPC("AxeOn", RpcTarget.All);
                    pv.RPC("pistoletOFF", RpcTarget.All);
                    pv.RPC("rifleOFF", RpcTarget.All);
                    pv.RPC("ShootgunOFF", RpcTarget.All);
                    pv.RPC("rifleanimationff", RpcTarget.All);
                    pv.RPC("axeanimationOn", RpcTarget.All);
                    pv.RPC("PISTOanimationff", RpcTarget.All);
                    Debug.Log("bolta");


                }

            }

            else
            {

                Raycast = false;
            }

            if (hit.transform.tag == "pistolet")
            {
                Raycast = true;
                ShaderChange change = hit.transform.GetComponent<ShaderChange>();
                if (change != null)
                    change.white();
                if (Input.GetKey(KeyCode.E))
                {
                    damagepis PIC = FindObjectOfType<damagepis>();
                    pistoletpick = true;
                    axepick = false;
                    pistol_icon.SetActive(true);
                    axe_icon.SetActive(false);
                    // rifleanimationff();
                      pistoletOn();
                      AXEOFF();
                    // rifleOFF();
                    // ShootgunOFF();
                      axeanimationff();
                    // rifleanimationff();
                      PISTOLanimationOn();
                    pv.RPC("pistoletOn", RpcTarget.All);
                    pv.RPC("AXEOFF", RpcTarget.All);
                    pv.RPC("rifleOFF", RpcTarget.All);
                    pv.RPC("rifleanimationff", RpcTarget.All);
                    pv.RPC("axeanimationff", RpcTarget.All);
                    pv.RPC("ShootgunOFF", RpcTarget.All);
                    pv.RPC("PISTOLanimationOn", RpcTarget.All);
                    Debug.Log("PISTOLET");


                }

            }
            else
            {
                Raycast = false;

            }
            if (hit.transform.tag == "rifle")
            {
                Raycast = true;
                ShaderChange change = hit.transform.GetComponent<ShaderChange>();
                if (change != null)
                    change.white();
                if (Input.GetKey(KeyCode.E))
                {
                    shootgun_icon.SetActive(false);
                    rifle_icon.SetActive(true);
                    riflepicked = true;
                    shootgunpicked = false;
                    //  rifleOn();
                    // AXEOFF();
                    // pistoletOFF();
                    //  ShootgunOFF();
                    //  rifleanimationOn();
                    //    axeanimationff();
                    // PISTOanimationff();
                    pv.RPC("pistoletOFF", RpcTarget.All);
                    pv.RPC("AXEOFF", RpcTarget.All);
                    pv.RPC("rifleOn", RpcTarget.All);
                    pv.RPC("ShootgunOFF", RpcTarget.All);
                    pv.RPC("rifleanimationOn", RpcTarget.All);
                    pv.RPC("axeanimationff", RpcTarget.All);
                    pv.RPC("PISTOanimationff", RpcTarget.All);
                    Debug.Log("rifle");


                }

            }
            else
            {
                Raycast = false;

            }
            if (hit.transform.tag == "shootgun")
            {
                Raycast = true;
                ShaderChange change = hit.transform.GetComponent<ShaderChange>();
                if (change != null)
                    change.white();
                if (Input.GetKey(KeyCode.E))
                {
                    shootgun_icon.SetActive(true);
                    rifle_icon.SetActive(false);
                    riflepicked = false;
                    shootgunpicked = true;
                    //  ShootgunOn();
                    //  rifleOFF();
                    //  AXEOFF();
                    //  pistoletOFF();
                    //  rifleanimationOn();
                    //  axeanimationff();
                    //  PISTOanimationff();
                    pv.RPC("ShootgunOn", RpcTarget.All);
                    pv.RPC("pistoletOFF", RpcTarget.All);
                    pv.RPC("AXEOFF", RpcTarget.All);
                    pv.RPC("rifleOFF", RpcTarget.All);
                    pv.RPC("rifleanimationOn", RpcTarget.All);
                    pv.RPC("axeanimationff", RpcTarget.All);
                    pv.RPC("PISTOanimationff", RpcTarget.All);
                    Debug.Log("shootgun");


                }

            }
            else
            {
                Raycast = false;

            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha1) && axepick == true)
        {
            axe_icon.SetActive(true);
            pistol_icon.SetActive(false);
            pistoletpick = false;
            // AxeOn();
            //pistoletOFF();
            //  rifleOFF();
            // ShootgunOFF();
            // axeanimationOn();
            // rifleanimationff();
            // PISTOanimationff();
            pv.RPC("AxeOn", RpcTarget.All);
            pv.RPC("pistoletOFF", RpcTarget.All);
            pv.RPC("rifleOFF", RpcTarget.All);
            pv.RPC("ShootgunOFF", RpcTarget.All);
            pv.RPC("axeanimationOn", RpcTarget.All);
            pv.RPC("rifleanimationff", RpcTarget.All);
            pv.RPC("PISTOanimationff", RpcTarget.All);

        } else
        if (Input.GetKeyUp(KeyCode.Alpha1) && pistoletpick == true)
        {

            axe_icon.SetActive(false);
            pistol_icon.SetActive(true);
            axepick = false;
            // pistoletOn();
            // AXEOFF();
            // rifleOFF();
            // ShootgunOFF();
            // axeanimationff(); 
            // rifleanimationff();
            // PISTOLanimationOn();
            pv.RPC("pistoletOn", RpcTarget.All);
            pv.RPC("AXEOFF", RpcTarget.All);
            pv.RPC("rifleOFF", RpcTarget.All);
            pv.RPC("ShootgunOFF", RpcTarget.All);
            pv.RPC("axeanimationff", RpcTarget.All);
            pv.RPC("rifleanimationff", RpcTarget.All);
            pv.RPC("PISTOLanimationOn", RpcTarget.All);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2) && riflepicked == true)
        {
            rifle_icon.SetActive(true);
            shootgun_icon.SetActive(false);
            shootgunpicked = false;
            //rifleOn();
            // pistoletOFF();
            // AXEOFF();
            // ShootgunOFF();
            // rifleanimationOn();
            // axeanimationff();
            // PISTOanimationff();
            pv.RPC("pistoletOFF", RpcTarget.All);
            pv.RPC("AXEOFF", RpcTarget.All);
            pv.RPC("rifleOn", RpcTarget.All);
            pv.RPC("ShootgunOFF", RpcTarget.All);
            pv.RPC("rifleanimationOn", RpcTarget.All);
            pv.RPC("axeanimationff", RpcTarget.All);
            pv.RPC("PISTOanimationff", RpcTarget.All);
        }


        if (Input.GetKeyUp(KeyCode.Alpha2) && shootgunpicked == true)
        {
            shootgun_icon.SetActive(true);
            rifle_icon.SetActive(false);
            riflepicked = false;
            //ShootgunOn();
            // pistoletOFF();
            // AXEOFF();
            // rifleOFF();
            //rifleanimationOn();
            // axeanimationff();
            // PISTOanimationff();
            pv.RPC("ShootgunOn", RpcTarget.All);
            pv.RPC("pistoletOFF", RpcTarget.All);
            pv.RPC("AXEOFF", RpcTarget.All);
            pv.RPC("rifleOFF", RpcTarget.All);
            pv.RPC("rifleanimationOn", RpcTarget.All);
            pv.RPC("axeanimationff", RpcTarget.All);
            pv.RPC("PISTOanimationff", RpcTarget.All);
        }


    }
    [PunRPC]
    public void AxeOn()
    {
        axe.SetActive(true);
        axe_two.SetActive(true);
        anim.SetBool("axeidle", true);
        TEXT.SetActive(false);
    }


    [PunRPC]
    public void AXEOFF()
    {
        axe.SetActive(false);
        axe_two.SetActive(false);
        TEXT.SetActive(true);
    }

    [PunRPC]
    public void pistoletOn()
    {
        pistolet.SetActive(true);
        pistolpick.SetActive(true);



    }

    [PunRPC]
    public void pistoletOFF()
    {
        pistolet.SetActive(false);
        pistolpick.SetActive(false);


    }
    [PunRPC]
    public void rifleOn()
    {
        rifle.SetActive(true);
        riflepick.SetActive(true);



    }

    [PunRPC]
    public void rifleOFF()
    {
        rifle.SetActive(false);
        riflepick.SetActive(false);

    }
    [PunRPC]
    public void ShootgunOn()
    {
        shootgun.SetActive(true);
        shootgunpick.SetActive(true);


    }

    [PunRPC]
    public void ShootgunOFF()
    {
        shootgun.SetActive(false);
        shootgunpick.SetActive(false);

    }
    [PunRPC]
    public void rifleanimationOn()
    {
        anim.SetBool("normalweapon", true);


    }

    [PunRPC]
    public void rifleanimationff()
    {
        anim.SetBool("normalweapon", false);

    }

    [PunRPC]
    public void axeanimationOn()
    {
        anim.SetBool("axeidle", true);


    }

    [PunRPC]
    public void axeanimationff()
    {
        anim.SetBool("axeidle", false);

    }

    [PunRPC]
    public void PISTOLanimationOn()
    {
        anim.SetBool("pistolidle", true);


    }

    [PunRPC]
    public void PISTOanimationff()
    {
        anim.SetBool("pistolidle", false);

    }
    [PunRPC]
    public void fireON()
    {
       // fire.SetActive(true);
        gun.Play();
    }
    public void fireOFF()
    {
        fire.SetActive(false);
    }
    /*  private void OnTriggerStay(Collider other)
      {

          if (other.gameObject.tag == "taxta")
          {
              if(other.gameObject.GetComponent<MakeBarrekada>()!=null)

              other.gameObject.GetComponent<MakeBarrekada>().enabled = true;
          }
          else
          {
              if (other.gameObject.GetComponent<MakeBarrekada>() != null)
                  other.gameObject.GetComponent<MakeBarrekada>().enabled = false;
          }

      }
    */

}
