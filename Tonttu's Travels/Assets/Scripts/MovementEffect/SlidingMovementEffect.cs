using UnityEngine;

public class SlidingMovementEffect : MovementEffect
{
  private Vector3 direction;
  private float maxSlideSpeed;

  public SlidingMovementEffect(Vector3 direction, float maxSlideSpeed) : base() {
    this.direction = direction;
    this.maxSlideSpeed = maxSlideSpeed;
  }

  public override Vector3 GetMovement(Vector3 velocity) {
    // Slide direction * vertical velocity (clamped to the maxSlideSpeed * the intensity of the slope)
    return direction * Mathf.Clamp(Mathf.Abs(velocity.y), 0, maxSlideSpeed * Mathf.Abs(direction.y));
  }
}
