using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  public List<Item> items = new List<Item>();
  AudioSource audioSource;
  HUDScript hud;

  private void Start() {
    audioSource = GetComponent<AudioSource>();

    hud = GetComponent<ThirdPersonCharacterController>().hud;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Item")) {
      Item collidedItem = other.gameObject.GetComponent<Item>();

      switch (collidedItem.itemName) {
        case "Quill":
          hud.ToggleQuill();
          break;
        case "Lighter":
          hud.ToggleLighter();
          break;
        case "Ring":
          hud.ToggleRing();
          break;
        case "Coin":
          hud.ToggleCoin();
          break;
        case "Key":
          hud.ToggleKey();
          break;
      }

      items.Add(collidedItem);

      collidedItem.UseItem();
    }
  }
}
