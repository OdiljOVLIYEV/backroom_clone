using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dooropen : MonoBehaviour
{
	
	
	public AudioSource opens;
	public AudioSource close;
	public Animator anim;
	public bool birinchi_eshik;
	public GameObject bx1;
	public GameObject bx2;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		Animator anim=FindObjectOfType<Animator>();
		
	}
	
    // Start is called before the first frame update
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		
		
		if(other.gameObject.tag=="Player"){
			
			//birinchi_eshik=true;
			
			//if(birinchi_eshik==true){
				bx1.SetActive(true);
				bx2.SetActive(false);
			anim.SetBool("open",true);
			
			
				opens.Play();
				
			
			//	}	
				
		}/*else{
			
			dooropentwo open=FindObjectOfType<dooropentwo>();
			open.ikkinchi_eshik=true;
			
		}*/
			
			
		
	}
	
	// OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
	
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		
		if(other.gameObject.tag=="Player"){
			
			
			//if(birinchi_eshik==true){
			
			
			
			Invoke("closedoor",1f);
				
			
			
			//}
			
		}
		
	}
	
	void closedoor(){
		
		bx1.SetActive(true);
		bx2.SetActive(true);
		 close.Play();
		anim.SetBool("open",false);
		anim.SetBool("open2",false);
		birinchi_eshik=false;
	}
}
