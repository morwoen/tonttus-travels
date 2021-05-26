using UnityEngine;

public class Portal : MonoBehaviour {
  public Transform transportTo;
  bool isActive = false;
  //Animator animator;
  public GameObject winScreenUIPanel;

  //private void Start() {
  //  animator = GetComponent<Animator>();
  //}

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("Player") && isActive) {
      winScreenUIPanel.SetActive(true);
      //other.gameObject.transform.position = transportTo.position;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  public void Activate() {
    isActive = true;
  }
}
