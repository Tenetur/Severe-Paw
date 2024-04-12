
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public CharacterController CharacterController;
    public Camera Camera;

    #region Movement

    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float runSpeed = 12;
    private float speed = 0;
    private Vector3 moveDirection;
    [HideInInspector] public bool IsRunning => isRunning;
    private bool isRunning = false;
    [HideInInspector] public bool IsGrounded() => CharacterController.isGrounded;

    #endregion

    #region Jump

    [SerializeField] private float jumpSpeed = 0.2f;
    [SerializeField] private int maxNumberOfJumps = 2;
    private int numberOfJumps = 0;

    #endregion

    #region Gravity

    [SerializeField] private float gravity = -0.3f;
    [SerializeField] private float gravityMultiplier = 1f;
    private float velocity;

    #endregion

    #region Rotation

    private readonly float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    #endregion

    #region KeyCodes

    public KeyCode runKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;

    #endregion

    private Vector3 GetDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }
    private void MoveAndRotation(in Vector3 direction, ref Vector3 moveDirection)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
    private void Sprint(Vector3 direction)
    {
        if (direction != null)
        {
            if (!isRunning && Input.GetKeyDown(runKey))
            {
                speed = runSpeed;
                isRunning = true;
            }
            else if ((IsRunning && Input.GetKeyDown(runKey)) || direction.magnitude < 0.1f)
            {
                speed = walkSpeed;
                isRunning = false;
            }
        }
    }
    private void Gravitation()
    {
        velocity += gravity * gravityMultiplier * Time.deltaTime;
        velocity = Mathf.Clamp(velocity, gravity, float.MaxValue);
        CharacterController.Move(new Vector3(0, velocity, 0) * Time.deltaTime);
    }
    private void JumpReset()
    {
        if (IsGrounded())
        {
            numberOfJumps = 0;
            Debug.Log("JumpReset true");
        }
    }
    private void Jump()
    {   
        if (!Input.GetKeyDown(jumpKey)) return;
        if (!IsGrounded() && numberOfJumps >= maxNumberOfJumps) return;
        
        Debug.Log("Jump!");

        numberOfJumps++;
        velocity = jumpSpeed;
    }

    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {   
        Vector3 direction = GetDirection();
        Sprint(direction);
        MoveAndRotation(direction, ref moveDirection);
        Jump();
        Gravitation();
        JumpReset();

        if (direction.magnitude >= 0.1f)
        {   
            MoveAndRotation(direction, ref moveDirection);
            CharacterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }
}
