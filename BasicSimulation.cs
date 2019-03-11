using UnityEngine;

// grabbed from https://docs.unity3d.com/ScriptReference/Physics2D.Simulate.html

public class BasicSimulation : MonoBehaviour {
	[Range(0, 1)]
	private float timer;

	void FixedUpdate() {
		if (Physics2D.autoSimulation)
			return; // do nothing if the automatic simulation is enabled

		timer += Time.deltaTime;

		// Catch up with the game time.
		// Advance the physics simulation in portions of Time.fixedDeltaTime
		// Note that generally, we don't want to pass variable delta to Simulate as that leads to unstable results.
		while (timer >= Time.fixedDeltaTime) {
			timer -= Time.fixedDeltaTime;
			Physics2D.Simulate(Time.fixedDeltaTime * Settings.actual.gameSpeed);
		}

		// Here you can access the transforms state right after the simulation, if needed...
	}
}