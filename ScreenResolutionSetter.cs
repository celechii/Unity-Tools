using UnityEngine;

public class ScreenResolutionSetter : MonoBehaviour {

	public bool setPrefOnLoad;
	public int resolutionsFromMax;
	public bool fullscreen;
	public Vector2Int targetWindowAspect;
	public Vector2Int targetResolution;

	private void Awake() {
		if (setPrefOnLoad)
			SetResolution();
	}

	public void SetResolution() {
		if (resolutionsFromMax == Screen.resolutions.Length - 1)
			return;

		Resolution targetRes = Screen.resolutions[Screen.resolutions.Length - (fullscreen?0 : resolutionsFromMax) - 1];
		Vector2Int size = new Vector2Int(targetRes.width, targetRes.height);
		if (!fullscreen) {
			float aspect;
			if (targetResolution == Vector2Int.zero && targetWindowAspect != Vector2Int.zero)
				aspect = (float)targetWindowAspect.y / targetWindowAspect.x;
			else
				aspect = (float)targetResolution.y / targetResolution.x;
			size.x = Mathf.CeilToInt((float)size.y / aspect);

			if (size.y > Screen.width) {
				resolutionsFromMax++;
				SetResolution();
				return;
			}
		}
		Screen.SetResolution(size.x, size.y, fullscreen?FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

		PlayerPrefs.SetInt("Screenmanager Resolution Width", size.x);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", size.y);
		PlayerPrefs.SetInt("Screenmanager Fullscreen Mode", fullscreen?1 : 0);
		PlayerPrefs.Save();
	}
}