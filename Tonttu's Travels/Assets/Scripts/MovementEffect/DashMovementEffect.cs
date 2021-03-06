using UnityEngine;

public class DashMovementEffect : MovementEffect {
  private Vector3 movement;

  public DashMovementEffect(Vector3 movement, float duration) : base(duration) {
    this.movement = movement;
  }

  public override Vector3 GetMovement(Vector3 velocity) {
    return movement;
  }
}
