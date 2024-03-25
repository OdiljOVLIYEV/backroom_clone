using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ovoz : MonoBehaviour
{
	
	
	private AudioSource source;
	public SphereCollider doira;
	
	
    // Start is called before the first frame update
    void Start()
	{
		doira =  GetComponent<SphereCollider>();
		source = GetComponent<AudioSource>();
		
    }

    // Update is called once per frame
	void Update()
	{ 
		if(Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKey(KeyCode.LeftShift)){
			
			doira.enabled=true;
			source.Play();
			source.loop=true;
		}
		if(Input.GetKeyUp(KeyCode.LeftShift)){
			
			
			doira.enabled=false;
			source.loop=false;
			//source.Stop();
			
		}
		
		if(Input.GetButtonDown("Fire1")){
			
			doira.enabled=true;
			
			
		}
		
		if(Input.GetButtonUp("Fire1")){
			
			doira.enabled=false;
			
			
		}
	}	
}
