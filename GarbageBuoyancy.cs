using UnityEngine;

public class GarbageBuoyancy : MonoBehaviour {

	public RangeFloat force = new RangeFloat(20, 30);
	[Range(0, 1)]
	public float drag;
	public bool disableGravityScale;

	private BoxCollider boxCollider;

	private void Awake() {
		boxCollider = GetComponent<BoxCollider>();
	}

	private void OnTriggerStay(Collider other) {

		float height = other.bounds.size.y;
		float percentSubmerged = Mathf.InverseLerp(other.bounds.min.y, other.bounds.max.y, transform.position.y);
		float percentUnder = Mathf.InverseLerp(boxCollider.bounds.min.y, boxCollider.bounds.max.y, other.bounds.center.y);

		other.attachedRigidbody.AddTorque(other.attachedRigidbody.angularVelocity * -drag, ForceMode.VelocityChange);
		other.attachedRigidbody.AddForce(other.attachedRigidbody.velocity * -drag, ForceMode.Impulse);
		// other.attachedRigidbody.velocity *= drag;
		if (percentSubmerged < 1)
			other.attachedRigidbody.AddForce(Vector3.up * force.min * percentSubmerged, ForceMode.Acceleration);
		else
			other.attachedRigidbody.AddForce(Vector3.up * force.GetAt(1 - percentUnder), ForceMode.Acceleration);
	}

	private void OnTriggerEnter(Collider other) {
		if (disableGravityScale) {
			GravityScale gravity = other.GetComponent<GravityScale>();
			if (gravity != null)
				gravity.enabled = false;
		}

		WaterDetector water = other.GetComponent<WaterDetector>();
		if (water != null)
			water.isUnderWater = true;
	}

	private void OnTriggerExit(Collider other) {
		if (disableGravityScale) {
			GravityScale gravity = other.GetComponent<GravityScale>();
			if (gravity != null)
				gravity.enabled = true;
		}

		WaterDetector water = other.GetComponent<WaterDetector>();
		if (water != null)
			water.isUnderWater = false;
	}
}