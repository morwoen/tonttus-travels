using UnityEngine;
using FSM;

public class JumpingState : StateBase
{
  private PlayerController player;
  private JumpController controller;

  public JumpingState(PlayerController player) : base(false) {
    controller = player.GetComponent<JumpController>();
    this.player = player;
  }

  public override void OnFixedUpdate() {
    player.SetMove(Vector3.up * Mathf.Abs(controller.jumpForce * Physics.gravity.y), Vector3.up);
    controller.DidJump();
  }
}
