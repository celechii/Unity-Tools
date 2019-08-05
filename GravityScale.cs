using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScale : MonoBehaviour {

	public float gravityScale = 1;

	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		rb.AddForce(Vector3.down * (-gravityScale + 1) * Physics.gravity.y * rb.mass);
	}

}