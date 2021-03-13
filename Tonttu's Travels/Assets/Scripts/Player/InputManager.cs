using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
  public Vector2 movement
  {
    get;
    private set;
  }

  public void OnMove(InputAction.CallbackContext context) {
    movement = context.ReadValue<Vector2>().normalized;
  }
}
