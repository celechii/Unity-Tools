using UnityEngine;

public class GameWindow : MonoBehaviour {

	private static GameWindow control;

	public static Vector2Int Resolution => control.resolution;

	public Vector2Int resolution;
	[Range(0, 1)]
	public float targetSizePercent = .9f;

	private void Awake() {
		control = this;

		SetResolution();
	}

	private void SetResolution() {

		float aspect = (float)resolution.x / resolution.y;
		int targetY = (int)(Screen.currentResolution.height * targetSizePercent);
		int targetX = (int)(Screen.currentResolution.width * targetSizePercent);
		Vector2Int size;

		if (targetY * aspect > targetX) {
			size = new Vector2Int(targetX, (int)(targetX / aspect));
		} else {
			size = new Vector2Int((int)(targetY * aspect), targetY);
		}

		// Debug.LogAssertion($"screen size: {new Vector2(Screen.currentResolution.width, Screen.currentResolution.height)}\nrendering size: {new Vector2(Display.main.renderingWidth, Display.main.renderingHeight)}\nsystem size: {new Vector2(Display.main.systemWidth, Display.main.systemHeight)}\ngame size: {size}");

		Screen.SetResolution(size.x, size.y, FullScreenMode.Windowed);

		PlayerPrefs.SetInt("Screenmanager Resolution Width", size.x);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", size.y);
		PlayerPrefs.SetInt("Screenmanager Fullscreen Mode", 0);
		PlayerPrefs.Save();
	}
}