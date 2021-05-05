using UnityEngine;

[System.Serializable]
public class LissajousCurve {
	public Vector2 size = Vector2.one;
	public int ratio = 1;

	private const float tau = Mathf.PI * 2f;

	public Vector2 Evaluate(float percent) {
		return size * new Vector2(
			Mathf.Sin(tau * percent),
			Mathf.Cos(tau * percent * ratio)
		);
	}

	public void DrawCurve(Vector3 position) {
		Gizmos.color = Color.white;
		int segments = 100;
		float increment = 1f / segments;
		for (float i = 0; i < 1; i += increment)
			Gizmos.DrawLine(position + (Vector3)Evaluate(i), position + (Vector3)Evaluate(i + increment));
		Gizmos.color = new Color(1, 1, 1, .35f);
		Gizmos.DrawWireCube(position, size * 2f);
		Gizmos.DrawLine(position + Vector3.right * size.x, position + Vector3.left * size.x);
		Gizmos.DrawLine(position + Vector3.up * size.y, position + Vector3.down * size.y);
	}
}