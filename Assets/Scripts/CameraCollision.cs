using UnityEngine;

public class CameraCollision : MonoBehaviour
{
	public float minDistance = 1f; // Minimum distance from the camera to the target
	public float maxDistance = 5f; // Maximum distance from the camera to the target
	public float smooth = 10f; // Smoothing factor for camera movement
	public LayerMask collisionMask; // Layers to consider for collision detection

	private Transform target; // Target to follow, e.g., player transform
	private Vector3 dollyDirection; // Direction from the camera to the target
	private float distance; // Current distance from the camera to the target

	void Awake()
	{
		CameraRotation CAMERA=GetComponent<CameraRotation>();
	
		
		target = CAMERA.player.transform;
			dollyDirection = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	void Update()
	{
		// Calculate desired camera position
		Vector3 desiredCameraPosition = target.position - transform.forward * distance;

		// Perform raycast from target to camera
		RaycastHit hit;
		if (Physics.Linecast(target.position, desiredCameraPosition, out hit, collisionMask))
		{
			// Calculate new camera position along the collision normal
			Vector3 collisionNormal = hit.normal;
			Vector3 newCameraPosition = hit.point + collisionNormal * 0.1f; // Move slightly away from the wall
			distance = Mathf.Clamp((newCameraPosition - target.position).magnitude, minDistance, maxDistance);
		}
		else
		{
			// If no collision, move the camera to the desired position smoothly
			distance = maxDistance;
		}

		// Smoothly move the camera to the new position
		Vector3 targetCameraPosition = target.position - transform.forward * distance;
		transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * smooth);
	}
}
