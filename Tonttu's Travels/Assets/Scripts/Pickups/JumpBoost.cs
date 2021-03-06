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
  private float previousAirJumpForce;
  private bool pickedUp = false;

  IEnumerator JumpBoostLife(PlayerController player, HUDScript hud) {
    //previousJumpForce = player.jumpSpeed;
    //previousAirJumpForce = player.airJumpSpeed;
    //player.jumpSpeed += playerJumpBoostAmount;
    //player.airJumpSpeed += playerJumpBoostAmount;

    hud?.SetJumpBoost(duration);

    yield return new WaitForSeconds(duration);
    //player.jumpSpeed = previousJumpForce;
    //player.airJumpSpeed = previousAirJumpForce;
    pickedUp = false;
  }

  IEnumerator PickMe() {
    pickupModel.SetActive(false);
    yield return new WaitForSeconds(respawnTimeInSeconds);
    pickupModel.SetActive(true);
  }

  public void Pick(PlayerController player, HUDScript hud) {
    if (pickedUp) return;
    pickedUp = true;
    pickupSound.Play();
    StartCoroutine(JumpBoostLife(player, hud));
    StartCoroutine(PickMe());
  }
}
