using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

  [Header("Movement")]
  [SerializeField]
  private float turnTime = 0.2f;
  [SerializeField]
  private float movementSpeed = 5;
  private float currentSpeed;
  private float turnSmoothVelocity;
  private Vector2 movement = Vector2.zero;
  private Vector3 movementDir;
  private Transform cam;

  private CharacterController cc;
  private SlidingController slidingController;

  private List<MovementEffect> movementEffects = new List<MovementEffect>();
  private MovementEffect overwritingEffect;

  public bool isGrounded {
    get => cc.isGrounded && (slidingController?.isSliding != true);
  }

  public float slopeLimit
  {
    get => cc.slopeLimit;
  }

  public Vector3 velocity
  {
    get => cc.velocity;
  }

  void Start() {
    currentSpeed = movementSpeed;
    cc = GetComponent<CharacterController>();
    slidingController = GetComponent<SlidingController>();
    cam = Camera.main.transform;
  }

  private void HandleMovement() {
    float yVelocity = movementDir.y;

    if (overwritingEffect != null) {
      movementDir = overwritingEffect.GetMovement(movementDir);
    } else if (movement.magnitude > 0) {
      float inputAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);
      transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      movementDir = Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * currentSpeed;
    } else {
      movementDir = Vector3.zero;
    }

    if (isGrounded) {
      movementDir.y = 0.0f;
    } else {
      movementDir.y = yVelocity;
    }

    foreach (var effect in movementEffects) {
      movementDir += effect.GetMovement(movementDir);
    }
  }

  private void Update() {
    // Tick && clear the movement effects
    foreach (var effect in movementEffects) {
      effect.Tick();
    }

    if (overwritingEffect != null) {
      overwritingEffect.Tick();
      if (!overwritingEffect.IsActive()) {
        overwritingEffect = null;
      }
    }

    movementEffects.RemoveAll(effect => !effect.IsActive());
  }

  void FixedUpdate() {
    HandleMovement();
    cc.Move(movementDir * Time.fixedDeltaTime);
  }

  public void OnMove(InputAction.CallbackContext context) {
    movement = context.ReadValue<Vector2>().normalized;
  }

  public bool HasMovementEffect(Type t) {
    if (overwritingEffect != null && overwritingEffect.GetType() == t) {
      return true;
    }

    foreach (var effect in movementEffects) {
      if (effect.GetType() == t) {
        return true;
      }
    }

    return false;
  }

  public void AddOverwritingMovementEffect(MovementEffect effect) {
    overwritingEffect = effect;
  }

  public void AddMovementEffect(MovementEffect effect) {
    movementEffects.Add(effect);
  }

  public void RemoveMovementEffect(Type effectType) {
    if (overwritingEffect != null && overwritingEffect.GetType() == effectType) {
      overwritingEffect = null;
    }

    movementEffects.RemoveAll(effect => effect.GetType() == effectType);
  }
}
