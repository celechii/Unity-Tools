using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Extensions {

	public static string ToHexRGBA(this Color c) {
		string r = ((int)Mathf.Lerp(0, 255, c.r)).ToString("X");
		string g = ((int)Mathf.Lerp(0, 255, c.g)).ToString("X");
		string b = ((int)Mathf.Lerp(0, 255, c.b)).ToString("X");
		string a = ((int)Mathf.Lerp(0, 255, c.a)).ToString("X");
		return (r.Length == 2 ? r : "0" + r) + (g.Length == 2 ? g : "0" + g) + (b.Length == 2 ? b : "0" + b) + (a.Length == 2 ? a : "0" + a);
	}

	public static string Colour(this string s, Color c) {
		string cHex = c.ToHexRGBA();
		return "<color=#" + cHex + ">" + s + "</color>";
	}

	public static Gradient AsGradient(this Color c) {
		Gradient grad = new Gradient();
		grad.colorKeys = new GradientColorKey[] {
			new GradientColorKey(c, 0),
				new GradientColorKey(c, 1),
		};
		grad.alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(c.a, 0),
				new GradientAlphaKey(c.a, 1),
		};
		return grad;
	}

	public static T[] ToArray<T>(this HashSet<T> set) {
		T[] array = new T[set.Count];
		int count = 0;
		foreach (T t in set) {
			array[count] = t;
			count++;
		}
		return array;
	}

	/// <summary>
	/// gets u either -1, 0, 1
	/// </summary>
	public static float Direction(this float value) {
		if (value == 0)
			return 0;
		return value / Math.Abs(value);
	}

	/// <summary>
	/// ok so do smth like
	/// yield return StartCoroutine(Extensions.DoThingOverTime(duration, elapsed => {
	/// 	print("im gay");
	/// }));
	/// </summary>
	public static IEnumerator DoThingOverTime(float duration, Action<float> action) {
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
			action.Invoke(elapsed);
			yield return null;
		}
	}

	public static string[] Trim(this string[] s) {
		for (int i = 0; i < s.Length; i++)
			s[i] = s[i].Trim();
		return s;
	}

	public static bool IsLetter(this char c) => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');

	public static bool IsInsideInvisibleBox2D(this Vector2 pos, Vector2 min, Vector2 max) {
		return pos.x >= min.x && pos.x <= max.x && pos.y >= min.y && pos.y <= max.y;
	}

	public static bool IsInsideInvisibleBox2D(this Vector3 pos, Vector2 min, Vector2 max) {
		return pos.x >= min.x && pos.x <= max.x && pos.y >= min.y && pos.y <= max.y;
	}

	public static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 t) {
		return new Vector2(Mathf.InverseLerp(a.x, b.x, t.x), Mathf.InverseLerp(a.y, b.y, t.y));
	}

	public static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t) {
		return new Vector2(Mathf.Lerp(a.x, b.x, t.x), Mathf.Lerp(a.y, b.y, t.y));
	}

	public static Vector3 ToV3(this Vector2 p, float z) {
		return new Vector3(p.x, p.y, z);
	}

	public static Vector2 Clamp(this Vector2 v, Vector2 a, Vector2 b) {
		v.x = Mathf.Clamp(v.x, Mathf.Min(a.x, b.x), Mathf.Max(a.x, b.x));
		v.y = Mathf.Clamp(v.y, Mathf.Min(a.y, b.y), Mathf.Max(a.y, b.y));
		return v;
	}

	public static Transform GetGrandChild(this Transform trans) {
		if (trans.childCount == 0)
			return trans;
		return trans.GetChild(0).GetGrandChild();
	}

	public static bool IsUpper(this char c) => c >= 'A' && c <= 'Z';

	public static bool IsLower(this char c) => c >= 'a' && c <= 'z';

	public static T DeepCopy<T>(T other) {
		using(MemoryStream ms = new MemoryStream()) {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, other);
			ms.Position = 0;
			return (T)formatter.Deserialize(ms);
		}
	}

	public static bool IsPointInside(this Bounds bounds, Vector2 point) =>
		point.x <= bounds.max.x && point.x >= bounds.min.x && point.y <= bounds.max.y && point.y >= bounds.min.y;

	public static int LengthOfEnum<TEnum>(this TEnum t)where TEnum : struct => System.Enum.GetNames(typeof(TEnum)).Length;

	public static string MakeEnumReadable<TEnum>(this TEnum t)where TEnum : struct, IConvertible {
		string entry = t.ToString();
		for (int i = 1; i < entry.Length; i++) {
			if (entry[i].IsUpper()) {
				entry = entry.Insert(i, " ");
				i++;
			}
		}
		return entry;
	}

	// based on how unity did it:
	// https://stackoverflow.com/questions/61372498/how-does-mathf-smoothdamp-work-what-is-it-algorithm

	public static Color ColourSmoothDamp(Color current, Color target, ref Vector4 currentVelocity, float smoothTime) =>
		ColourSmoothDamp(current, target, ref currentVelocity, smoothTime, Mathf.Infinity, Time.deltaTime);
	public static Color ColourSmoothDamp(Color current, Color target, ref Vector4 currentVelocity, float smoothTime, float maxSpeed) =>
		ColourSmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, Time.deltaTime);

	public static Color ColourSmoothDamp(Color current, Color target, ref Vector4 currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
		// Based on Game Programming Gems 4 Chapter 1.10
		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float omega = 2f / smoothTime;

		Vector4 currentColour = new Vector4(current.r, current.g, current.b, current.a);
		Vector4 targetColour = new Vector4(target.r, target.g, target.b, target.a);

		float x = omega * deltaTime;
		float exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
		Vector4 change = currentColour - targetColour;
		Vector4 originalTo = targetColour;

		// Clamp maximum speed
		Vector4 maxChange = maxSpeed * smoothTime * Vector4.one;
		change.x = Mathf.Clamp(change.x, -maxChange.x, maxChange.x);
		change.y = Mathf.Clamp(change.y, -maxChange.y, maxChange.y);
		change.z = Mathf.Clamp(change.z, -maxChange.z, maxChange.z);
		change.w = Mathf.Clamp(change.w, -maxChange.w, maxChange.w);

		targetColour = currentColour - change;

		Vector4 temp = (currentVelocity + (omega * change)) * deltaTime;
		currentVelocity = (currentVelocity - omega * temp) * exp;
		Vector4 output = targetColour + (change + temp) * exp;

		// Prevent overshooting
		if (originalTo.x - currentColour.x > 0.0f == output.x > originalTo.x) {
			output.x = originalTo.x;
			currentVelocity.x = (output.x - originalTo.x) / deltaTime;
		}
		if (originalTo.y - currentColour.y > 0.0f == output.y > originalTo.y) {
			output.y = originalTo.y;
			currentVelocity.y = (output.y - originalTo.y) / deltaTime;
		}
		if (originalTo.z - currentColour.z > 0.0f == output.z > originalTo.z) {
			output.z = originalTo.z;
			currentVelocity.z = (output.z - originalTo.z) / deltaTime;
		}
		if (originalTo.w - currentColour.w > 0.0f == output.w > originalTo.w) {
			output.w = originalTo.w;
			currentVelocity.w = (output.w - originalTo.w) / deltaTime;
		}

		return new Color(output.x, output.y, output.z, output.w);
	}

	// none of these work the way u think they work

	// // from freya holmér <3
	// // https://twitter.com/FreyaHolmer/status/1068280371907883008
	// public static float Elerp(float a, float b, float t) {
	// 	float a1 = Mathf.Log10(a);
	// 	float b1 = Mathf.Log10(b);
	// 	float exp = Mathf.LerpUnclamped(a1, b1, t) * 3.321922809489f;
	// 	return Mathf.Pow(2, exp);
	// }

	// // from ashley @khyperia <3
	// // https://twitter.com/khyperia
	// // https://twitter.com/FreyaHolmer/status/1068293398073929728
	// public static float Eerp(float a, float b, float t) {
	// 	return a * Mathf.Pow(b / a, t);
	// }

	public static float ExpLerp01(float t, float exp = 10f) => ExpLerp(0, 1, Mathf.Clamp01(t), exp);

	public static float ExpLerp(float a, float b, float t, float exp = 10f) {
		return a + Mathf.Pow(Mathf.InverseLerp(a, b, t), exp) * (b - a);
	}
}