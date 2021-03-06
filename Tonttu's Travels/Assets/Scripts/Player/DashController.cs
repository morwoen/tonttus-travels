using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class DashController : MonoBehaviour
{
  [SerializeField]
  private float dashSpeed = 40;
  [SerializeField]
  private float dashCooldown = 2;
  [SerializeField]
  private float dashDuration = 0.2f;

  private PlayerController player;
  private float remainingDashCooldown = 0f;

  void Start() {
    player = GetComponent<PlayerController>();
  }

  void Update() {
    if (remainingDashCooldown > 0) {
      remainingDashCooldown -= Time.deltaTime;
    }
  }

  public void Dash(InputAction.CallbackContext context) {
    if (!context.ReadValueAsButton()) return;

    if (remainingDashCooldown > 0) return;

    var isDashing = player.HasMovementEffect(typeof(DashMovementEffect));
    if (!isDashing) {
      player.AddOverwritingMovementEffect(new DashMovementEffect(transform.forward * dashSpeed, dashDuration));
      remainingDashCooldown = dashCooldown;
    }
  }
}
