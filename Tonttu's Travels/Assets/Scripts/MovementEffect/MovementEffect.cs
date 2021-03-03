using UnityEngine;

public class MovementEffect : Object {
  protected bool isTimed;
  protected bool isActive;
  protected float timer;
  protected float currentTimer;
  protected string type;
  protected Vector3 effect;

  protected MovementEffect() {
    isTimed = false;
  }
  
  protected MovementEffect(float timer) {
    isTimed = true;
    this.timer = timer;
  }

  public void Activate() {
    isActive = true;
    ResetTimer();
  }

  protected void Deactivate() {
    isActive = false;
  }

  protected void ResetTimer() {
    currentTimer = timer;
  }

  public Vector3 Apply() {
    if (isActive) {
      return effect;
    }

    return Vector3.zero;
  }
}
