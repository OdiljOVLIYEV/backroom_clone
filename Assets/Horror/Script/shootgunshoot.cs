using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class shootgunshoot : MonoBehaviour
{
	public float damage= 10f;
	public float range =100f;
	
	public Camera cam;
	public ParticleSystem gun;
    public ParticleSystem gun_two;
    public AudioSource sound;
   
	//o'qdori kodi
	//public int Magazinammo=12;
	private magazinshotgun ammo;
	public int maxammo=6;
	public int bullet;
	public int qoldiq;
	public int raqam;
	public bool tugadi;
    public TMP_Text text; 
	
	public float Reloadtime =  2f;
	public bool  isReloading;
	
	public Animator anim;
    public Animator rifletps;
    private switchweapon change;
	private damagepis select;
	private bool canshoot;
	bool run;
	private BACKMENU back;
	private PlayerMovment movement;
    private PhotonView pv;
    public GameObject Blood;
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    protected void Start()
	{
		anim.keepAnimatorControllerStateOnDisable=true;
		change=FindObjectOfType<switchweapon>();
		canshoot=true;
		anim=GetComponent<Animator>();
		bullet=maxammo;
		ammo=FindObjectOfType<magazinshotgun>();
		movement=FindObjectOfType<PlayerMovment>();
        pv = GetComponent<PhotonView>();
    }
	// Update is called once per frame

	private void FixedUpdate()
	{
		if (Input.GetKey("left shift") && movement.z == 1f)
		{
			Debug.Log(movement.z);
			run = true;
			anim.SetBool("run", true);
		}
		else
		{
			anim.SetBool("run", false);
			run = false;
		}

		

    


}
    void Update()
	{
			
		
			
			
			
			if(bullet==0&&Input.GetButtonDown("Fire1")&&canshoot){
			
				anim.SetBool("shoot2",false);
			anim.SetBool("reload2",true);
            rifletps.SetBool("riflereloading", true);
            StartCoroutine(Reload());
			
		
			}
		
		
		
			
				
				back=FindObjectOfType<BACKMENU>();
				if(back.menu==false){
	    if(Input.GetButtonDown("Fire1")&&canshoot&&bullet>0&&run==false){
	    	anim.SetBool("changeweapon2",false);
	    	
	    	Shoot();
	    	
	    	
	              }
			   
			
		
		
		
			
		if(Input.GetKeyDown(KeyCode.R)&&ammo.Magazinammo>0&&canshoot){
			canshoot=false;
			
			anim.SetBool("reload2",true);
            rifletps.SetBool("riflereloading", true);
                Invoke("reload",2f);
			
			
		
		}
		if(Input.GetKeyDown(KeyCode.R)&&bullet==maxammo){
			
			
			anim.SetBool("reload2",false);
            rifletps.SetBool("riflereloading", false);



            }
		
		text.text=bullet+"/"+ammo.Magazinammo;
		
		if(bullet==0 && ammo.Magazinammo==0){
			
			anim.SetBool("reload2",false);
            rifletps.SetBool("riflereloading", false);
                return;
		}
		
		if(isReloading)
			return;
		
		
		
		
		
		
			
		
		
		}
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
		anim.SetBool("reload2",false);
        rifletps.SetBool("riflereloading", false);
    }
		
	
		
		
    
	void Shoot(){
		gun.Play();
        pv.RPC("fireON", RpcTarget.All);
        
        sound.Play();
		anim.SetBool("shoot2",true);
        rifletps.SetBool("shootgun_attack", true);
        canshoot =false;
		StartCoroutine(shotgun());
		bullet--;
		
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,range)){
			if (hit.collider.gameObject.CompareTag("Enemy"))
			{

				Vector3 spawnPosition = hit.point + (hit.normal * 0.1f);
				pv.RPC("SpawnObject", RpcTarget.All, spawnPosition);
				hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
				Debug.Log(hit.transform);

				Target target = hit.transform.GetComponent<Target>();

				if (target != null)
				{
					target.TakeDamage(damage);

				}

			}	
		}
	}
	
	
	IEnumerator Reload(){
		
		isReloading = true;
		
		
		
		yield return new WaitForSeconds(Reloadtime);
		
		
		
		if(ammo.Magazinammo>=maxammo){
			
			bullet=maxammo;	
			ammo.Magazinammo-=maxammo;
		}
		else{
			bullet=ammo.Magazinammo;
			ammo.Magazinammo=0;
			
		}
		
		anim.SetBool("reload2",false);
        rifletps.SetBool("riflereloading", false);
        isReloading =false;
	}
	
	IEnumerator shotgun(){
		
		yield return new WaitForSeconds(1f);
		canshoot=true;
		anim.SetBool("shoot2",false);
        rifletps.SetBool("shootgun_attack", false);
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
}
	
	

