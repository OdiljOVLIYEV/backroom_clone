using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MultiPlayerNavMeshAI : MonoBehaviour
{
	public string playerTag = "Player"; // Tag for players
	public float playerRange = 10f; // Range within which to detect players
	public float roamRange = 20f; // Range for roaming around
	public float roamInterval = 5f; // Interval for changing roam destination
	public float speed = 5f;

	private List<Transform> players = new List<Transform>();
	private Transform currentTarget;
	private NavMeshAgent agent;
	private Vector3 roamDestination;
	private float lastRoamTime = 0f;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		// Find all GameObjects with the player tag and add their Transforms to the list
		GameObject[] playerObjs = GameObject.FindGameObjectsWithTag(playerTag);
		foreach (GameObject playerObj in playerObjs)
		{
			players.Add(playerObj.transform);
		}

		// Set the initial roam destination
		SetRandomRoamDestination();
	}

	void Update()
	{
		// Check for the closest player
		Transform closestPlayer = GetClosestPlayer();
        
		// If there are no players or no closest player in range, roam around
		if (closestPlayer == null || Vector3.Distance(transform.position, closestPlayer.position) > playerRange)
		{
			if (Time.time - lastRoamTime > roamInterval)
			{
				SetRandomRoamDestination();
				lastRoamTime = Time.time;
			}

			// Move towards the roam destination
			agent.SetDestination(roamDestination);
		}
		else
		{
			// Follow the closest player
			agent.SetDestination(closestPlayer.position);
		}
	}

	void SetRandomRoamDestination()
	{
		Vector3 randomDirection = Random.insideUnitSphere * roamRange;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, roamRange, 1);
		roamDestination = hit.position;
	}

	Transform GetClosestPlayer()
	{
		Transform closestPlayer = null;
		float closestDistance = Mathf.Infinity;
		foreach (Transform player in players)
		{
			float distance = Vector3.Distance(transform.position, player.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestPlayer = player;
			}
		}
		return closestPlayer;
	}

	void OnTriggerEnter(Collider other)
	{
		// Check if the AI collides with a player
		if (other.CompareTag(playerTag))
		{
			players.Add(other.transform);
		}
	}

	void OnTriggerExit(Collider other)
	{
		// Check if the player exits the trigger
		if (other.CompareTag(playerTag))
		{
			players.Remove(other.transform);
		}
	}
}
