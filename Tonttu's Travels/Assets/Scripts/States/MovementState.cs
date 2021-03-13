using UnityEngine;
using FSM;

public class MovementState : StateBase
{
  private PlayerController player;
  private float turnSmoothVelocity;
  private Transform cam;
  private InputManager inputManager;
  private Vector3 axis;

  public MovementState(PlayerController player) : base(false) {
    this.player = player;
    cam = Camera.main.transform;
    inputManager = player.GetComponent<InputManager>();
    axis = Vector3.right + Vector3.forward;
  }

  public override void OnFixedUpdate() {
    var movement = inputManager.movement;
    if (movement.magnitude > 0) {
      float inputAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, player.turnTime);
      player.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      player.SetMove(Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * player.currentSpeed, axis);
    } else {
      player.SetMove(Vector3.zero, axis);
    }
  }
}
