using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using FSM;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

  [Header("Movement")]
  public float turnTime = 0.2f;
  [SerializeField]
  private float movementSpeed = 5;

  private Vector3 movementDir;
  private StateMachine fsm;
  private CharacterController cc;
  private SlidingController slidingController;

  private List<MovementEffect> movementEffects = new List<MovementEffect>();
  public float currentSpeed
  {
    get;
    private set;
  }

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

    fsm = new StateMachine(this);

    fsm.AddState("Movement", new MovementState(this));
    fsm.AddState("Jumping", new JumpingState(this));

    var jumpController = GetComponent<JumpController>();
    fsm.AddTransition(new Transition(
        "Movement",
        "Jumping",
        (transition) => jumpController.doingJump
    ));

    fsm.AddTransition(new Transition(
        "Jumping",
        "Movement",
        (transition) => !jumpController.doingJump
    ));

    // This configures the entry point of the state machine
    fsm.SetStartState("Movement");
    // Initialises the state machine and must be called before OnLogic() is called
    fsm.OnEnter();
  }

  private void Update() {
    fsm.OnUpdate();

    // Tick && clear the movement effects
    foreach (var effect in movementEffects) {
      effect.Tick();
    }

    movementEffects.RemoveAll(effect => !effect.IsActive());
  }

  void FixedUpdate() {
    float yVelocity = movementDir.y;

    if (isGrounded) {
      movementDir.y = 0.0f;
    } else {
      movementDir.y = yVelocity;
    }

    fsm.OnFixedUpdate();

    foreach (var effect in movementEffects) {
      // execute all in priority overwriting or adding stuff
      movementDir += effect.GetMovement(movementDir);
    }

    cc.Move(movementDir * Time.fixedDeltaTime);
  }

  public void AddMove(Vector3 movement) {
    movementDir += movement;
  }

  public void SetMove(Vector3 movement, Vector3 axis) {
    var keptMomentum = new Vector3(
      axis.x == 1 ? 0 : movementDir.x,
      axis.y == 1 ? 0 : movementDir.y,
      axis.z == 1 ? 0 : movementDir.z
    );
    movementDir = movement + keptMomentum;
  }

  public bool HasMovementEffect(Type t) {
    foreach (var effect in movementEffects) {
      if (effect.GetType() == t) {
        return true;
      }
    }

    return false;
  }

  public void AddMovementEffect(MovementEffect effect) {
    movementEffects.Add(effect);
  }

  public void RemoveMovementEffect(Type effectType) {
    movementEffects.RemoveAll(effect => effect.GetType() == effectType);
  }
}
