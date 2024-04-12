using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Observer : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [Header("Îבחמנ ךאלונû")]

    [SerializeField] private float defaultFOV = 45.0f;
    [SerializeField] private float runFOV;
    [Space]
    [SerializeField] private float toDefaultSpeed = 0.01f;
    [SerializeField] private float toActionSpeed = 0.01f;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        runFOV = defaultFOV + 5f;
        if (playerMovement.IsRunning)
        {
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(cinemachineFreeLook.m_Lens.FieldOfView, runFOV, toActionSpeed);
        }
        else
        {
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(cinemachineFreeLook.m_Lens.FieldOfView, defaultFOV, toDefaultSpeed);
        }

    }
}
