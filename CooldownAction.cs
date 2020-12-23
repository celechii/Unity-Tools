using System;

public class CooldownAction {

	/// <summary>
	/// No cooldown at 0, full cooldown at 1.
	/// </summary>
	public float percentRemaining => cooldownLeft / cooldownDuration;

	private Action action;
	public float cooldownDuration;
	public float cooldownLeft { get; private set; }
	private float bufferTime;
	private float bufferLeft;

	/// <param name="action">The action you want called.</param>
	/// <param name="cooldownDuration">How long to cooldown between action calls.</param>
	/// <param name="bufferTime">How long to hold onto an Invoke before losing it to the void.</param>
	public CooldownAction(float cooldownDuration, Action action = null, float bufferTime = 0) {
		if (action != null)
			this.action += action;
		this.cooldownDuration = cooldownDuration;
		this.bufferTime = bufferTime;
	}

	public void AddAction(Action action) => this.action += action;
	public void RemoveAction(Action action) => this.action -= action;

	/// <summary>
	/// Updates the timers with deltaTime.
	/// Might also call the action if its been buffered.
	/// </summary>
	public void UpdateTime(float deltaTime) {
		if (cooldownLeft > 0)
			cooldownLeft -= Math.Min(deltaTime, cooldownLeft);

		if (bufferLeft > 0) {
			bufferLeft -= Math.Min(deltaTime, bufferLeft);
			if (bufferLeft > 0 && cooldownLeft == 0)
				Invoke();
		}
	}

	/// <summary>
	/// Calls the action, returns true if the action can be called taking the buffer time into account.
	/// </summary>
	public bool Invoke() {
		if (action == null)
			return false;

		if (cooldownLeft <= 0) {
			action.Invoke();
			bufferLeft = 0;
			cooldownLeft = cooldownDuration;
			return true;
		} else {
			bufferLeft = bufferTime;
			return cooldownLeft <= bufferTime;
		}
	}
}