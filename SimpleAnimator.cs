using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleAnimator : MonoBehaviour {

	[SerializeField]
	private bool playOnStart = true;
	[SerializeField]
	private PlayType playType;
	[SerializeField]
	private Sprite[] frames;
	[Space]
	[SerializeField]
	private int frameRate;
	[SerializeField]
	private int frameRateVariation;
	[SerializeField]
	private int initialOffset;

	private SpriteRenderer spriteRenderer;
	private int index;

	private void Start() {
		if (playOnStart)
			StartCoroutine(Run());
	}

	private IEnumerator Run() {

		spriteRenderer = GetComponent<SpriteRenderer>();
		float actualFrameRate = frameRate + Random.Range(-frameRateVariation, frameRateVariation);

		if (playType == PlayType.Loop) {
			while (true) {

				spriteRenderer.sprite = frames[(index + initialOffset) % frames.Length];

				yield return new WaitForSeconds(1f / actualFrameRate);
				index = (index + 1) % frames.Length;
			}

		} else if (playType == PlayType.Once) {

			for (int i = initialOffset; i < frames.Length; i++) {
				spriteRenderer.sprite = frames[i];
				yield return new WaitForSeconds(1f / actualFrameRate);
			}
		}
	}

	public void Play() {
		StartCoroutine(Run());
	}

	private enum PlayType {
		Loop,
		Once
	}

}