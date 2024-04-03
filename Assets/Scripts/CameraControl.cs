using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float SensivityX;
    public float SensivityY;

    public Transform orientation;

    float xRotation;
    float yRotation;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensivityY;
    }
}
