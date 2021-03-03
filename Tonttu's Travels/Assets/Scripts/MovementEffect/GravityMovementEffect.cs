using UnityEngine;

public class GravityMovementEffect : MovementEffect {
  public GravityMovementEffect() : base() {
    Activate();
  }

  public void UpdateSelf() {
    effect = Physics.gravity * Time.fixedDeltaTime;
  }
}
