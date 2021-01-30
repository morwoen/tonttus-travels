using System.Collections;
using UnityEngine;

public class JumpBoost : MonoBehaviour, IPickable {
    public float duration;
    public float playerJumpBoostAmount;

    float previousJumpForce;

    IEnumerator JumpBoostLife(ThirdPersonCharacterController player) {
        OnPickupDestroy();
        yield return new WaitForSeconds(duration);
        player.jumpSpeed = previousJumpForce;
    }

    public void Pick(ThirdPersonCharacterController player) {
        previousJumpForce = player.jumpSpeed;

        player.jumpSpeed += playerJumpBoostAmount;
        StartCoroutine(JumpBoostLife(player));
    }

    public void OnPickupDestroy() {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, duration + 1);
    }
}
