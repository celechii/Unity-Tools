/*
MIT License

Copyright (c) 2021 Noé Charron

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public static class NAudio {

	private static List<AudioSourceLink> sources = new List<AudioSourceLink>();

	private static Transform sourcePool;
	private const float DEFAULT_MIN_DIST = 1f;
	private const float DEFAULT_MAX_DIST = 500f;

	public static float RemapVolume(float targetVolume) => (1 - Mathf.Pow(targetVolume, 1f / 0.2f)) + 2f * targetVolume - 1;

	// multiple sounds, no pitch
	public static void PlaySound(AudioClip[] sounds, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], volume, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], source, worldSpace, volume, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, Vector3 worldPosition, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], worldPosition, volume, mixerGroup);

	// multiple sounds, pitch range
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch, volume, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch, source, worldSpace, volume, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, float minPitch, float maxPitch, Vector3 worldPosition, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], minPitch, maxPitch, worldPosition, volume, mixerGroup);

	// multiple sounds, single pictch
	public static void PlaySound(AudioClip[] sounds, float pitch, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, float pitch, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch, source, worldSpace, volume, mixerGroup);
	public static void PlaySound(AudioClip[] sounds, float pitch, Vector3 worldPosition, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sounds[Random.Range(0, sounds.Length)], pitch, worldPosition, volume, mixerGroup);

	// single sound, no pitch
	public static void PlaySound(AudioClip sound, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, 1, mixerGroup);
	public static void PlaySound(AudioClip sound, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, 1, source, worldSpace, volume, mixerGroup);
	public static void PlaySound(AudioClip sound, Vector3 worldPosition, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, 1, worldPosition, volume, mixerGroup);

	// single sound, pitch range
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, minPitch, maxPitch, sourcePool, true, volume, mixerGroup);
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, Random.Range(minPitch, maxPitch), source, worldSpace, volume, mixerGroup);
	public static void PlaySound(AudioClip sound, float minPitch, float maxPitch, Vector3 worldPosition, float volume = 1f, AudioMixerGroup mixerGroup = null) => PlaySound(sound, Random.Range(minPitch, maxPitch), worldPosition, volume, mixerGroup);

	// single sound, single pitch
	public static void PlaySound(AudioClip sound, float pitch, float volume = 1f, AudioMixerGroup mixerGroup = null, float minDist = DEFAULT_MIN_DIST, float maxDist = DEFAULT_MAX_DIST) {
		AudioSource2DLink link = FindLink<AudioSource2DLink>();
		if (link == null) {
			link = new AudioSource2DLink(mixerGroup);
			sources.Add(link);
		} else
			link.UpdateLink(mixerGroup);
		link.PlaySound(sound, volume, pitch, minDist, maxDist);
	}

	public static void PlaySound(AudioClip sound, float pitch, Transform source, bool worldSpace, float volume = 1f, AudioMixerGroup mixerGroup = null, float minDist = DEFAULT_MIN_DIST, float maxDist = DEFAULT_MAX_DIST) {
		AudioSourceTransformLink link = FindLink<AudioSourceTransformLink>();
		if (link == null) {
			link = new AudioSourceTransformLink(source, worldSpace, mixerGroup);
			sources.Add(link);
		} else
			link.UpdateLink(source, worldSpace, mixerGroup, minDist, maxDist);
		link.PlaySound(sound, volume, pitch, minDist, maxDist);
	}
	public static void PlaySound(AudioClip sound, float pitch, Vector3 position, float volume = 1f, AudioMixerGroup mixerGroup = null, float minDist = DEFAULT_MIN_DIST, float maxDist = DEFAULT_MAX_DIST) {
		AudioSourcePositionLink link = FindLink<AudioSourcePositionLink>();
		if (link == null) {
			link = new AudioSourcePositionLink(position, mixerGroup);
			sources.Add(link);
		} else
			link.UpdateLink(position, mixerGroup);
		link.PlaySound(sound, volume, pitch, minDist, maxDist);
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

		public AudioSource audioSource;
		public int id;
		public bool Active => audioSource.isPlaying;

		protected abstract void GoToPosition();

		public AudioSourceLink() {
			id = sourceCount++;
			GameObject go = new GameObject($"Sound {id}", typeof(AudioSource));
			audioSource = go.GetComponent<AudioSource>();
			audioSource.spatialBlend = 1f;

			if (NAudio.sourcePool == null)
				NAudio.sourcePool = new GameObject("Audio Pool", typeof(NAudioUpdater)).transform;
			go.transform.SetParent(NAudio.sourcePool);
		}

		public void UpdatePosition() {
			if (Active)
				GoToPosition();
		}

		public void PlaySound(AudioClip clip, float volume, float pitch, float minDist, float maxDist) {
			audioSource.volume = NAudio.RemapVolume(volume);
			audioSource.minDistance = minDist;
			audioSource.maxDistance = maxDist;
			audioSource.clip = clip;
			audioSource.pitch = pitch;
			audioSource.Play();
		}
	}

	private class AudioSource2DLink : AudioSourceLink {

		public AudioSource2DLink(AudioMixerGroup mixerGroup) : base() {
			audioSource.spatialBlend = 0;
			UpdateLink(mixerGroup);
		}

		protected override void GoToPosition() {}

		public void UpdateLink(AudioMixerGroup mixerGroup) {
			audioSource.outputAudioMixerGroup = mixerGroup;
		}
	}

	private class AudioSourceTransformLink : AudioSourceLink {
		private Transform source;
		private bool worldSpace;

		public AudioSourceTransformLink(Transform transformTracker, bool worldSpace, AudioMixerGroup mixerGroup = null, float minDist = DEFAULT_MIN_DIST, float maxDist = DEFAULT_MAX_DIST) : base() {
			UpdateLink(transformTracker, worldSpace, mixerGroup, minDist, maxDist);
		}

		protected override void GoToPosition() {
			base.audioSource.transform.localPosition = worldSpace ? source.position : source.localPosition;
		}

		public void UpdateLink(Transform newSource, bool worldSpace, AudioMixerGroup mixerGroup, float minDist, float maxDist) {
			source = newSource;
			audioSource.outputAudioMixerGroup = mixerGroup;
			audioSource.minDistance = minDist;
			audioSource.maxDistance = maxDist;
			this.worldSpace = worldSpace;
			UpdatePosition();
		}
	}

	private class AudioSourcePositionLink : AudioSourceLink {
		public AudioSourcePositionLink(Vector3 worldPosition, AudioMixerGroup mixerGroup) : base() =>
			UpdateLink(worldPosition, mixerGroup);

		protected override void GoToPosition() {}

		public void UpdateLink(Vector3 newPosition, AudioMixerGroup mixerGroup) {
			audioSource.transform.localPosition = newPosition;
			audioSource.outputAudioMixerGroup = mixerGroup;
		}
	}
}

[System.Serializable]
public abstract class NAudioSound {
	[MinMaxSlider(0f, 3f)]
	public Vector2 pitchRange = Vector2.one;
	[Range(0, 1)]
	public float volume = 1f;
	public RangeFloat distRange = new RangeFloat(1, 500);
	[Space]
	public AudioMixerGroup mixerGroup;

	public abstract void Play(Transform source, bool worldSpace);
	public abstract void Play(Vector3 worldPos);
	public abstract void Play();

	public float Pitch => Random.Range(pitchRange.x, pitchRange.y);
}

[System.Serializable]
public class NSoundClip : NAudioSound {
	public AudioClip sound;

	public override void Play(Transform source, bool worldSpace) => NAudio.PlaySound(sound, Pitch, source, worldSpace, volume, mixerGroup, distRange.min, distRange.max);
	public override void Play(Vector3 worldPos) => NAudio.PlaySound(sound, Pitch, worldPos, volume, mixerGroup, distRange.min, distRange.max);
	public override void Play() => NAudio.PlaySound(sound, Pitch, volume, mixerGroup, distRange.min, distRange.max);
}

[System.Serializable]
public class NSoundList : NAudioSound {
	public AudioClip[] sounds;

	private AudioClip Sound => sounds == null || sounds.Length == 0 ? null : sounds[Random.Range(0, sounds.Length)];

	public(AudioClip, float)GetSoundAndPitch() => (Sound, Pitch);

	public override void Play(Transform source, bool worldSpace) => NAudio.PlaySound(Sound, Pitch, source, worldSpace, volume, mixerGroup, distRange.min, distRange.max);
	public override void Play(Vector3 worldPos) => NAudio.PlaySound(Sound, Pitch, worldPos, volume, mixerGroup, distRange.min, distRange.max);
	public override void Play() => NAudio.PlaySound(Sound, Pitch, volume, mixerGroup, distRange.min, distRange.max);
}