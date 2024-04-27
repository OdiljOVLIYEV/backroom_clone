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
	public float minDistanceToWall = 0.1f; // Minimum distance between camera and wall
	public float raycastSpreadAngle = 45f; // Angle to spread raycasts horizontally

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
			float distanceToObstacle = Mathf.Max(hit.distance - minDistanceToWall, minDistance);

			// Adjust desired position to be above ground
			desiredPosition = hit.point + transform.forward * distanceToObstacle;

			// Spread raycasts horizontally to check for side obstacles
			for (float angle = -raycastSpreadAngle; angle <= raycastSpreadAngle; angle += raycastSpreadAngle)
			{
				Vector3 direction = Quaternion.Euler(0, angle, 0) * -transform.forward;
				if (Physics.Raycast(player.position, direction, out hit, maxDistance, obstacleMask))
				{
					float sideDistance = Vector3.Distance(player.position, hit.point);
					if (sideDistance < distanceToObstacle)
					{
						desiredPosition = hit.point + transform.forward * minDistanceToWall; // Move slightly away from the wall
					}
				}
			}
		}

		// Move the camera to the desired position
		transform.position = desiredPosition;
	}
}
