using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public float minY = -90f; // Vertikal pastga qarash limiti
    public float maxY = 90f;  // Vertikal tepaga qarash limiti

    public float minX = -90f; // Gorizontal chapga qarash limiti
    public float maxX = 90f;  // Gorizontal o‘ngga qarash limiti

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertikal kamerani cheklash
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        // Gorizontal kamerani cheklash
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, minX, maxX);

        // Faqat kamera harakat qiladi
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}