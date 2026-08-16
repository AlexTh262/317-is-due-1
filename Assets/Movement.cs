using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed;
    float compoundSpeed;
    public Transform orientation;

    public float groundDrag;

    float horizontal;
    float vertical;

    Vector3 moveDirection;

    public static Rigidbody rb;

    public static bool isSprinting;
    public static bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    void Update()
    {
        MovementInput();
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            compoundSpeed = moveSpeed + 2;
            isSprinting = true;
        } else
        {
            compoundSpeed = moveSpeed;
            isSprinting = false;
        }

        rb.linearDamping = groundDrag;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void MovementInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void PlayerMove()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        rb.AddForce(moveDirection * compoundSpeed * 10f, ForceMode.Force);
        if (rb.linearVelocity == Vector3.zero)
        {
            isMoving = false;
        } else
        {
            isMoving = true;
        }

    }

    public static void freezeMovement()
    {
        if (isMoving)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}