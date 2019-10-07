using UnityEngine;
using UnityEngine.Events;

public class ObjectInteraction : MonoBehaviour {

	private static Camera mainCam;
	private static float interactDist = 5;

	public UnityEvent OnInteract;

	/// <summary>
	/// Call the interact from the centre of the screen
	/// </summary>
	public static void Interact() {
		Interact(new Vector2(.5f, .5f));
	}

	/// <summary>
	/// Call the interact from the position of the mouse on the screen
	/// </summary>
	public static void InteractMouse() {
		if (mainCam == null)
			mainCam = Camera.main;
		Interact(mainCam.ScreenToViewportPoint(Input.mousePosition));
	}

	/// <summary>
	/// Call the interact from wherever tf u want in normalized viewport coords
	/// </summary>
	public static void Interact(Vector2 normalizedPos) {
		if (mainCam == null)
			mainCam = Camera.main;

		Ray ray = mainCam.ViewportPointToRay(normalizedPos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, interactDist))
			hit.transform.GetComponent<ObjectInteraction>()?.OnInteract.Invoke();
	}

}