using UnityEngine;

public class Picker : MonoBehaviour
{
    ThirdPersonCharacterController player;
    HUDScript hud;

    private void Start() {
        player = GetComponent<ThirdPersonCharacterController>();
        hud = player.hud;
    }

    private void OnTriggerEnter(Collider other) {
        string pickupTag = other.tag;

        if (pickupTag == "Pickup") {
            IPickable pickable = other.gameObject.GetComponent<IPickable>();
            pickable.Pick(player, hud);
        }
    }
}
