using UnityEngine;

public class Portal : MonoBehaviour {
  public Transform transportTo;
  bool isActive = false;
  //Animator animator;

  //private void Start() {
  //  animator = GetComponent<Animator>();
  //}

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("Player") && isActive) {
      other.gameObject.transform.position = transportTo.position;
    }
  }

  public void Activate() {
    isActive = true;
  }
}
