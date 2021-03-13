using System;

namespace FSM
{
  public class State : StateBase
  {
    private Action<State> onEnter;
    private Action<State> onUpdate;
    private Action<State> onFixedUpdate;
    private Action<State> onExit;
    private Func<State, bool> canExit;

    public Timer timer;

    /// <summary>
    /// Initialises a new instance of the State class
    /// </summary>
    /// <param name="onEnter">A function that is called when the state machine enters this state</param>
    /// <param name="onUpdate">A function that is called by the Update of the state machine if this state is active</param>
    /// <param name="onFixedUpdate">A function that is called by the FixedUpdate of the state machine if this state is active</param>
    /// <param name="onExit">A function that is called when the state machine exits this state</param>
    /// <param name="canExit">(Only if needsExitTime is true):
    /// 	Called when a state transition from this state to another state should happen.
    /// 	If it can exit, it should call fsm.StateCanExit()
    /// 	and if it can not exit right now, later in OnLogic() it should call fsm.StateCanExit()</param>
    /// <param name="needsExitTime">Determins if the state is allowed to instantly
    /// 	exit on a transition (false), or if the state machine should wait until the state is ready for a
    /// 	state change (true)</param>
    public State(
            Action<State> onEnter = null,
            Action<State> onUpdate = null,
            Action<State> onFixedUpdate = null,
            Action<State> onExit = null,
            Func<State, bool> canExit = null,
            bool needsExitTime = false) : base(needsExitTime) {
      this.onEnter = onEnter;
      this.onUpdate = onUpdate;
      this.onFixedUpdate = onFixedUpdate;
      this.onExit = onExit;
      this.canExit = canExit;

      this.timer = new Timer();
    }

    override public void OnEnter() {
      timer.Reset();

      if (onEnter != null) onEnter(this);
    }

    override public void OnUpdate() {
      if (onUpdate != null) onUpdate(this);
    }

    override public void OnFixedUpdate() {
      if (onFixedUpdate != null) onFixedUpdate(this);
    }

    override public void OnExit() {
      if (onExit != null) onExit(this);
    }

    override public void RequestExit() {
      if (!needsExitTime || canExit != null && canExit(this)) {
        fsm.StateCanExit();
      }
    }
  }
}
