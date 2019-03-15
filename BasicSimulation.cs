using UnityEngine;

// grabbed from https://docs.unity3d.com/ScriptReference/Physics2D.Simulate.html

public class BasicSimulation : MonoBehaviour {

	private float timer;

	private void Start() {
		SetGameSpeed(Settings.Actual.gameSpeed);
	}

	private void Update() {
		if (Physics2D.autoSimulation)
			return;

		timer += Time.deltaTime * Settings.Actual.gameSpeed;

		while (timer >= Time.fixedDeltaTime) {
			timer -= Time.fixedDeltaTime;
			Physics2D.Simulate(Time.fixedDeltaTime);
		}
	}

	public static void SetGameSpeed(float gameSpeed) {
		if (gameSpeed >= 1f)
			Time.fixedDeltaTime = .005f;
		else
			Time.fixedDeltaTime = .001f;
	}
}