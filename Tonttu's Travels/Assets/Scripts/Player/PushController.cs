using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PushController : MonoBehaviour
{
  [SerializeField]
  private float pushPower = 1;

  private PlayerController player;

  void Start() {
    player = GetComponent<PlayerController>();
  }

  void OnControllerColliderHit(ControllerColliderHit hit) {
    Rigidbody body = hit.collider.attachedRigidbody;
    if (body == null || body.isKinematic)
      return;

    // Calculate push direction from move direction,
    // we only push objects to the sides never up and down
    Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

    // Apply the push (multiply by the velocity of the player)
    body.velocity = pushDir * pushPower * player.velocity.magnitude;
  }
}
