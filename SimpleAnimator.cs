using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class SimpleAnimator : MonoBehaviour {

	[SerializeField]
	private bool playOnStart = true;
	[SerializeField]
	private PlayType playType;
	[SerializeField]
	private int frameRate;
	[SerializeField]
	private int frameRateVariation;
	[SerializeField]
	private int initialOffset;
	[Space]
	[SerializeField]
	private Frame[] frames;

	private int index;

	private void Start() {
		if (playOnStart)
			StartCoroutine(Run());
	}

	private IEnumerator Run() {

		float actualFrameRate = frameRate + Random.Range(-frameRateVariation, frameRateVariation);

		if (playType == PlayType.Loop) {
			while (true) {
				Frame current = frames[(index + initialOffset) % frames.Length];
				SetImage(current.frame);

				yield return new WaitForSeconds((float)current.duration / actualFrameRate);
				index = (index + 1) % frames.Length;
			}

		} else if (playType == PlayType.Once) {

			for (int i = initialOffset; i < frames.Length; i++) {
				SetImage(frames[i].frame);
				yield return new WaitForSeconds((float)frames[i].duration / actualFrameRate);
			}
		}
	}

	protected abstract void SetImage(Sprite sprite);

	public void Play() {
		StartCoroutine(Run());
	}

	private enum PlayType {
		Loop,
		Once
	}

	[System.Serializable]
	private class Frame {
		public Sprite frame;
		public int duration = 1;
	}
}