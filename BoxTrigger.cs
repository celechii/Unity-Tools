using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxTrigger : MonoBehaviour {

	public new bool enabled = true;
	public bool visualize = true;
	[Tag]
	public string[] tags = new string[] { "Player" };
	[Space]
	public Events[] events = new Events[1];

	private BoxCollider2D boxCollider;

	private void Awake() {
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		CheckTrigger(other, TInteraction.Enter);
	}

	private void OnTriggerExit2D(Collider2D other) {
		CheckTrigger(other, TInteraction.Exit);
	}

	private void CheckTrigger(Collider2D other, TInteraction interaction) {
		if (!enabled)
			return;

		foreach (string s in tags) {
			if (other.CompareTag(s)) {
				for (int i = 0; i < events.Length; i++) {
					if (events[i].interactions.Has(interaction)) {
						if (events[i].directions.Has(FindDirection(other.transform.localPosition)))
							events[i].Call.Invoke();
					}
				}
				return;
			}
		}
	}

	private TDirection FindDirection(Vector2 pos) {
		TDirection dir = TDirection.Top;
		float smallestDist = Mathf.Abs(pos.y - boxCollider.bounds.max.y);

		float temp = Mathf.Abs(boxCollider.bounds.min.y - pos.y);
		if (temp < smallestDist) {
			dir = TDirection.Bottom;
			smallestDist = temp;
		}
		temp = Mathf.Abs(boxCollider.bounds.min.x - pos.x);
		if (temp < smallestDist) {
			dir = TDirection.Left;
			smallestDist = temp;
		}
		temp = Mathf.Abs(boxCollider.bounds.max.x - pos.x);
		if (temp < smallestDist)
			dir = TDirection.Right;

		return dir;
	}

	[Button]
	private void SetTransformFromBoxOffset() {
		transform.Translate(boxCollider.offset);
		boxCollider.offset = Vector2.zero;
	}

	private void OnDrawGizmos() {
		if (visualize) {
			Gizmos.color = new Color(enabled ? 0 : 1, enabled ? 1 : 0, 0, .3f);
			if (boxCollider == null)
				boxCollider = GetComponent<BoxCollider2D>();
			Gizmos.DrawCube(boxCollider.offset + (Vector2)transform.localPosition, boxCollider.size);
		}
	}

	[System.Serializable]
	public class Events {
		public TInteraction interactions = (TInteraction)(-1);
		[MultiEnum]
		public TDirection directions = (TDirection)(-1);
		public UnityEvent Call;
	}

	[System.Flags]
	public enum TInteraction {
		Enter = 1 << 0,
		Exit = 1 << 1
	}

	[System.Flags]
	public enum TDirection {
		Top = 1 << 0,
		Bottom = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3
	}
}