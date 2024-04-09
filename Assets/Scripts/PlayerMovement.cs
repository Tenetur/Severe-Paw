
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    public CharacterController controller;
    public Camera cam;

    #region Movement

    [SerializeField] private float MoveSpeed = 6f; 
    private Vector3 moveDirection;
    public static bool isRunning { set; get; }
    private bool IsGrounded => controller.isGrounded;

    #endregion

    #region Jump

    [SerializeField] private float jumpSpeed = 0.2f;
    private int numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

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
        if (IsGrounded && velocity < 0.0f)
            velocity = -0.1f;
        else
            velocity += gravity * gravityMultiplier * Time.deltaTime;
    }
    private void Jump(ref float velocity)
    {
        if (!Input.GetKeyDown(jumpKey)) return;
        if (!IsGrounded && numberOfJumps >= maxNumberOfJumps) return;
        if (numberOfJumps == 0) StartCoroutine(WaitForLanding());

        Debug.Log("Jump!");
        numberOfJumps++;
        velocity = jumpSpeed;

    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded);
        yield return new WaitUntil(() => IsGrounded);

        numberOfJumps = 0;
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
