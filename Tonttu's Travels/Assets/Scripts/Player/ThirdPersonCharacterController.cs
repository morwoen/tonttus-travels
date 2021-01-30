﻿using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
  public GameObject cam;

  private Rigidbody rb;

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
  private float jumpTimer = 0.0f;
  #endregion

  #region Sprinting
  public float maxStamina = 5.0f;
  public float sprintSpeed = 6.0f;
  public float staminaDrain = 3.0f;
  public float staminaRegen = 1.0f;

  private float stamina = 0.0f;
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
    jumpTimer -= Time.deltaTime;
    jumpTimer = Mathf.Clamp(jumpTimer, 0.0f, jumpCooldown);

    if ((onGround || currentJump < maxJumps) && Input.GetAxis("Jump") != 0 && jumpTimer == 0)
    {
      isJumping = true;
      currentJump++;
      jumpTimer = jumpCooldown;
    }
  }

  void CheckForSprint()
  {
    if (stamina > 0 && Input.GetAxis("Fire1") != 0 && onGround)
    {
      isSprinting = true;
      isInStealth = false;
    }
    else
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
  }

  void CheckForDash()
  {
    dashTimer -= Time.deltaTime;
    dashTimer = Mathf.Clamp(dashTimer, 0.0f, dashCooldown);

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
      }

      Vector3 moveDir = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;
      rb.MovePosition(transform.position + moveDir.normalized * speed * Time.fixedDeltaTime);
    }
  }

  void HandleJump()
  {
    if (isJumping)
    {
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
  #endregion

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      onGround = true;
      currentJump = 0;
    }
  }

  void Start()
  {
    rb = GetComponentInChildren<Rigidbody>();
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
    HandleMovement();
    HandleJump();
    HandleDash();
  }
}
