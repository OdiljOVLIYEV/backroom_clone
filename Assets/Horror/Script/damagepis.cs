using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;


public class damagepis : MonoBehaviour
{
	public float damage= 10f;
	public float range =100f;
	public float oneshoot=100f;
	
	public Camera cam;
	public ParticleSystem gun;
    public ParticleSystem gun_two;
    public AudioSource sound;
   
	//o'qdori kodi
	//public int Magazinammo=12;
	private magazinpistolet ammo;
	public int maxammo=6;
	public int bullet;
	public int qoldiq;
	public int raqam;
	public bool tugadi;
	public TMP_Text text;
	
	public float Reloadtime =  2f;
	public bool  isReloading;
	
	public Animator anim;
    public Animator animtps;
    private switchweapon change;
	bool run;
	public GameObject Blood;
	public GameObject devor;
    private PhotonView pv;

    public bool canshoot;
	private BACKMENU back;
	private PlayerMovment movement;
	//public PhotonView PV;
    
	
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
	{
		anim.keepAnimatorControllerStateOnDisable=true;
		ammo=FindObjectOfType<magazinpistolet>();
		anim=GetComponent<Animator>();
		anim.SetBool("changeweapon",true);
		canshoot=true;
		bullet=maxammo;
		change=FindObjectOfType<switchweapon>();
		movement=FindObjectOfType<PlayerMovment>();
        pv = GetComponent<PhotonView>();

    }

    public void FixedUpdate()
    {
        if (Input.GetKey("left shift") && movement.z == 1f)
        {

			
            run = true;
            anim.SetBool("run", true);
        }
        else
        {
			
            run = false;
            anim.SetBool("run", false);
         
        }

        if (bullet > 0)
        {


            anim.SetBool("shoot", false);



           // back = FindObjectOfType<BACKMENU>();
          //  if (back.menu == false)
           // {
                if (Input.GetButtonDown("Fire1") && canshoot && run == false)
                {


                    Shoot();


                }

           //}


        }
        else
        {

            anim.SetBool("shoot", false);
        }
    }
    // Update is called once per frame
    void Update()
	{
		Invoke("retardanimation",0.1f);
		//Debug.Log(movement.z);
		//detectors.playerDetected=false;
		
		
		
		
		
		
		
		/*if(bullet==0&&Input.GetButtonDown("Fire1")&&!isReloading){
			Debug.Log("O'Q TAMOM");
			return;
		}*/
		
		
		
		
		
		
			
		if(bullet==0&&Input.GetButtonDown("Fire1")&&!isReloading){
			
			anim.SetBool("shoot",false);
            animtps.SetBool("pistolReloading", true);
            anim.SetBool("reload",true);
			
		StartCoroutine(Reload());
			
		
		}
		
		if(bullet>0){
				
			anim.SetBool("shoot",false);
			
		
		
					
					back=FindObjectOfType<BACKMENU>();
				if(back.menu==false){
			
					if(Input.GetButtonDown("Fire1")&&canshoot&&run==false){
	    	
						anim.SetBool("changeweapon",false);
						
				 Shoot();
	    	
					}
					if(Input.GetButtonUp("Fire1")&&canshoot&&run==false){
						
						
					}
				
			}
		}else{
			
				anim.SetBool("shoot",false);
		}
		
		
			
		if(Input.GetKeyDown(KeyCode.R)&&ammo.Magazinammo>0&&canshoot){
			canshoot=false;
            

            animtps.SetBool("pistolReloading", true);
            anim.SetBool("reload",true);
			
			Invoke("reload",2f);
			
			
		
		}
		if(Input.GetKeyDown(KeyCode.R)&&bullet==maxammo){
			
			anim.SetBool("reload",false);
            animtps.SetBool("pistolReloading", false);




        }
		//if(ammo.Magazinammo!=null)
			
			text.text=bullet+"/"+ammo.Magazinammo;
		//text.text=bullet+"/"+ammo.Magazinammo;
		
		if(bullet==0 && ammo.Magazinammo==0){
            animtps.SetBool("pistolReloading", false);
            anim.SetBool("reload",false);
			//anim.SetBool("shoot",false);
			return;
		}
		
		if(isReloading)
			return;
		
		
		
		
		
		
			
		
		}
	void retardanimation(){
		
		anim.SetBool("changeweapon",false);
	}
	
	
	void reload(){
		
		if(ammo.Magazinammo<maxammo){
				
				qoldiq=maxammo-bullet;
				ammo.Magazinammo-=qoldiq;
			    raqam=ammo.Magazinammo+bullet;
				
				
				if(raqam<maxammo){
				
					raqam=maxammo+ammo.Magazinammo;
					if(raqam<maxammo){
						bullet+=ammo.Magazinammo;
					}else{
						
						
						Debug.Log("error");
					}
					
				}
				
				if(ammo.Magazinammo<=0){
					
					ammo.Magazinammo=0;
				}
			
				
				bullet=bullet+qoldiq;
				Debug.Log(qoldiq);
					
			}else{
						
				qoldiq=maxammo;
				qoldiq-=bullet;
				ammo.Magazinammo-=qoldiq;
				bullet=maxammo;
				//Debug.Log(Magazinammo);
						
			}
		
		canshoot=true;
        animtps.SetBool("pistolReloading", false);
        anim.SetBool("reload",false);
		
	}
		
	
		
		
    
