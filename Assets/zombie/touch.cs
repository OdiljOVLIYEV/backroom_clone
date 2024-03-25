using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class touch : MonoBehaviour
{




	public bool approach;
	private Transform playertransform;
	public float speed;
	public bool BarricadeDestroy;
	
	protected void Start()
	{

		playertransform = GameObject.FindGameObjectWithTag("Player").transform;
		BarricadeDestroy = false;

	}

	protected void Update()

	{
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(playertransform.transform.position);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("move", true);

      //  walk();
		

	}

	private void OnCollisionStay(Collision col)
	{
       /* if (col.gameObject.tag == "Enemy" )
        {
            

                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.speed = 0f;
           




        }
        else
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }*/




    

		if (col.gameObject.tag == "Player") {




			approach = true;
			zombieattack at = GetComponent<zombieattack>();
			at.attack = true;

			Animator anim = GetComponent<Animator>();
			anim.SetBool("attack", true);
			Invoke("enablmesh", 1f);

			if (approach == true) {
				NavMeshAgent agent = GetComponent<NavMeshAgent>();
				
				agent.speed = 0f;

			}





		}
		if (col.gameObject.GetComponent<MakeBarrekada>() != null)
			if (col.gameObject.GetComponent<MakeBarrekada>().barrikadaHealt > 0)
			{



				if (col.gameObject.tag == "taxta")
				{


					approach = true;
					zombieattack at = GetComponent<zombieattack>();
					at.attack = true;

					Animator anim = GetComponent<Animator>();
					anim.SetBool("attack", true);
					Invoke("enablmesh", 1f);

					if (approach == true)
					{
						NavMeshAgent agent = GetComponent<NavMeshAgent>();

						agent.speed = 0f;

					}
				}
			}
			else
			{
				Debug.Log("joni kichik");
				walk();
				zombieattack at = GetComponent<zombieattack>();
				at.attack = false;
				at.meshdisabled();

				Invoke("attacks", 1f);
			}

        /*if (col.gameObject.tag == "deraza")
		{


			timeline.Play();

        }*/








    }

    protected void OnCollisionExit(Collision col)
	{
		if(col.gameObject.tag=="Player"){
			 
			
			zombieattack at=GetComponent<zombieattack>();
			at.attack=false;
			at.meshdisabled();
			
			Invoke("attacks",1f);
			
			
		}
        if (col.gameObject.tag == "taxta")
        {


            zombieattack at = GetComponent<zombieattack>();
            at.attack = false;
            at.meshdisabled();

            Invoke("attacks", 1f);


        }
    }

	void walk()
	{
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(playertransform.transform.position);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("move", true);



    }


    void attacks(){
		  
		    approach=false;
			Animator anim=GetComponent<Animator>();
		anim.SetBool("attack",false);
		//anim.applyRootMotion=true;
			NavMeshAgent agent=GetComponent<NavMeshAgent>();
		agent.speed=speed;
		
	}
	void enablmesh(){
		
		
		zombieattack at=GetComponent<zombieattack>();
		at.meshenabled();
		
	}
}