using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{
  private CinemachineFreeLook _freeLookComponent;

  void Start() {
    _freeLookComponent = GetComponent<CinemachineFreeLook>();
  }

  public void OnLook(InputAction.CallbackContext context) {
    Vector2 lookMovement = context.ReadValue<Vector2>().normalized;
    _freeLookComponent.m_YAxis.m_InputAxisValue = lookMovement.y;
    _freeLookComponent.m_XAxis.m_InputAxisValue = lookMovement.x;
  }
}
