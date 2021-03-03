using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour {
  public GameObject cam;

  [Header("Movement")]
  public float jumpForce = 1;
  public float pushPower = 1;
  public float turnTime = 0.2f;
  public float movementSpeed = 5;
  public float jumpPushDuration = 400000.0f;

  private int jumpCount = 0;
  private bool isSliding = false;
  private bool doJump = false;
  private float currentSpeed;
  private float turnSmoothVelocity;
  private Vector3 movementDir;
  private Vector3 movement = new Vector3();
  private CharacterController cc;

  [Header("Dashing")]
  public float dashSpeed = 40;
  public float dashCooldown = 2;
  public float dashDuration = 0.2f;

  // TODO: How do we group it?
  private bool doDash = false;
  private float remainingDashDuration = 0f;
  private float remainingDashCooldown = 0f;

  private GravityMovementEffect gravityEffect;
  private SlidingMovementEffect slidingEffect;
  private SlidingJumpMovementEffect slidingJumpEffect;

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
        movementDir += slidingEffect.Apply();
      } else {
        movementDir.y = 0.0f;
      }
    }

    movementDir += slidingJumpEffect.Apply();

    if (doJump) {
      doJump = false;

      if (cc.isGrounded && isSliding) {
        slidingJumpEffect.Activate();
      }

      if (jumpCount < 2) {
        movementDir.y = Mathf.Abs(jumpForce * Physics.gravity.y);
        jumpCount++;
      }
    }

    movementDir += gravityEffect.Apply();
  }

  void Start() {
    currentSpeed = movementSpeed;
    cc = GetComponent<CharacterController>();
    gravityEffect = new GravityMovementEffect();
    slidingEffect = new SlidingMovementEffect();
    slidingJumpEffect = new SlidingJumpMovementEffect(jumpPushDuration);
  }

  private void Update() {
    gravityEffect.UpdateSelf();
    slidingJumpEffect.UpdateTimer();
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
      if (Vector3.Angle(Vector3.up, hit.normal) > cc.slopeLimit) {
        slidingEffect.UpdateSelf(hit.normal);
        slidingJumpEffect.UpdateSelf(hit.normal);
        isSliding = true;
      } else {
        isSliding = false;
      }
      return;
    }

    // Push other objects:
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