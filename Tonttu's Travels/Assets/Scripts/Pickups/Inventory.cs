using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  public List<Item> items = new List<Item>();

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Item")) {
      Item collidedItem = other.gameObject.GetComponent<Item>();

      items.Add(collidedItem);
      collidedItem.UseItem();
    }
  }
}
