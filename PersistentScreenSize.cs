using UnityEngine;

public class PersistentScreenSize : MonoBehaviour {


	private Camera mainCamera;
	private float defaultSize;
	private float scale = 1;

	private void Awake() {
		mainCamera = Camera.main;
		defaultSize = mainCamera.orthographicSize;
		scale = transform.localScale.x;
	}

	private void LateUpdate() {
		transform.localScale = (mainCamera.orthographicSize / defaultSize) * new Vector3(scale, scale, 0);
	}
}
