using System.Collections;
using UnityEngine;

public class JumpBoost : MonoBehaviour, IPickable {
    public float duration = 3f;
    public float playerJumpBoostAmount = 3f;
    public float respawnTimeInSeconds = 5f;

    public GameObject pickupModel;

    float previousJumpForce;

    IEnumerator JumpBoostLife(ThirdPersonCharacterController player) {
        previousJumpForce = player.jumpSpeed;
        player.jumpSpeed += playerJumpBoostAmount;
        
        yield return new WaitForSeconds(duration);
        player.jumpSpeed = previousJumpForce;
    }

    IEnumerator PickMe() {
        pickupModel.SetActive(false);
        yield return new WaitForSeconds(respawnTimeInSeconds);
        pickupModel.SetActive(true);
    }

    public void Pick(ThirdPersonCharacterController player) {
        StartCoroutine(JumpBoostLife(player));
        StartCoroutine(PickMe());
    }
}
