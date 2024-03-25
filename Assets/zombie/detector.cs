using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class detector : MonoBehaviourPunCallbacks
{
	public Transform[] waypoints; // Devriye noktalarining ro'yxati
	public float tezlik = 1f; // Harakat tezligi

	private int currentWaypointIndex = 0;
	public bool qaytib_keldi;
	
	public int fieldOfView = 45;
	public int viewDistance = 30;
	
	private Transform playerTransform;
	public bool playerDetected ;
	
	public bool isAware=false;
	private bool isDetecting=false;
	public float losetheshold=10f;
	public float losetimer=0f;
	
	private NavMeshAgent agent; 
	[HideInInspector]
	public float time=1f;
	public float wanderRadius=3f;
	private Vector3  wanderpoint;
	// Start is called before the first frame update
    
    
	void Start()
	{
		
		
		agent= GetComponent<NavMeshAgent>();
	    
		//wanderpoint= RandomWanderPoint();
		//qaytib_keldi=false;
		// NavMeshAgent komponentini olamiz
		
		
		// Boshlang'ich navbat nuqtasiga o'tkazamiz
		if (waypoints.Length > 0)
		{
			agent.SetDestination(waypoints[currentWaypointIndex].position);
		}
	}
    

	// Update is called once per frame
	private void Update()
	{  
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	   
		if(transform.position.x==waypoints[currentWaypointIndex].position.x)
		{
			agent.SetDestination(waypoints[currentWaypointIndex].position);
			qaytib_keldi=false;
			//isAware = false;
			
			
			Debug.Log("sdsdsdsdsd");
		}
		
		
			
		if(qaytib_keldi==true){
			
			
			currentWaypointIndex = 0;
			qaytib_keldi=false;
			
		}
		//Debug.Log(transform.position);
		
		
		if(agent.velocity.magnitude<10f){
			
			Animator anim=GetComponent<Animator>();
			anim.SetBool("move",false);
			
		}
		
		
		if(isAware==true){
			animationmove();
			if(agent!=null)
				agent.SetDestination(playerTransform.transform.position);
			
			
			if(!isDetecting){
				
				
				losetimer +=Time.deltaTime;
				
				if(losetimer >=	losetheshold){
					
					
					losetimer = 0f;
					isAware = false;
					qaytib_keldi=true;
					Animator anim=GetComponent<Animator>();
					anim.SetBool("run",false);
					agent.speed=3f;
					
				}
			}
		}else{
			
			
				
			Wander();
				
			
			
			/*hearing_a_voice hearing=FindObjectOfType<hearing_a_voice>();
			
			if(hearing.hearing_position==false){
				
			Wander();
				
			}*/
			
			animationmove();
		}
    	
		DetectPlayer();
		
	}
    
	

	public  void DetectPlayer()
	{  
		RaycastHit hitInfo;
		Vector3 rayDirection = playerTransform.position - transform.position;
		if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView)
		{
			if (Physics.Raycast(transform.position, rayDirection, out hitInfo, viewDistance))
			{
				if(hitInfo.transform.tag=="Player"){
					
					
					OnAware();
					
					//Debug.Log("PLAYER");
					
				
				
				}else{
					playerDetected=false;
					isDetecting=false;
					
				}
			}else{
				
				playerDetected=false;
				isDetecting=false;
			}
		}else
		{
			playerDetected=false;
			isDetecting=false;
		}
		
	}

	private void OnDrawGizmos()
	{
		if (!Application.isEditor && playerTransform == null)
		{
			return;
		}

		if(playerDetected==true)
		{
			
			Debug.DrawLine(transform.position, playerTransform.position, Color.green);
		}
			

		Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
		Vector3 leftRayPoint = Quaternion.Euler(0f, -fieldOfView, 0f) * frontRayPoint;
		Vector3 rightRayPoint = Quaternion.Euler(0f, fieldOfView, 0f) * frontRayPoint;
        
		Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
		Debug.DrawLine(transform.position, leftRayPoint, Color.blue);
		Debug.DrawLine(transform.position, rightRayPoint, Color.blue);
	}
	
	public void OnAware(){
		
		agent.speed=5f;
		playerDetected = true;
		Animator anim=GetComponent<Animator>();
		anim.SetBool("run",true);
		isAware=true;
		isDetecting=true;
		losetimer=0f;
		agent.isStopped=false;
		
	}
	public void Wander(){
		
		
		// Agar NavMeshAgent komponenti faol bo'lsa
		if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
		{
			// Keyingi navbat nuqtasiga o'tkazamiz
			currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
			agent.SetDestination(waypoints[currentWaypointIndex].position);
			
			
		}
		
	}
	/*public void Wander(){
		
	//agent.GetComponent<NavMeshAgent>().enabled=false;
	if (waypointIndex <= waypoints.Length - 1)
	{  
			
		
	transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, tezlik * Time.deltaTime);
			
	// Rotate towards the target waypoint
	Vector3 direction = waypoints[waypointIndex].position - transform.position;
	Quaternion rotation = Quaternion.LookRotation(direction);
	transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);

	if (transform.position == waypoints[waypointIndex].position)
	{
				
	waypointIndex++;
				
	}
				
			
	}
	else
	{
			
	waypointIndex = 0;
			
			
	}
	}*/
	
	/*public void Wander(){
		
	if(Vector3.Distance(transform.position,wanderpoint)<2f){
			
			
	wanderpoint =RandomWanderPoint();
	}else{
			
	agent.SetDestination(wanderpoint);
	}
		
	
	}
	
	public Vector3 RandomWanderPoint(){
		
	Vector3 randompoint =(Random.insideUnitSphere*wanderRadius)+transform.position;
	NavMeshHit navhit;
	NavMesh.SamplePosition(randompoint,out navhit,wanderRadius, -1);
	return new Vector3(navhit.position.x,transform.position.y,navhit.position.z);
	}
	*/
	public void agentOn(){
		
		agent.GetComponent<NavMeshAgent>().enabled=true;
	}
	
	public void animationmove(){
		
		Animator anim=GetComponent<Animator>();
		anim.SetBool("move",true);
		
	} 
	
	
	
	
}