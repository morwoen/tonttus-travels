using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class SlidingController : MonoBehaviour
{
  [SerializeField]
  private float maxSlidingSpeed = 20;
  private PlayerController player;
  private Vector3 slideDirection;

  public bool isSliding {
    get;
    private set;
  }

  void Start() {
    player = GetComponent<PlayerController>();
  }

  void OnControllerColliderHit(ControllerColliderHit hit) {
    if (hit.moveDirection.y >= -0.3f) return;

    if (Vector3.Angle(Vector3.up, hit.normal) > player.slopeLimit) {
      Vector3 c = Vector3.Cross(Vector3.up, hit.normal);
      Vector3 slideDirection = Vector3.Cross(c, hit.normal);

      var sameSlope = slideDirection == this.slideDirection;
      
      if (isSliding && sameSlope) return;
      isSliding = true;

      // Recreate the effect with the new direction if we are on a new slope
      if (!sameSlope) {
        player.RemoveMovementEffect(typeof(SlidingMovementEffect));
      }

      this.slideDirection = slideDirection;
      player.AddMovementEffect(new SlidingMovementEffect(slideDirection, maxSlidingSpeed));
    } else {
      isSliding = false;
      player.RemoveMovementEffect(typeof(SlidingMovementEffect));
    }
  }
}
