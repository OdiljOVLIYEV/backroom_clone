using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class akszoom : MonoBehaviour
{
	public Animator anim;
	
	public GameObject crosshair;
	private AKSshoot shoot;
	public bool press;
    // Start is called before the first frame update
	void Start()
    
	{   anim.keepAnimatorStateOnDisable=true;
		press= true;
	    anim= GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKey("left shift")){
			
			anim.SetBool("akszoom",false);
			crosshair.SetActive(true);
			press=true;
		}else{
			
			
		}
		
		
		
		if(Input.GetButtonDown("Fire2")){
			if(press==true){
				press=false;
			crosshair.SetActive(false);
			anim.SetBool("akszoom",true);
			
			
			}else{
				press=true;
			crosshair.SetActive(true);
				anim.SetBool("akszoom",false);
		
			
		}
		
		
		}
		
		shoot=FindObjectOfType<AKSshoot>();
		if(shoot!=null)
			if(shoot.bullet>0){
				
				if(Input.GetButton("Fire1")){
					
					anim.SetBool("akszoomshoot",true);
				
					Invoke("restart",0.1f);
				}	
				
			}	
				
	}			
			
	
	/*void retard(){
		
		anim.SetBool("akszoom",false);
	}*/

	
    
    
	 void restart(){
		
		  anim.SetBool("akszoomshoot",false);
		     
	}


}