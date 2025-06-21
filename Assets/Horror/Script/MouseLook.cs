using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
	public Transform playerBody;
	
	public float mouseSensitivity = 100f;
	
	float xRotation = 0f;

    //public PhotonView photonView;

   // [SerializeField] Transform aimPos;
   // [SerializeField] float aimSmoothSpeed;
   // [SerializeField] LayerMask aimMask;
    // Start is called before the first frame update
    void Start()
    {
	    // Cursor.lockState = CursorLockMode.Locked;
       
    }

    // Update is called once per frame
    void Update()
    {
       /* if (!photonView.IsMine)
        {
            return;
        }*/
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
	    float mouseY = Input.GetAxis("Mouse Y") *mouseSensitivity * Time.deltaTime;
	   
	    xRotation -=mouseY;
	    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
	     transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
	    playerBody.Rotate(Vector3.up * mouseX);

       /* Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector2.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);*/
    }
}
