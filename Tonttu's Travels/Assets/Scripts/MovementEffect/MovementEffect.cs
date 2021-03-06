using UnityEngine;

public abstract class MovementEffect {
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

  public void Tick() {
    if (!isTimed) return;
    if (timer <= 0) return;
    timer -= Time.deltaTime;
  }

  public virtual bool IsActive() {
    return !isTimed || timer > 0;
  }

  // Movement Effects are applied in FixedUpdate, always use Time.fixedDeltaTime
  // velocity is the player velocity so far
  public abstract Vector3 GetMovement(Vector3 velocity);
}
