using UnityEngine;

public class DynamicSort : MonoBehaviour {

	[SerializeField]
	private bool staticSort;
	[SerializeField]
	private bool useLocalPos = true;
	[SerializeField]
	private int numParents;
	[SerializeField]
	private int offset;
	[SerializeField]
	private string sortingLayerName = "Dynamic";

	private SpriteRenderer spriteRenderer;
	private Transform transUsing;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		transUsing = GetParent(transform, numParents);
		spriteRenderer.sortingLayerName = sortingLayerName;
		if (staticSort) {
			SetOrder();
			enabled = false;
		}
	}

	private void Update() {
		if (!WorldControl.IsPaused)
			SetOrder();
	}

	public void SetOrder() {
		spriteRenderer.sortingOrder = GetOrder(useLocalPos?transUsing.localPosition.y : transUsing.position.y);
	}

	public int GetOrder(float yVal) => Mathf.RoundToInt(yVal * -100) + offset;

	private Transform GetParent(Transform child, int num) {
		if (num == 0 || !child.parent)
			return child;
		return GetParent(child.parent, num - 1);
	}
}