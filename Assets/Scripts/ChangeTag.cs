using UnityEngine;

public class ChangeTag : MonoBehaviour
{
	public string newTag = "Dead";

	void Start()
	{
		// Change the tag when the script starts
		gameObject.tag = newTag;
	}

	void Update()
	{
		// For demonstration, change the tag when a key is pressed (you can use any condition)
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ChangeGameObjectTag();
		}
	}

	void ChangeGameObjectTag()
	{
		// Change the tag to the newTag value
		gameObject.tag = newTag;

		Debug.Log("Tag changed to: " + newTag);
	}
}
