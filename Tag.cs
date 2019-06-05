using UnityEngine;

public class Tag : MonoBehaviour {

	[MultiEnum]
	public Tags tags;

}

[System.Flags]
public enum Tags {
	None = 0,
	One = 1,
	Two = 2,
	Four = 4,
	Eight = 8
}