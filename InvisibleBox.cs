using UnityEngine;

[ExecuteInEditMode]
public class InvisibleBox : MonoBehaviour {

	public Vector2 size = Vector2.one * 4;
	public Vector2 offset;
	public bool useCentreOrigin = true;
	[SerializeField]
	private TransformPosition useTransformPosition = TransformPosition.Local;
	[SerializeField]
	private bool showBox = true;

	public bool IsInBox(Vector2 position) {

		// if (useTransformPosition == TransformPosition.World)
		// 	position -= (Vector2)transform.position;

		if (useTransformPosition == TransformPosition.Local)
			position -= (Vector2)transform.localPosition;

		position -= offset;

		if (useCentreOrigin)
			position += size / 2f;

		return position.IsInsideInvisibleBox2D(Vector2.zero, size);
	}

	public bool IsInBox(Transform trans, bool useLocal = true) => IsInBox((useLocal?trans.localPosition : Vector3.zero));

	private void OnDrawGizmos() {
		if (showBox) {
			Gizmos.color = Color.green;

			Vector2 worldOffset = Vector2.zero;
			if (useTransformPosition == TransformPosition.Local)
				worldOffset = transform.localPosition;

			if (useCentreOrigin)
				Gizmos.DrawWireCube(offset + worldOffset, size);
			else
				Gizmos.DrawWireCube(offset + worldOffset + size / 2f, size);

		}
	}

	private enum TransformPosition {
		World,
		Local
	}
}