using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_AIMLOOK : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float sensitivity = 5.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY += Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, -90f, 90f);
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }
}
