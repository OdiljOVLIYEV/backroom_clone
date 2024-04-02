using UnityEngine;

public class CameraRotation : MonoBehaviour
{
	public Transform player; // Player to follow
	public LayerMask obstacleMask; // LayerMask for obstacles
	public float rotationSpeed = 2.0f;
	public float maxDistance = 5.0f;
	public float minYAngle = -90f; // Minimum vertical angle
	public float maxYAngle = 90f; // Maximum vertical angle
	public float minDistance = 2.0f; // Minimum distance to player
	public float groundOffset = 0.2f; // Offset from ground to avoid clipping

	private float mouseX, mouseY;

	void Update()
	{
		if (player == null)
		{
			Debug.LogWarning("Player not assigned to CameraRotation.");
			return;
		}

		// Get mouse movement
		mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
		mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

		// Clamp the vertical rotation to avoid flipping
		mouseY = Mathf.Clamp(mouseY, minYAngle, maxYAngle);

		// Rotate the player horizontally
		player.rotation = Quaternion.Euler(0, mouseX, 0);

		// Rotate the camera vertically
		transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

		// Calculate the desired camera position behind the player
		Vector3 desiredPosition = player.position - transform.forward * maxDistance;

		// Check for obstacles between camera and player
		RaycastHit hit;
		if (Physics.Raycast(player.position, -transform.forward, out hit, maxDistance, obstacleMask))
		{
			// If there's an obstacle, adjust desired position
			float distanceToObstacle = Vector3.Distance(player.position, hit.point);

			// If the distance to the obstacle is less than the minimum distance, move closer
			if (distanceToObstacle < minDistance)
			{
				desiredPosition = player.position - transform.forward * minDistance;
			}

			// Adjust desired position to be above ground
			desiredPosition.y = hit.point.y + groundOffset;
		}

		// Move the camera to the desired position
		transform.position = desiredPosition;
	}
}
