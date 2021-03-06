using UnityEngine;

public class Picker : MonoBehaviour
{
  private PlayerController player;
  private HUDScript hud;

  private void Start() {
    player = GetComponent<PlayerController>();
    hud = FindObjectOfType<HUDScript>();
  }

  private void OnTriggerEnter(Collider other) {
    string pickupTag = other.tag;

    if (pickupTag == "Pickup") {
      IPickable pickable = other.gameObject.GetComponent<IPickable>();
      if (pickable == null) {
        pickable = other.gameObject.GetComponentInParent<IPickable>();
      }
      pickable.Pick(player, hud);
    }
  }
}
