using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	private static ScreenShake staticRef;

	[HideInInspector] public bool enable = true; // makes the shake n push methods do nothin
	[HideInInspector] public bool isPaused = false; // pause the shit

	public Vector3 Output { get { return output + offset; } }

	[Header("//SETTING SHIT")]
	[Tooltip("should this b the instance u can reference statically?")]
	[SerializeField] private bool isStaticReference = false;
	[Tooltip("recentering behaviour")]
	[SerializeField] private SmoothType smoothType = SmoothType.Smooth;
	[Tooltip("what to shake n push")]
	[SerializeField] private OutputType outputType = OutputType.Self;
	[Space]
	[Tooltip("its the time... between shakes")]
	[SerializeField] private float timeBetweenShakes = .1f;
	[Tooltip("transform to set if Output Type is set to another transform")]
	[SerializeField] private Transform transformTarget;
	[Space]
	[Tooltip("n start should it set the offset to the targets position?")]
	[SerializeField] private bool setOffsetToStartPos = true;
	[Tooltip("how much to offset when reading the output")]
	[SerializeField] private Vector3 offset;
	[Header("//SMOOTH SHIT")]
	[Tooltip("how soon the shake recenters")]
	[SerializeField] private float recenterTime = .1f;
	[Header("//SPRING SHIT")]
	[Tooltip("how much to multiply spring force in")]
	[SerializeField] private float forceMultiplier = 69;
	[Tooltip("how loosy goosy the spring is")]
	[SerializeField] private float spring = 16;
	[Range(0, 1)]
	[Tooltip("how quickly the spring chills")]
	[SerializeField] private float damper = .35f;

	[HideInInspector]
	public Vector3 output;

	private Vector3 velRef;
	private Vector3 springVel;

	private void Awake() {
		if (isStaticReference) {
			if (staticRef != null) {
				Debug.LogError($"hey jsyk {name} is set to b the static reference when {staticRef.name} already is so im not gonna set it");
				isStaticReference = false;
			} else
				staticRef = this;
		}
	}

	private void Start() {
		if (setOffsetToStartPos && outputType != OutputType.None) {
			if (outputType == OutputType.Self)
				offset = transform.localPosition;
			else if (outputType == OutputType.OtherTransformLocal)
				offset = transformTarget.localPosition;
			else if (outputType == OutputType.OtherTransformWorld)
				offset = transformTarget.position;
		}
	}

	private void Update() {

		if (isPaused)
			return;

		// ~~ testing shit feel free to delete <3 ~~
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				if (!isStaticReference)
					Shake(1, 1);
			} else
				staticRef.Shake(1, 1);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				if (!isStaticReference)
					Shake(1, 2);
			} else
				staticRef.Shake(1, 2);
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				if (!isStaticReference)
					Shake(1, 3);
			} else
				staticRef.Shake(1, 3);
		}

		// smooth shit
		if (smoothType == SmoothType.Smooth) {
			output = Vector3.SmoothDamp(output, Vector3.zero, ref velRef, recenterTime);

		} else if (smoothType == SmoothType.Spring) {
			springVel += (-output) * spring - (springVel * damper);
			output += (Vector3)springVel * Time.deltaTime;
		}

		// clamping shit
		float minValue = .01f;
		if (output.magnitude < minValue) {
			if (smoothType == SmoothType.Smooth && velRef.magnitude < minValue)
				output = velRef = Vector3.zero;
			else if (smoothType == SmoothType.Spring && springVel.magnitude < minValue)
				output = springVel = Vector3.zero;
		}

		// output shit
		if (outputType == OutputType.None)
			return;

		if (outputType == OutputType.Self)
			transform.localPosition = Output;
		else if (outputType == OutputType.OtherTransformLocal)
			transformTarget.localPosition = Output;
		else if (outputType == OutputType.OtherTransformLocal)
			transformTarget.position = Output;
	}

	/// <summary>
	/// Pushes the target in a direction.
	/// </summary>
	/// <param name="direction">The direction of the push.</param>
	/// <param name="amount">The magnitude of the push</param>
	public void Push(Vector2 direction, float amount) => Push(direction * amount);

	/// <summary>
	/// Pushes the target in a direction.
	/// </summary>
	/// <param name="force">The vector force of the push direction.</param>
	public void Push(Vector2 force) {
		if (!enable)
			return;
		if (smoothType == SmoothType.Spring)
			springVel += (Vector3)force * forceMultiplier;
		else
			output += (Vector3)force;
	}

	/// <summary>
	/// Shakes the target some amount of times.
	/// </summary>
	/// <param name="intensity">How intense the shake is. For a SmoothType of Smooth this is how far off center it's pushed.</param>
	/// <param name="numShakes">The number of times to shake.</param>
	/// <param name="realtime">Should this ignore Time.timeScale?</param>
	public void Shake(float intensity, int numShakes, bool realtime = false) {
		StartCoroutine(DoShake(intensity, numShakes, realtime));
	}

	private IEnumerator DoShake(float intensity, int numShakes, bool realtime = false) {
		if (!enable)
			yield break;
		for (int i = 0; i < numShakes; i++) {
			Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			Push(dir * intensity);

			yield return StartCoroutine(PauseableWaitForSeconds(timeBetweenShakes, realtime));
		}
	}

	private IEnumerator PauseableWaitForSeconds(float duration, bool realtime = false) {
		for (float elapsed = 0; elapsed < duration; elapsed += (realtime?Time.unscaledDeltaTime : Time.deltaTime)) {
			while (isPaused)
				yield return null;
			yield return null;
		}
	}

	public enum SmoothType {
		Smooth,
		Spring
	}

	public enum OutputType {
		None,
		Self,
		OtherTransformLocal,
		OtherTransformWorld
	}
}