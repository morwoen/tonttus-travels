using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour
{
  public GameObject cam;

  [Header("Movement")]
  public float turnTime = 0.2f;
  public float movementSpeed = 5;
  public float jumpForce = 1;
  public float pushPower = 1;

  private Vector3 hitNormal;
  private float currentSpeed;
  private Vector3 movementDir;
  private CharacterController cc;
  private bool isSliding = false;
  private float turnSmoothVelocity;

  private Vector3 movement = new Vector3();
  private bool doJump = false;
  private int jumpCount = 0;

  [Header("Dashing")]
  public float dashSpeed = 40;
  private bool doDash = false;
  // TODO: How do we group it?
  public float dashDuration = 0.2f;
  private float remainingDashDuration = 0f;
  public float dashCooldown = 2;
  private float remainingDashCooldown = 0f;


  private void HandleHorizontalDirection() {
    Vector3 inputDir = movement.normalized;
    float yVelocity = movementDir.y;

    if (doDash) {
      movementDir = transform.forward * dashSpeed;

      remainingDashDuration -= Time.fixedDeltaTime;
      if (remainingDashDuration <= 0) {
        doDash = false;
      }
    } else if (inputDir.magnitude > 0) {
      float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);
      transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      movementDir = Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * currentSpeed;
    } else {
      movementDir = Vector3.zero;
    }

    movementDir.y = yVelocity;
  }

  // TODO: Should we merge the two, the vertical does sliding as well?
  private void HandleVerticalDirection() {
    if (cc.isGrounded) {
      if (isSliding) {
        movementDir.x += (1.0f - hitNormal.y) * hitNormal.x * Mathf.Abs(Physics.gravity.y);
        movementDir.z += (1.0f - hitNormal.y) * hitNormal.z * Mathf.Abs(Physics.gravity.y);
      } else {
        movementDir.y = Mathf.Max(movementDir.y, 0.0f);
      }
    }

    if (doJump) {
      doJump = false;

      if (isSliding) {
        // TODO: keep the force (without the *10) for some period
        movementDir.x += (1.0f - hitNormal.y) * hitNormal.x * Mathf.Abs(Physics.gravity.y) * 10;
        movementDir.z += (1.0f - hitNormal.y) * hitNormal.z * Mathf.Abs(Physics.gravity.y) * 10;
      }

      if (jumpCount < 2) {
        movementDir.y = Mathf.Abs(jumpForce * Physics.gravity.y);
        jumpCount++;
      }
    }

    movementDir += Physics.gravity * Time.fixedDeltaTime;
  }

  void Start() {
    currentSpeed = movementSpeed;
    cc = GetComponent<CharacterController>();
  }

  private void Update() {
    // Capture input
    if (Input.GetButtonDown("Jump")) {
      doJump = true;
    }

    if (!doDash && remainingDashCooldown <= 0 && Input.GetButtonDown("Fire2")) {
      doDash = true;
      remainingDashDuration = dashDuration;
      remainingDashCooldown = dashCooldown;
    }

    movement.x = Input.GetAxisRaw("Horizontal");
    movement.z = Input.GetAxisRaw("Vertical");

    // Tick cooldowns
    if (remainingDashCooldown > 0) {
      remainingDashCooldown -= Time.deltaTime;
    }

    if (cc.isGrounded) {
      jumpCount = 0;
    }
  }

  void FixedUpdate() {
    HandleHorizontalDirection();
    HandleVerticalDirection();

    cc.Move(movementDir * Time.fixedDeltaTime);
  }

  void OnControllerColliderHit(ControllerColliderHit hit) {
    if (hit.moveDirection.y < -0.3f) {
      hitNormal = hit.normal;
      if (Vector3.Angle(Vector3.up, hitNormal) > cc.slopeLimit) {
        isSliding = true;
      } else {
        isSliding = false;
      }
      return;
    }

    Rigidbody body = hit.collider.attachedRigidbody;

    // The other object doesnt have a rigid body
    if (body == null || body.isKinematic)
      return;

    // Calculate push direction from move direction,
    // we only push objects to the sides never up and down
    Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

    // Apply the push (multiply by the velocity of the player)
    body.velocity = pushDir * pushPower * cc.velocity.magnitude;
  }
}
