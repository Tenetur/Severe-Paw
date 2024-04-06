using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    public CinemachineFreeLook cam;

    [Header("Îבחמנ ךאלונû")]

    public float defaultFOV = 45.0f;
    [SerializeField] private float runFOV;
    [Space]
    [SerializeField] private float toDefaultSpeed = 0.05f;
    [SerializeField] private float toActionSpeed = 0.05f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        runFOV = defaultFOV + 3f;
        if (PlayerMovement.isRunning)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, runFOV, toActionSpeed);
        }
        else
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, defaultFOV, toDefaultSpeed);
        }

    }
}
