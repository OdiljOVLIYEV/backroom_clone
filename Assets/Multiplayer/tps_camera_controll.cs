using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class tps_camera_controll : MonoBehaviour
{
    [SerializeField] Transform aimPos;
    [SerializeField] float aimSmoothSpeed;
    [SerializeField] LayerMask aimMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenCentre = new Vector2(Screen.width/2, Screen.height/2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector2.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
    }
}
