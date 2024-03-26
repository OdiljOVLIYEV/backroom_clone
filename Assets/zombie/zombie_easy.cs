using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombie_easy : MonoBehaviour
{
	private Transform playerTransform;
	private CapsuleCollider col;
	private NavMeshAgent agent;
	private Animator anim;
	private Detector detectors;
	private hearing_a_voice eshitish;
	public bool eshitildi;
	//private float accyrasy = 0.14f;
	private Target target;
	public bool shoot;
	public bool tegsa;
	
	
    // Start is called before the first frame update
    void Start()
	{
		anim =GetComponent<Animator>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		col=GetComponent<CapsuleCollider>();
	    agent= GetComponent<NavMeshAgent>();
		detectors=FindObjectOfType<Detector>();
		eshitish=FindObjectOfType<hearing_a_voice>();
		target = FindObjectOfType<Target>();
    }

    // Update is called once per frame
	void Update()
	{
		
		
	
		if(shoot==true){
			
			
			//anim.SetBool("move",true);
			agent.SetDestination(playerTransform.transform.position);
			
		}
		
		
		if(agent.velocity.magnitude>0.15f){
			//agent.speed=0f;
			anim.SetBool("move",false);
		
		}
		/*if(target.healt<=0f){
			agent.speed=0f;
			//GetComponent<CapsuleCollider>().enabled=false;
		}*/
		
		PlayerHealt cam=FindObjectOfType<PlayerHealt>();
		cam.CAMERA2.SetActive(false);
		//if(Vector3.Distance(agent.transform.position,playerTransform.position)> accyrasy);
	}
	
	
    
}
