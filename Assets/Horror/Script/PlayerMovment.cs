using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMovment : MonoBehaviourPunCallbacks
{
	public CharacterController controller;

	public Transform groundCheck;

	public LayerMask groundMask;

	private damagepis anima;
	//public PhotonView pw;

	public float speed = 5f;
	public float gravity = -9.8f;
	public float groundDistans = 0.4f;

	public float jumpHeight = 3f;
	[HideInInspector]
	public float z;
	public float x;
	Vector3 velocity;
	public bool isGrounded;
	public float speedwalk;
	public bool jumpidle=false;  
	private AudioSource source;
	public Animator anim;
   
    // Start is called before the first frame update
    void Start()
	{
		source = GetComponent<AudioSource>();
		//pw=GetComponent<PhotonView>();

	}

	// Update is called once per frame
	void Update()
	{
		if (photonView.IsMine)
		{
		
			if (Input.GetKey(KeyCode.W))
			{
				anim.SetBool("crouchwalk", true);
				anim.SetBool("Walk", true);
				
				anim.SetBool("walkback", false);
				
			}
			else

		if (Input.GetKey(KeyCode.A))
			{
			anim.SetBool("crouchwalk", true);
				anim.SetBool("Walk", true);
			anim.SetBool("walkback", false);
				
			}

			else


		if (Input.GetKey(KeyCode.D))
		{
			anim.SetBool("crouchwalk", true);
				anim.SetBool("Walk", true);
			anim.SetBool("walkback", false);
				
			}

			else

		if (Input.GetKey(KeyCode.S))
			{

			anim.SetBool("crouchwalk", true);
			anim.SetBool("walkback", true);
				anim.SetBool("Walk", false);
				
			}
			else
			{
				anim.SetBool("crouchwalk", false);
				anim.SetBool("Walk", false);
				anim.SetBool("walkback", false);
				

				}
		}
		
		/*if (Input.GetButtonDown("Fire1"))
		{
			attackpistol();

			Invoke("attackpistolback", 0.7f);
		}*/


		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");

		

		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistans, groundMask);
		if (isGrounded && velocity.y < 0) {
			velocity.y = -2f;
		}
		if(isGrounded==false){
				
			anim.SetBool("JumpIdle", true);
			jumpidle=true;	
		}
		
		if(jumpidle==true&&isGrounded==true){
			anim.SetBool("JumpUp", false);
			anim.SetBool("JumpIdle", false);
			jumpidle=false;	
		}
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
			Debug.Log("sakradi");
			anim.SetBool("JumpUp", true);
			
		
		}

		if (Input.GetKey("left ctrl"))
		{   anim.SetBool("crouch", true);
			controller.height = 1f;
			
		} else
		{  anim.SetBool("crouch", false);
			controller.height = 2f;
			anim.SetBool("crouchwalk", false);
		}
		/*if (Input.GetKeyDown("left shift") && z == 0)
		{

			

		}*/
		if (Input.GetKeyDown("left shift") && z >= 0) {

			source.Play();

		} else {


		}
		if (Input.GetKey("left shift") && z == 0)
		{
			source.Stop();
           
        }
		else if (Input.GetKey("left shift")&& z == 1f)
		{
			anim.SetBool("Run", true);
			//anim.SetBool("Walk", false);
            source.loop = true;
			anima = FindObjectOfType<damagepis>();
			speed = 6f;
            

        } else
		{

			anim.SetBool("Run", false);
	     
            source.loop = false;
			// velocitys = 0f;
			speed = 3f;
           
        }

		if (z == -1f) {

			speed = 3f;
           

        }


		Vector3 move = transform.right * x + transform.forward * z;
		controller.Move(move * speed * Time.deltaTime);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);



	}
 
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag=="Player"&&isGrounded==false){
			
			anim.SetBool("JumpIdle", false);
		}
		
		
	}
   
}
