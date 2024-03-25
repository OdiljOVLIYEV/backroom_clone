using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Target : MonoBehaviour
{
	//public GameObject game;
	public float healt=50f;
	
	public bool dead;

	public PhotonView pw;
	private NavMeshAgent agent;
	public GameObject BX;
	public SphereCollider SC;
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		
		pw=GetComponent<PhotonView>();
		agent=GetComponent<NavMeshAgent>();
		
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		
	}
   

    [PunRPC]
    public void TakeDamage (float amount){
		Debug.Log("ZARAR KO'RMOQDA");

		healt-=amount;
		if(healt<=0f){
            SpawnEnemy enemy = FindObjectOfType<SpawnEnemy>();
            if (enemy != null)
                enemy.MAX_ZOMBIE -=1;

            CapsuleCollider capsuleCollider=GetComponent<CapsuleCollider>(); 
			capsuleCollider.enabled=false;
			SC.enabled = false;
			BX.GetComponent<SphereCollider>().enabled = false;
			agent=GetComponent<NavMeshAgent>();
			agent.speed = 0f;
			// if(anim.agent.speed!=null)
			// anim.agent.speed=0f;
			//Debug.Log("DEAD");
			offlineDead();

            // pw.RPC("EnemyAnim", RpcTarget.All);
			 //Invoke("Enemydeath", 3f); 
			
		}
		
	}

	public void Enemydeath()
	{
		pw.RPC("Die", RpcTarget.All);
        

    }
	[PunRPC]
	public void EnemyAnim() {

        Animator anim = GetComponent<Animator>();
        anim.SetBool("dead", true);
    }

	[PunRPC]
	private void Die(){



		PhotonNetwork.Destroy(gameObject);
			
		
			
		
	}
		
	public void offlineDead()
	{
       
        EnemyAnim();
        Destroy(gameObject,3f);

    }	
	
			
		
	
}
