using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour
{
    public GameObject cam;
    public float turnTime;
    public float movementSpeed;
    public float jumpForce;

    private Vector3 hitNormal;
    private float currentSpeed;
    private Vector3 movementDir;
    private CharacterController cc;
    private bool isSliding = false;
    private float turnSmoothVelocity;

    private void HandleHorizontalDirection()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
        float yVelocity = movementDir.y;

        if (inputDir.magnitude > 0)
        {
            float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            movementDir = Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * currentSpeed;
        }
        else
        {
            movementDir = Vector3.zero;
        }

        movementDir.y = yVelocity;
    }

    private void HandleVerticalDirection()
    {

        if (cc.isGrounded)
        {
            if (isSliding)
            {
                movementDir.x += (1.0f - hitNormal.y) * hitNormal.x * Mathf.Abs(Physics.gravity.y);
                movementDir.z += (1.0f - hitNormal.y) * hitNormal.z * Mathf.Abs(Physics.gravity.y);
            }
            else
            {
                movementDir.y = Mathf.Max(movementDir.y, 0.0f);
            }

            Vector3 jumpVelocity = new Vector3(0.0f, Mathf.Abs(Input.GetAxisRaw("Jump") * jumpForce * Physics.gravity.y), 0.0f);
            movementDir += jumpVelocity;
        }

        movementDir += Physics.gravity * Time.deltaTime;
    }

    void Start()
    {
        currentSpeed = movementSpeed;
        cc = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        HandleHorizontalDirection();
        HandleVerticalDirection();

        cc.Move(movementDir * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
        if (Vector3.Angle(Vector3.up, hitNormal) > cc.slopeLimit)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
    }
}
