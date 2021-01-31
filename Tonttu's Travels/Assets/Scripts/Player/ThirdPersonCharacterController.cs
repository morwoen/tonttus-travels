﻿using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
  public GameObject cam;

  private Animator animator;
  private Rigidbody rb;
  private Collision groundCollission;

  #region IO
  private float hor;
  private float ver;
  #endregion

  #region Movement
  public float deadzone = 0.1f;
  public float turnTime = 0.1f;
  public float glidingSpeed = 1.5f;
  public float movementSpeed = 3.0f;

  private float turnSmoothVelocity;
  private float internalSpeed = 4.0f;
  #endregion

  #region Jumping
  public int maxJumps = 2;
  public float jumpSpeed = 6.0f;
  public float airJumpSpeed = 3.0f;
  public float jumpCooldown = 0.3f;

  private int currentJump = 0;
  private bool onGround = true;
  private bool isJumping = false;
  private bool letGoOfJump = true;
  private float jumpTimer = 0.0f;
  #endregion

  #region Sprinting
  public float maxStamina = 5.0f;
  public float sprintSpeed = 6.0f;
  public float staminaDrain = 3.0f;
  public float staminaRegen = 1.0f;

  private float stamina = 0.0f;
  private bool isExhausted = false;
  private bool isSprinting = false;
  #endregion

  #region Dashing
  public float dashSpeed = 8.0f;
  public float dashCooldown = 3.0f;
  public float dashDuration = 10.0f;

  private float dashTimer = 0.0f;
  private bool isDashing = false;
  #endregion

  #region Stealth
  public float stealthSpeed = 1.5f;

  private bool isInStealth = false;
  private bool usingStealthAxis = false;
  #endregion

  #region Climbing
  public float climbingSpeed = 2.0f;
  public float dismountSpeed = 1.0f;
  public float ropeTurnSpeed = 200.0f;

  private bool isClimbing = false;
  private Transform rope = null;
  #endregion

  #region HUD
  public HUDScript hud;
  #endregion

  #region Utilities
  float GetSpeed(float modifyer)
  {
    return internalSpeed * modifyer;
  }

  void StopDashVelocity()
  {
    rb.velocity = Vector3.zero;
  }
  #endregion

  #region Action Checks
  void CheckForMovement()
  {
    hor = Input.GetAxisRaw("Horizontal");
    ver = Input.GetAxisRaw("Vertical");
  }

  void CheckForJump()
  {
    if (Input.GetAxis("Jump") == 0) {
      letGoOfJump = true;
    }

    jumpTimer -= Time.deltaTime;
    jumpTimer = Mathf.Clamp(jumpTimer, 0.0f, jumpCooldown);

    if (letGoOfJump && (onGround || isClimbing || currentJump < maxJumps) && Input.GetAxis("Jump") != 0 && jumpTimer == 0)
    {
      isJumping = true;
      letGoOfJump = false;
      currentJump++;
      jumpTimer = jumpCooldown;
    }
  }

  void CheckForSprint()
  {
    if (onGround && !isSprinting && stamina > 0 && Input.GetAxis("Fire1") != 0 && !isExhausted)
    {
      isSprinting = true;
      isInStealth = false;
    }

    if (isSprinting && stamina == 0)
    {
      isSprinting = false;
      isExhausted = true;
    }

    if (isSprinting && (Input.GetAxis("Fire1") == 0 || !onGround))
    {
      isSprinting = false;
    }

    if (isSprinting)
    {
      stamina -= staminaDrain * Time.deltaTime;
      stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);
    }
    else
    {
      stamina += staminaRegen * Time.deltaTime;
      stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);
    }

    if (stamina == maxStamina)
    {
      isExhausted = false;
    }

    hud.SetSprint(stamina, maxStamina);
  }

  void CheckForDash()
  {
    dashTimer -= Time.deltaTime;
    dashTimer = Mathf.Clamp(dashTimer, 0.0f, dashCooldown);
    hud.SetDash(dashTimer, dashCooldown);

    if (Input.GetAxis("Fire2") != 0 && dashTimer == 0)
    {
      isDashing = true;
      isInStealth = false;
      dashTimer = dashCooldown;
    }
  }

  void CheckForStealth()
  {
    if (Input.GetAxis("Fire3") != 0 && !usingStealthAxis)
    {
      isInStealth = !isInStealth;
      usingStealthAxis = true;
    }

    if (Input.GetAxis("Fire3") == 0)
    {
      usingStealthAxis = false;
    }
  }
  #endregion

  #region Action Handlers
  void HandleMovement()
  {
    Vector3 inputDir = new Vector3(hor, 0f, ver).normalized;

    if (inputDir.magnitude >= deadzone && !isDashing)
    {
      float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);

      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      float speed = onGround ? isSprinting ? GetSpeed(sprintSpeed) : GetSpeed(movementSpeed) : GetSpeed(glidingSpeed);
      if (isSprinting)
      {
        speed = GetSpeed(sprintSpeed);
        animator.SetFloat("Locomotion", 1.0f);
      }
      else if (isInStealth)
      {
        speed = GetSpeed(stealthSpeed);
      }
      else if (!onGround)
      {
        speed = GetSpeed(glidingSpeed);
      }
      else
      {
        speed = GetSpeed(movementSpeed);
        animator.SetFloat("Locomotion", 0.5f);
      }

      Vector3 moveDir = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;
      rb.MovePosition(transform.position + moveDir.normalized * speed * Time.fixedDeltaTime);
    }
    else
    {
      animator.SetFloat("Locomotion", 0.0f);
    }
  }

  void HandleJump()
  {
    if (isJumping)
    {
      animator.SetTrigger("Jump");
      isJumping = false;
      float speed = onGround ? jumpSpeed : airJumpSpeed;
      rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
      onGround = false;
    }
  }

  void HandleDash()
  {
    if (isDashing)
    {
      isDashing = false;
      rb.AddForce(transform.forward * GetSpeed(dashSpeed), ForceMode.Impulse);
      Invoke("StopDashVelocity", dashDuration * Time.fixedDeltaTime);
    }
  }

  void HandleClimbing()
  {
    rb.useGravity = false;
    rb.velocity = Vector3.zero;
    Vector3 direction = new Vector3(0.0f, ver, 0.0f).normalized;
    var verticalMovement = transform.position + direction * climbingSpeed * Time.fixedDeltaTime;

    Quaternion q = Quaternion.AngleAxis(-hor * ropeTurnSpeed * Time.fixedDeltaTime, Vector3.up);
    var horizontalMovement = q * (rb.transform.position - rope.transform.position) + rope.transform.position;

    horizontalMovement.y = verticalMovement.y;

    rb.MovePosition(horizontalMovement);
    rb.MoveRotation(rb.transform.rotation * q);

    if (isJumping)
    {
      rb.useGravity = true;
      rb.AddForce(-transform.forward * GetSpeed(glidingSpeed), ForceMode.Impulse);
      rb.AddForce(Vector3.up * dismountSpeed, ForceMode.Impulse);
      isJumping = false;
      isClimbing = false;
    }
  }

  void HandleGroundDetection()
  {
    RaycastHit hit;

    Vector3 origin = transform.position;
    origin.y += 0.1f;
    bool hasHit = Physics.Raycast(origin, Vector3.down, out hit, 0.2f);

    if (hasHit)
    {
      onGround = true;
      currentJump = 0;
      animator.SetBool("isFalling", false);
    }
    else
    {
      animator.SetBool("isFalling", true);
    }
  }
  #endregion

  #region Collisions
  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.CompareTag("rope") && !isClimbing)
    {
      rope = collider.gameObject.transform;
      Vector3 direction = collider.gameObject.transform.position;
      direction.y = transform.position.y;
      transform.LookAt(direction);

      isClimbing = true;
      onGround = false;
      currentJump = 0;
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.gameObject.CompareTag("rope"))
    {
      rope = null;
      isClimbing = false;
      rb.useGravity = true;
    }
  }
  #endregion

  void Start()
  {
    rb = GetComponentInChildren<Rigidbody>();
    animator = GetComponentInChildren<Animator>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  void Update()
  {
    CheckForMovement();
    CheckForJump();
    CheckForSprint();
    CheckForDash();
    CheckForStealth();
  }

  void FixedUpdate()
  {
    if (!isJumping)
    {
      HandleGroundDetection();
    }

    if (!isClimbing)
    {
      HandleMovement();
      HandleJump();
      HandleDash();
    }
    else
    {
      HandleClimbing();
    }
  }
}
