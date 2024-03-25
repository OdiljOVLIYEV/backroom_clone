using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uitext : MonoBehaviour
{
	public delegate void number();
	public static  number numbertwo;
	
	public float ammo;
	public Text text;
	public int clock;
    // Start is called before the first frame update
    void Start()
	{
		//StartCoroutine(start());
		//ammo=clock*1;
		
    }

	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		
		
		Invoke("pluss",1f);
		//pluss();
		text.text=ammo.ToString("F0");
	}
    
	void pluss(){
		
		
		StartCoroutine(start());
		Debug.Log("salom "+ammo);
		ammo=ammo+Time.deltaTime;;
		
	}
	
	IEnumerator start(){
		
		Debug.Log("test");
		
		yield return new WaitForSecondsRealtime(10f);
		
		
		//numbertwo-=pluss;
	}
	
}
