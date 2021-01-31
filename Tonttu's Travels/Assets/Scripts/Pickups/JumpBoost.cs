using System.Collections;
using UnityEngine;

public class JumpBoost : MonoBehaviour, IPickable
{
  public float duration = 3f;
  public float playerJumpBoostAmount = 3f;
  public float respawnTimeInSeconds = 5f;
  public AudioSource pickupSound;

  public GameObject pickupModel;

  private float previousJumpForce;
  private bool pickedUp = false;

  IEnumerator JumpBoostLife(ThirdPersonCharacterController player, HUDScript hud) {
    previousJumpForce = player.jumpSpeed;
    player.jumpSpeed += playerJumpBoostAmount;

    hud.SetJumpBoost(duration);

    yield return new WaitForSeconds(duration);
    player.jumpSpeed = previousJumpForce;
    pickedUp = false;
  }

  IEnumerator PickMe() {
    pickupModel.SetActive(false);
    yield return new WaitForSeconds(respawnTimeInSeconds);
    pickupModel.SetActive(true);
  }

  public void Pick(ThirdPersonCharacterController player, HUDScript hud) {
    if (pickedUp) return;
    pickedUp = true;
    pickupSound.Play();
    StartCoroutine(JumpBoostLife(player, hud));
    StartCoroutine(PickMe());
  }
}
