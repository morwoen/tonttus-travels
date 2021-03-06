using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class JumpController : MonoBehaviour
{
  [SerializeField]
  private float jumpForce = 1;
  [SerializeField]
  private int maxJumps = 1;
  private int jumpCount = 0;
  private PlayerController player;
  private SlidingController slidingController;

  void Start() {
    player = GetComponent<PlayerController>();
    slidingController = GetComponent<SlidingController>();
  }

  void FixedUpdate() {
    if (player.isGrounded) {
      jumpCount = 0;
    }
  }

  public void OnJump(InputAction.CallbackContext context) {
    if (context.ReadValueAsButton()) {
      var isJumping = player.HasMovementEffect(typeof(JumpMovementEffect));
      if (!isJumping && jumpCount < maxJumps) {
        jumpCount++;
        player.AddMovementEffect(new JumpMovementEffect(jumpForce));

        if (slidingController?.isSliding == true) {
          // TODO: Add sliding effect/tell the sliding controller to stop
        }
      }
    }
  }
}
