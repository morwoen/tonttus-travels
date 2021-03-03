using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingJumpMovementEffect : MovementEffect
{
  private float jumpPushForce = 5.0f;

  public SlidingJumpMovementEffect(float timer) : base(timer) { }

  public void UpdateSelf(Vector3 hitNormal) {
    effect = new Vector3((1.0f - hitNormal.y) * hitNormal.x * Mathf.Abs(Physics.gravity.y) * jumpPushForce, 0.0f, (1.0f - hitNormal.y) * hitNormal.z * Mathf.Abs(Physics.gravity.y) * jumpPushForce);
  }

  public void UpdateTimer() {
    if (currentTimer <= 0) {
      Deactivate();
    } else {
      currentTimer -= Time.fixedDeltaTime;
    }
  }

  public Vector3 Apply() {
    var test = base.Apply();
    Debug.Log(test);
    return test;
  }
}
