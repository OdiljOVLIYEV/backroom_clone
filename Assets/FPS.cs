using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
	public TextMeshProUGUI Fpstext;
	
	private float pollingTime=1f;
	private float time;
	private int frameCount;
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		time+=Time.deltaTime;
		
		frameCount++;
		
		if(time >=pollingTime){
			
			
			int frameRate =Mathf.RoundToInt(frameCount / time);
			Fpstext.text = frameRate.ToString() +" "+"FPS";
			
			time -= pollingTime;
			frameCount=0;
		}
	}
	
}
