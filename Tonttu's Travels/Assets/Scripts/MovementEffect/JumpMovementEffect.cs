using UnityEngine;

public class JumpMovementEffect : MovementEffect {
  private float jumpForce;
  private bool applied = false;

  public JumpMovementEffect(float jumpForce) : base() {
    this.jumpForce = jumpForce;
  }

  public override Vector3 GetMovement(Vector3 velocity) {
    applied = true;
    return Vector3.up * Mathf.Abs(jumpForce * Physics.gravity.y);
  }

  public override bool IsActive() {
    return !applied;
  }
}
