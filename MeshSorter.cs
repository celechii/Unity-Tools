using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshSorter : MonoBehaviour {

	[SerializeField]
	private string layerName = "UI";
	[SerializeField]
	private int sortingOrder = 1;
	private MeshRenderer meshRenderer;

	[ContextMenu("Update Shit")]
	void OnEnable() {
		meshRenderer = GetComponent<MeshRenderer>();
		Set(layerName, sortingOrder);
	}

	public void Set(string layerName, int sortingOrder) {
		this.layerName = layerName;
		this.sortingOrder = sortingOrder;

		meshRenderer.sortingLayerName = layerName;
		meshRenderer.sortingOrder = sortingOrder;
	}
}