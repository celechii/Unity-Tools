using UnityEngine;

public class NameElementsAttribute : PropertyAttribute {

	public readonly bool drawIndex;

	public NameElementsAttribute(bool drawIndex = false) {
		this.drawIndex = drawIndex;
	}
}