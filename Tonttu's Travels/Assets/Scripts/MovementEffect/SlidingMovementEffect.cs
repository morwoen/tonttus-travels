using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingMovementEffect : MovementEffect
{
  public SlidingMovementEffect() : base() {
    Activate();
  }

  public void UpdateSelf(Vector3 hitNormal) {
    effect = new Vector3((1.0f - hitNormal.y) * hitNormal.x * Mathf.Abs(Physics.gravity.y), 0.0f, (1.0f - hitNormal.y) * hitNormal.z * Mathf.Abs(Physics.gravity.y));
  }
}
