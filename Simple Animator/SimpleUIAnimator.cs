using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SimpleUIAnimator : SimpleAnimator {

	private Image image;

	private void Awake() {
		image = GetComponent<Image>();
	}

	protected override void SetImage(Sprite sprite) {
		image.sprite = sprite;
	}
}