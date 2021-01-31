using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IUsable
{
  public string itemID;
  public string itemName;

  public void UseItem() {
    gameObject.SetActive(false);
  }

  public void Place(Vector3 pos) {
    gameObject.transform.position = pos;
    gameObject.SetActive(true);
  }
}
