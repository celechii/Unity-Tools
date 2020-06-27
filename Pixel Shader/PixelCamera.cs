// based on code by Dmitry Timofeev
// https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/pixelation-65554

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class PixelCamera : MonoBehaviour {

	public Shader shader;
	[Range(64.0f, 512.0f)]
	public float BlockCount = 240;

	private Material Material {
		get {
			if (material == null) {
				material = new Material(shader);
				material.hideFlags = HideFlags.HideAndDontSave;
			}
			return material;
		}
	}
	private Material material;

	private Camera mainCam;

	private void Awake() {
		mainCam = GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (mainCam == null)
			mainCam = GetComponent<Camera>();

		Vector2 count = new Vector2(BlockCount, BlockCount / mainCam.aspect);
		Vector2 size = new Vector2(1f / count.x, 1f / count.y);

		Material.SetVector("BlockCount", count);
		Material.SetVector("BlockSize", size);
		Graphics.Blit(source, destination, Material);
	}

	private void OnDisable() {
		if (material)
			DestroyImmediate(material);
	}

}