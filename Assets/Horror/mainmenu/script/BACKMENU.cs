using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BACKMENU : MonoBehaviourPunCallbacks
{
	public GameObject mainMenu;
	public bool menu;
	public bool chqib_ketdi;
	
	void Update()
	{
		
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleMenu();
		}
	}

	void ToggleMenu()
	{
		if (!menu)
		{
			Cursor.lockState = CursorLockMode.Confined;
			menu = true;
			mainMenu.SetActive(true);
			Cursor.visible = true;
		}
		else
		{
			Cursor.visible = false;
			menu = false;
			mainMenu.SetActive(false);
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	
		
		
	
	 
}
