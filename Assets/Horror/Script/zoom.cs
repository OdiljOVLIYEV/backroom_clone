﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class zoom : MonoBehaviour
{
	public Animator anim;
	public GameObject crosshair;
	private damagepis shoot;
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
		
		{
			if (Input.GetKey("left shift"))
			{

				anim.SetBool("zoom", false);
				crosshair.SetActive(true);
				press = true;
			}
			else
			{


			}


			if (Input.GetButtonDown("Fire2"))
			{

				if (press == true)
				{
					press = false;
					crosshair.SetActive(false);
                    anim.SetBool("zoom", true);

                   


				}
				else
				{
					press = true;
					crosshair.SetActive(true);
					anim.SetBool("zoom", false);

				}
			}
			shoot = FindObjectOfType<damagepis>();
			if (shoot != null)
				if (shoot.bullet > 0)
				{

					if (Input.GetButtonDown("Fire1"))
					{

						anim.SetBool("ZOOM SHOOT", true);

						Invoke("restart", 0.1f);
					}

				}
		}	
	}


	/*void retard(){
		
		anim.SetBool("zoom",false);
	}*/
	
    
    
	 void restart(){
		
		  anim.SetBool("ZOOM SHOOT",false);
		     
	}


}