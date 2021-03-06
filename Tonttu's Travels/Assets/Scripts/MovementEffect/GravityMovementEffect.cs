using UnityEngine;

public class GravityMovementEffect : MovementEffect {
  public GravityMovementEffect() : base() {}

  public override Vector3 GetMovement(Vector3 velocity) {
    return Physics.gravity * Time.fixedDeltaTime;
  }
}
