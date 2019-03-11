using UnityEngine;

public class CursorControl : MonoBehaviour {

	public bool changeSpeed;
	public bool useSmoothing;
	public float speed = 0;
	public float accel = 0;

	private Vector2 velRef;
	private Point lastMousePos;
	private Vector2 v2ToSet;

	private void FixedUpdate() {
		if (!changeSpeed)
			return;
		Vector2 direction = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized;

		//Point cursorSpeed = new Point(Mathf.RoundToInt(direction.x * speed), Mathf.RoundToInt(direction.y * speed));

		////Point difference = MouseUtils.GetSystemMousePos() - lastMousePos;
		////Point newPos = MouseUtils.GetSystemMousePos() + new Point(Mathf.RoundToInt(difference.x * speed), Mathf.RoundToInt(difference.y * speed));

		Point currentMousePosition = MouseUtils.GetSystemMousePos();

		//Vector2 targetPos = new Vector2(currentMousePosition.x + cursorSpeed.x, currentMousePosition.y + cursorSpeed.y);

		//v2ToSet = Vector2.SmoothDamp(v2ToSet, targetPos, ref velRef, accel);

		//StartCoroutine(MouseUtils.SetRelativeMousePosUnconstrained(new Point(Mathf.RoundToInt(v2ToSet.x), Mathf.RoundToInt(v2ToSet.y))));

		Point newPoint = currentMousePosition + new Point(Mathf.RoundToInt(direction.x * speed), Mathf.RoundToInt(direction.y * speed));

		MouseUtils.SetSystemMousePos(newPoint);
	}
}
