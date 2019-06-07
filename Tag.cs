using UnityEngine;

public class Tag : MonoBehaviour {

	[MultiEnum]
	public Tags tags;

	public bool Has(Tags tag) => tags.HasFlag(tag);

	public void AddTag(Tags tag) {
		tags |= tag;
	}

	public void RemoveTag(Tags tag) {
		tags &= ~tag;
	}

	public void Toggle(Tags tag) {
		tags ^= tag;
	}

}

[System.Flags]
public enum Tags {
	Nothing = 1,
	WillMove = 2,
	IsEnvironment = 4
}