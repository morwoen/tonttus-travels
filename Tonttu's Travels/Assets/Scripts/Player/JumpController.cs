using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class JumpController : MonoBehaviour
{
  public float jumpForce = 1;
  [SerializeField]
  private int maxJumps = 1;
  private int jumpCount = 0;
  
  public bool doingJump {
    get;
    private set;
  } = false;

  private PlayerController player;
  private SlidingController slidingController;

  void Start() {
    player = GetComponent<PlayerController>();
    slidingController = GetComponent<SlidingController>();
  }

  void Update() {
    if (!doingJump && player.isGrounded) {
      jumpCount = 0;
    }
  }

  public void DidJump() {
    if (doingJump) {
      jumpCount++;
      doingJump = false;
    }
  }

  public void OnJump(InputAction.CallbackContext context) {
    if (context.ReadValueAsButton()) {
      if (!doingJump && jumpCount < maxJumps) {
        doingJump = true;

        // Have only 1 mid air jump if you walk off a platform
        if (!player.isGrounded && jumpCount == 0) {
          jumpCount++;
        }

        if (slidingController?.isSliding == true) {
          // TODO: Add sliding effect/tell the sliding controller to stop
        }
      }
    }
  }
}
