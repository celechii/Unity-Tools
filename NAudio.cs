using System.Collections.Generic;
using UnityEngine;

public static class NAudio {

	private static List<AudioSourceLink> sources = new List<AudioSourceLink>();

	private static Transform sourcePool;
	private static Transform audioListener;

	// multiple sounds, no pitch
	public static void PlaySound(AudioClip[] sounds) => PlaySound(sounds[Random.Range(0, sounds.Length)]);
	public static void PlaySound(AudioClip[] sounds, Transform source, bool worldSpace) => PlaySound(sounds[Random.Range(0, sounds.Length)], source, worldSpace);
	public static void PlaySound(AudioClip[] sounds, Vector3 worldPosition) => PlaySound(sounds[Random.Range(0, sounds.Length)], worldPosition);

	// multiple sounds, pitch range
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch);
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch, Transform source, bool worldSpace) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch, source, worldSpace);
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch, Vector3 worldPosition) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch, worldPosition);

	// multiple sounds, single pictch
	public static void PlaySound(AudioClip[] sounds, float pitch) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch);
	public static void PlaySound(AudioClip[] sounds, float pitch, Transform source, bool worldSpace) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch, source, worldSpace);
	public static void PlaySound(AudioClip[] sounds, float pitch, Vector3 worldPosition) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch, worldPosition);

	// single sound, no pitch
	public static void PlaySound(AudioClip sound) => PlaySound(sound, 1);
	public static void PlaySound(AudioClip sound, Transform source, bool worldSpace) => PlaySound(sound, 1, source, worldSpace);
	public static void PlaySound(AudioClip sound, Vector3 worldPosition) => PlaySound(sound, 1, worldPosition);

	// single sound, pitch range
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch) => PlaySound(sound, minPitch, maxPitch, sourcePool, true);
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch, Transform source, bool worldSpace) => PlaySound(sound, Random.Range(minPitch, maxPitch), source, worldSpace);
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch, Vector3 worldPosition) => PlaySound(sound, Random.Range(minPitch, maxPitch), worldPosition);

	// single sound, single pitch
	public static void PlaySound(AudioClip sound, float pitch) => PlaySound(sound, pitch, audioListener, true);

	public static void PlaySound(AudioClip sound, float pitch, Transform source, bool worldSpace) {
		AudioSourceTransformLink link = FindLink<AudioSourceTransformLink>();
		if (link == null) {
			link = new AudioSourceTransformLink(source, worldSpace);
			sources.Add(link);
		}
		link.PlaySound(sound, pitch);
	}
	public static void PlaySound(AudioClip sound, float pitch, Vector3 position) {
		AudioSourcePositionLink link = FindLink<AudioSourcePositionLink>();
		if (link == null) {
			link = new AudioSourcePositionLink(position);
			sources.Add(link);
		}
		link.PlaySound(sound, pitch);
	}

	private static T FindLink<T>()where T : AudioSourceLink {
		foreach (AudioSourceLink source in sources)
			if (!source.Active && source.GetType() == typeof(T))
				return (T)source;
		return null;
	}

	private class NAudioUpdater : MonoBehaviour {

		private void Start() {
			LateUpdate();
		}

		public void LateUpdate() {
			foreach (AudioSourceLink sources in NAudio.sources)
				if (sources.Active)
					sources.UpdatePosition();
		}
	}

	private abstract class AudioSourceLink {
		private static int sourceCount;

		public AudioSource source;
		public int id;
		public bool Active => source.isPlaying;

		protected abstract void GoToPosition();

		public AudioSourceLink() {
			id = sourceCount++;
			GameObject go = new GameObject($"Sound {id}", typeof(AudioSource));
			source = go.GetComponent<AudioSource>();
			source.spatialBlend = 1f;

			if (NAudio.sourcePool == null)
				NAudio.sourcePool = new GameObject("Audio Pool", typeof(NAudioUpdater)).transform;
			go.transform.SetParent(NAudio.sourcePool);
		}

		public void UpdatePosition() {
			if (Active)
				GoToPosition();
		}

		public void PlaySound(AudioClip clip, float pitch) {
			source.clip = clip;
			source.pitch = pitch;
			source.Play();
		}
	}

	private class AudioSourceTransformLink : AudioSourceLink {

		private Transform track;
		private bool worldSpace;

		public AudioSourceTransformLink(Transform transformTracker, bool worldSpace) : base() {
			track = transformTracker;
			this.worldSpace = worldSpace;
		}

		protected override void GoToPosition() {
			source.transform.localPosition = worldSpace ? track.position : track.localPosition;
		}
	}

	private class AudioSourcePositionLink : AudioSourceLink {

		public AudioSourcePositionLink(Vector3 worldPosition) : base() =>
			source.transform.localPosition = worldPosition;

		protected override void GoToPosition() {}

	}
}