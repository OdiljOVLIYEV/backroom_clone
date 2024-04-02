using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
	public GameObject[] cameras;
    
	void Start()
	{
		// Disable all cameras except the first one initially
		for (int i = 1; i < cameras.Length; i++)
		{
			cameras[i].SetActive(false);
		}
	}

	public void SwitchCamera(int playerIndex)
	{
		// Disable all cameras
		foreach (var cam in cameras)
		{
			cam.SetActive(false);
		}

		// Enable the camera for the corresponding player index
		if (playerIndex >= 0 && playerIndex < cameras.Length)
		{
			cameras[playerIndex].SetActive(true);
		}
		else
		{
			Debug.LogWarning("Player index out of range for cameras!");
		}
	}
}
