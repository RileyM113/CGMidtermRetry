using UnityEngine;

public class BasicMovement: MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float sprintSpeed = 2f;

    private InputSystem_Actions controls;

    private Rigidbody rb;
    private bool isGrounded;

    private Vector2 moveInput;
    private float currentSpeed;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        float jump = controls.Player.Jump.ReadValue<float>();
        float sprint = controls.Player.Sprint.ReadValue<float>();
        bool interactPressed = controls.Player.Interact.WasPressedThisFrame();
        
        currentSpeed = speed;
        if (sprint > 0.5f)
        {
            currentSpeed = sprintSpeed;
        }
        
        if (jump > 0.5f && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2f * Physics.gravity.magnitude * jumpHeight);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.linearVelocity = new Vector3(move.x * currentSpeed, rb.linearVelocity.y, move.z * currentSpeed);
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}