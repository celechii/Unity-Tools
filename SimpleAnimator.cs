using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleAnimator : MonoBehaviour {

	[SerializeField]
	private Sprite[] frames;
	[SerializeField]
	private int frameRate;
	[SerializeField]
	private int variation;
	[SerializeField]
	private int initialOffset;

	private SpriteRenderer spriteRenderer;
	private int index;

	private IEnumerator Start() {

		spriteRenderer = GetComponent<SpriteRenderer>();
		float actualFrameRate = frameRate + Random.Range(-variation, variation);

		while (true) {

			spriteRenderer.sprite = frames[(index + initialOffset) % frames.Length];

			yield return new WaitForSeconds(1f / actualFrameRate);
			index = (index + 1) % frames.Length;
		}
	}

}
