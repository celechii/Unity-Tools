using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimator : SimpleAnimator {

	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void SetImage(Sprite sprite) {
		spriteRenderer.sprite = sprite;
	}
}