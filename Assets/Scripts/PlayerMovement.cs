using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    public CharacterController controller;
    public new Camera camera;

    [SerializeField]
    private float MoveSpeed = 6f;
    private float moveDown = 5f;

    public static bool isRunning { set; get; }

    private readonly float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public KeyCode runKey = KeyCode.LeftControl;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        isRunning = false;
        MoveSpeed = 6f;
    }

    void Update()
    {     
        // ��������� ����������� ��������
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // �������� � ���� ��������
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * MoveSpeed * Time.deltaTime);
        }
        
        // ��� ����
        if (!isRunning && Input.GetKeyDown(runKey) && direction != null) 
        {
            MoveSpeed = 12f;
            isRunning = true;
        }
        else if ((isRunning && Input.GetKeyDown(runKey)) || direction.magnitude < 0.1f) 
        {
            MoveSpeed = 6f;
            isRunning = false;
        }

        // ������� ����, ���� �� �� �����
        if (!controller.isGrounded)
        {
            Vector3 downMove = new Vector3(0f, -1f, 0f);
            controller.Move(downMove * moveDown);
        }
    }
}
