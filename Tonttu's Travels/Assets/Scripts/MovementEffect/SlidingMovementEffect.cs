using UnityEngine;

public class SlidingMovementEffect : MovementEffect
{
  private Vector3 direction;
  private float maxSlideSpeed;
  private bool firstFrame = true;

  public SlidingMovementEffect(Vector3 direction, float maxSlideSpeed) : base() {
    this.direction = direction;
    this.maxSlideSpeed = maxSlideSpeed;
  }

  public override Vector3 GetMovement(Vector3 velocity) {
    // Slide direction * vertical velocity (clamped to the maxSlideSpeed * the intensity of the slope)
    Debug.Log(direction);
    var move = direction * Mathf.Clamp(Mathf.Abs(velocity.y), 0, maxSlideSpeed * Mathf.Abs(direction.y));
    if (firstFrame) {
      firstFrame = false;
      move.y = velocity.y;
    }

    return move;
  }
}
