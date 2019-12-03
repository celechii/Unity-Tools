using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour {

	public float spring = 50;
	[Range(0, 1)]
	public float damper = .35f;
	public Vector2 targetPos;

	private Vector2 vel;

	private void Update() {
		// credit this bit from wilhelm nylund @wilnyl
		// https://twitter.com/wilnyl/status/1201516498445058048?s=20
		vel += (targetPos - (Vector2)transform.localPosition) * spring - (vel * damper);
		transform.Translate(vel * Time.deltaTime);
	}

	/// <summary>
	/// Push the GameObject in a direction.
	/// </summary>
	/// <param name="dir">The direction of the force applied.</param>
	/// <param name="amount">The magnitude of the force applied.</param>
	public void Push(Vector2 dir, float amount) {
		Push(dir * amount);
	}

	/// <summary>
	/// Push the GameObject in a direction.
	/// </summary>
	/// <param name="force">The force to apply to the GameObject.</param>
	public void Push(Vector2 force) {
		vel += force;
	}

	/// <summary>
	/// Shake the GameObject.
	/// </summary>
	/// <param name="intensity">The intensity of the force applied.</param>
	/// <param name="duration">The duration of the shake.</param>
	/// <param name="frequency">How frequently the GameObject should be pushed in a different direction.</param>
	/// <param name="realTime">Is the frequency in real time or time as per the scaled time?</param>
	public void Shake(float intensity, float duration, float frequency = .1f, bool realTime = false) {
		StartCoroutine(DoShake(intensity, duration, frequency, realTime));
	}

	private IEnumerator DoShake(float intensity, float duration, float frequency, bool realTime) {
		for (float elapsed = 0; elapsed < duration; elapsed += (realTime?Time.unscaledDeltaTime : Time.deltaTime)) {

			Vector2 dir = ((new Vector2(Random.value, Random.value) - Vector2.one / 2f) * 2f).normalized;
			Push(dir * intensity);

			if (realTime)
				yield return new WaitForSecondsRealtime(frequency);
			else
				yield return new WaitForSeconds(frequency);
		}
	}
}