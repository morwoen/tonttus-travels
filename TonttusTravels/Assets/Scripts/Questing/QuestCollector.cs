using System.Collections.Generic;
using UnityEngine;

public class QuestCollector : MonoBehaviour
{
  public LayerMask playerLayerMask;
  public List<Item> goals = new List<Item>();
  //public List<GameObject> placeholders = new List<GameObject>();
  

  private void FixedUpdate() {
    Collider[] players = Physics.OverlapSphere(transform.position, 10f, playerLayerMask);

    foreach (Collider player in players) {
      Inventory playerInventory = player.GetComponent<Inventory>();

      if (playerInventory != null) {
        List<Item> playerItems = playerInventory.items;

        foreach (Item playerItem in playerItems) {
          if (goals.Contains(playerItem)) {
            goals.Remove(playerItem);
            //GameObject placeholder = placeholders[0];
            //Vector3 placeholderPos = placeholder.transform.position;

            playerInventory.items.Remove(playerItem);
            //playerItem.Place(placeholderPos);
            //placeholder.SetActive(false);
            //placeholders.Remove(placeholder);

            if (goals.Count == 0) {
              GetComponent<Portal>().Activate();
            }
          }
        }
      }
    }
  }
}
