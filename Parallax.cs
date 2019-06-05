using UnityEngine;

public class Parallax : MonoBehaviour {

	[Range(0, 1)]
	public float influence;
	public bool reverse;
	public Vector2 offset;

	private Vector2 initPos;
	private Transform mainCam;

	private void Awake() {
		mainCam = Camera.main.transform;
		initPos = transform.localPosition;
	}

	private void LateUpdate() {
		transform.localPosition = Vector2.Lerp(initPos, ((Vector2)mainCam.transform.localPosition * (reverse? - 1 : 1)) + offset, influence);
	}

}