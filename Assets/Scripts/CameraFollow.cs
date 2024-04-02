using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target; // Player's transform to follow

	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}

	void Update()
	{
		if (target != null)
		{
			transform.position = target.position;
			transform.LookAt(target);
		}
	}
}
