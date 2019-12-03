using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour {

	public float spring = 50;
	[Range(0, 1)]
	public float damper = .35f;
	public Vector2 targetPos;

	private Vector2 vel;

	private void Update() {
		vel += (targetPos - (Vector2)transform.localPosition) * spring - (vel * damper);
		transform.Translate(vel * Time.deltaTime);
	}

	public void Push(Vector2 dir, float amount) {
		Push(dir * amount);
	}

	public void Push(Vector2 force) {
		vel += force;
	}

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