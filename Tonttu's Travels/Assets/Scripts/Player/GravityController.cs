using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class GravityController : MonoBehaviour
{
  void Start() {
    GetComponent<PlayerController>().AddMovementEffect(new GravityMovementEffect());
  }
}
