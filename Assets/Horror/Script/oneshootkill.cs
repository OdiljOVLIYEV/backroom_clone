using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class oneshootkill : MonoBehaviour
{
	public float healt=1f;
	public CapsuleCollider capsuleCollider;
	public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void TakeDamage (float amount){
		
		healt-=amount;
		if(healt<=0f){
			Animator anim=FindObjectOfType<Animator>();
			anim.SetBool("dead",true);
			
			capsuleCollider.enabled=false;
			NavMeshAgent agent=GetComponent<NavMeshAgent>();
			agent.speed=0f;
			//	if(anim.agent.speed!=null)
			//anim.agent.speed=0f;
			//Debug.Log("DEAD");
			
			
			
			Die();
		}
		
	}
	
	private void Die(){
		
	
	
		
			
		Destroy(gameObject,50f);
			
		
	}
		
}
