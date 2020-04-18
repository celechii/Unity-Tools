using System.Collections;
using UnityEngine;

public class SimpleStateAnimator : MonoBehaviour {

	public bool startOnEnable;
	public Animation[] anims;

	private SpriteRenderer spriteRenderer;
	private int animIndex;
	private Coroutine currentAnimPlaying;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnEnable() {
		if (startOnEnable)
			PlayAnim(0);
	}

	public void PlayAnim(string name) {
		for (int i = 0; i < anims.Length; i++) {
			if (anims[i].name == name) {
				PlayAnim(i);
				return;
			}
		}
		throw new System.Exception($"hmmmm, don't see {name} anywhere in here??");
	}

	public void PlayAnim(int index) {
		animIndex = index;
		if (currentAnimPlaying != null)
			StopCoroutine(currentAnimPlaying);
		currentAnimPlaying = StartCoroutine(Play(anims[index]));

	}

	public void PlayNext() {
		PlayAnim((animIndex + 1) % anims.Length);
	}

	public void PlayPrev() {
		if (animIndex == 0)
			animIndex = anims.Length - 1;
		else
			animIndex--;
		PlayAnim(animIndex);
	}

	private IEnumerator Play(Animation anim) {
		int index = 0;
		while (true) {
			while (WorldControl.isPaused)
				yield return null;

			spriteRenderer.sprite = anim.frames[index];
			if (index == anim.frames.Length - 1 && !anim.lööp)
				break;
			yield return new WaitForSeconds(1f / anim.framerate);
			index = (index + 1) % anim.frames.Length;
		}

		currentAnimPlaying = null;
	}

	[System.Serializable]
	public class Animation {
		public string name;
		public int framerate = 25;
		public Sprite[] frames;
		public bool lööp = true;
	}
}