using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class hearing_a_voice : MonoBehaviour
{
	private Transform playerTransform;
	[HideInInspector]
	public Vector3 transformsound;
	[HideInInspector]
	public Vector3 position;
	public bool hearing_position;
	
	
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		
		hearing_position=false;
	}
	
	
	protected void Update()
	{
		
		position=gameObject.transform.position;
		
		
		if(position.x==transformsound.x&&position.z==transformsound.z){
			
			hearing_position=false;
			
		}
		
	}
    // Start is called before the first frame update
	// OnTriggerEnter is called when the Collider other enters the trigger.
	private void OnTriggerEnter(Collider other)
	{
		
		if(other.gameObject.tag=="playersound"){
			
			hearing_position=true;
			Animator anim=GetComponent<Animator>();
			anim.SetBool("move",true);
		
			
			transformsound=new Vector3(playerTransform.transform.position.x,playerTransform.transform.position.y,playerTransform.transform.position.z);
			
			NavMeshAgent agent=GetComponent<NavMeshAgent>();
			agent.SetDestination(transformsound);
			
			
			
			
		}
		
		
		if(other.gameObject.tag=="weaponsound"){
			hearing_position=true;
			Animator anim=gameObject.GetComponent<Animator>();
			if(anim!=null)
			anim.SetBool("move",true);
			
			transformsound=new Vector3(playerTransform.transform.position.x,playerTransform.transform.position.y,playerTransform.transform.position.z);
			
			NavMeshAgent agent=GetComponent<NavMeshAgent>();
			agent.SetDestination(transformsound);
		}
	}
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	
	
}