	 void Shoot(){
	
		
        gun.Play();
        pv.RPC("fireON", RpcTarget.All);
       
       
        sound.Play();

		
		//tpsgun.Play();





        //WeaponPick PS=FindObjectOfType<WeaponPick>();

        //PhotonNetwork.Instantiate(ParSystem.name, ParPosition.transform.position, ParPosition.transform.rotation);



        canshoot =false;  
		anim.SetBool("shoot",true);
		animtps.SetBool("pistol_attack", true);
		bullet--;
		 StartCoroutine(pistol());
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,range)){
			
			if(hit.transform.tag == "Enemy")
            {

                
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All,damage);

                //Instantiate(Blood,hit.point,Quaternion.FromToRotation(Vector3.right,hit.normal));
                //PhotonNetwork.Instantiate(Blood.name, hit.point, Quaternion.identity);
                Vector3 spawnPosition = hit.point + (hit.normal * 0.1f);
                pv.RPC("SpawnObject", RpcTarget.All, spawnPosition);
                Target target	= hit.transform.GetComponent<Target>();

				

                 target.TakeDamage(damage);
				if(target!=null){
					
				
				
				}
				
				
				Debug.Log("DUSHMAN");
			}
			
			if(hit.transform.tag=="head"){
				Debug.Log("QIYMAT BOSHGA TENG");

               // Vector3 spawnPosition = hit.point + (hit.normal * 0.1f);
               // pv.RPC("SpawnObject", RpcTarget.All, spawnPosition);
                //hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

                 Instantiate(Blood, hit.point, Quaternion.FromToRotation(Vector3.right, hit.normal));

                Target target = hit.transform.GetComponentInParent<Target>();



                target.TakeDamage(oneshoot);
                if (target != null)
                {



                }
                /*Target target = FindObjectOfType<Target>();	
				if (target != null)
					target.TakeDamage(oneshoot);*/

                Debug.Log("bosh");
			}
				
			
				
			if(hit.transform.tag=="devor"){
				Instantiate(devor,hit.point,Quaternion.FromToRotation(Vector3.right,hit.normal));
				
			}
			
			
			
				
		}
	}
	
	

	IEnumerator Reload(){
		
		isReloading = true;
		
		
		
		yield return new WaitForSeconds(Reloadtime);
		
		
		
		if(ammo.Magazinammo>=maxammo){
			
			bullet=maxammo;	
			ammo.Magazinammo-=maxammo;
			//change.shootgunweapon=true;
			//change.shootgunweapon=true;
		}
		else{
			bullet=ammo.Magazinammo;
			ammo.Magazinammo=0;
			
		}
		
		anim.SetBool("reload",false);
        animtps.SetBool("pistolReloading", false);
        isReloading =false;
	}

	[PunRPC]
	


	IEnumerator pistol(){
       
        yield return new WaitForSeconds(0.1f);
		canshoot=true;
		anim.SetBool("shoot",false);
        animtps.SetBool("pistol_attack", false);
       
    }

    [PunRPC]
    public void fireON()
    {
		gun_two.Play();
        //ParSystem_two.SetActive(true);
    }

    [PunRPC]
    void SpawnObject(Vector3 spawnPosition)
    {
        GameObject obj = Instantiate(Blood, spawnPosition, Quaternion.identity);
        //if (photonView.IsMine) PhotonNetwork.Destroy(obj);
    }
	// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
	// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.

	

}
	
	

