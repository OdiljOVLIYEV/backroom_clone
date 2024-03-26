using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Detector : MonoBehaviourPunCallbacks
{
	public Transform[] waypoints;
	public float speed = 3.5f;

	private int currentWaypointIndex = 0;
	private NavMeshAgent agent;

	public string playerTag = "Player";
	public int fieldOfView = 45;
	public int viewDistance = 30;

	private bool playerDetected;
	private Transform nearestPlayer;

	private bool isAware = false;
	private bool isDetecting = false;
	private float loseThreshold = 10f; // 5 minutes in seconds
	private float loseTimer = 0f;
	private Animator anim;

	private PhotonView photonView;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		photonView = GetComponent<PhotonView>();
		anim = GetComponent<Animator>(); // Assign the Animator component

		if (waypoints.Length > 0)
		{
			agent.SetDestination(waypoints[currentWaypointIndex].position);
			animationmove();
		}
	}

	void Update()
	{
		if (!photonView.IsMine)
		{
			return;
		}

		if (!isAware)
		{
			if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
			{
				currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
				agent.SetDestination(waypoints[currentWaypointIndex].position);
				//animationmove();
			}
		}

		DetectPlayer();

		if (isAware)
		{
			anim.SetBool("run", true);
			agent.speed = 5f;
			agent.SetDestination(nearestPlayer.position);

			loseTimer += Time.deltaTime;
			if (loseTimer >= loseThreshold)
			{
				isAware = false;
				isDetecting = false;
				loseTimer = 0f;
				agent.speed = 3f;
				anim.SetBool("run", false);
			}
		}
		else
		{
			// If not aware, reset the lose timer
			loseTimer = 0f;
		}
	}

	void DetectPlayer()
	{
		RaycastHit hitInfo;
		Transform closestPlayer = null;
		float closestDistance = Mathf.Infinity;

		foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag(playerTag))
		{
			Transform playerTransform = playerObj.transform;

			if (playerTransform == transform)
			{
				continue; // Skip this player (it's the AI itself)
			}

			float distance = Vector3.Distance(transform.position, playerTransform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestPlayer = playerTransform;
			}
		}

		if (closestPlayer != null && Vector3.Angle(closestPlayer.position - transform.position, transform.forward) < fieldOfView)
		{
			if (Physics.Raycast(transform.position, closestPlayer.position - transform.position, out hitInfo, viewDistance))
			{
				if (hitInfo.transform.CompareTag(playerTag))
				{
					playerDetected = true;
					nearestPlayer = closestPlayer;
					isAware = true;
					isDetecting = true;
				}
				else
				{
					playerDetected = false;
					isDetecting = false;
				}
			}
			else
			{
				playerDetected = false;
				isDetecting = false;
			}
		}
		else
		{
			playerDetected = false;
			isDetecting = false;
		}
	}
	private void OnDrawGizmos()
	{
		if (!Application.isEditor && nearestPlayer.position == null)
		{
			return;
		}

		if (playerDetected == true)
		{

			Debug.DrawLine(transform.position, nearestPlayer.position, Color.green);
		}


		Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
		Vector3 leftRayPoint = Quaternion.Euler(0f, -fieldOfView, 0f) * frontRayPoint;
		Vector3 rightRayPoint = Quaternion.Euler(0f, fieldOfView, 0f) * frontRayPoint;

		Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
		Debug.DrawLine(transform.position, leftRayPoint, Color.blue);
		Debug.DrawLine(transform.position, rightRayPoint, Color.blue);
	}

	public void animationmove()
	{
		anim.SetBool("move",true);
	}
}
