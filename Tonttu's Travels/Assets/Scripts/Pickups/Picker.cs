using UnityEngine;

public class Picker : MonoBehaviour
{
    ThirdPersonCharacterController player;

    private void Start() {
        player = GetComponent<ThirdPersonCharacterController>();
    }

    private void OnTriggerEnter(Collider other) {
        string pickupTag = other.tag;

        if (pickupTag == "Pickup") {
            IPickable pickable = other.gameObject.GetComponent<IPickable>();
            pickable.Pick(player);
        }
    }
}
