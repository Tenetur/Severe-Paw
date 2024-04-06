using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    public CharacterController controller;
    public Camera cam;

    [SerializeField] private float MoveSpeed = 6f;
    [SerializeField] private float jumpSpeed = 0.01f;
    [SerializeField] private float gravity = -0.05f;
    [SerializeField] private float gravityMultiplier = 1f;
    private float velocity;

    private Vector3 moveDirection;
    public static bool isRunning { set; get; }
    private bool isGrounded => controller.isGrounded;

    private readonly float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public KeyCode runKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    private Vector3 GetDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }
    private void MoveAndRotation(in Vector3 direction, ref Vector3 moveDirection)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
    private void Sprint(ref float speed, in Vector3 direction)
    {
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
    }
    private void Gravitation(ref float velocity)
    {
        if (isGrounded && velocity < 0.0f)
            velocity = -0.5f;
        else
            velocity += gravity * gravityMultiplier * Time.deltaTime;
    }
    private void Jump(ref float velocity)
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity += jumpSpeed;
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        isRunning = false;
    }

    void Update()
    {
        Vector3 direction = GetDirection();

        Jump(ref velocity);
        Gravitation(ref velocity);
        Sprint(ref MoveSpeed, direction);
        MoveAndRotation(direction, ref moveDirection);

        if (direction.magnitude >= 0.1f)
        {   
            MoveAndRotation(direction, ref moveDirection);
            controller.Move(moveDirection.normalized * MoveSpeed * Time.deltaTime);
        }   
        controller.Move(new Vector3(0, velocity, 0));
    }
}
