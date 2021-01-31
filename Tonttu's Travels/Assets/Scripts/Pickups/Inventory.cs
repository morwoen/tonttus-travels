using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  public List<Item> items = new List<Item>();
  AudioSource audioSource;

  private void Start() {
    audioSource = GetComponent<AudioSource>();
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Item")) {
      Item collidedItem = other.gameObject.GetComponent<Item>();

      items.Add(collidedItem);

      audioSource.PlayOneShot(collidedItem.pickupAudio);
      collidedItem.UseItem();
    }
  }
}
