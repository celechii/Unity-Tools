using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Utils {

	#region UTILITIES

	#region Number Utils

	public static float pixelsPerUnit = 16;

	public static float PixelsToUnits(float value) => PixelsToUnits(value, pixelsPerUnit);
	public static float PixelsToUnits(float value, float ppu) => value / ppu;

	public static float UnitsToPixels(float value) => UnitsToPixels(value, pixelsPerUnit);
	public static float UnitsToPixels(float value, float ppu) => value * ppu;

	public static float ClampWithMax(float value, float max) => value > max ? max : value;
	public static float ClampWithMin(float value, float min) => value < min ? min : value;

	public static float FindTime(float speed, float distance) => distance / speed;
	public static float FindDist(float speed, float time) => speed * time;
	public static float FindSpeed(float distance, float time) => distance / time;

	public static int IndexOfSmallest(float[] values) {
		if (values == null || values.Length == 0)
			throw new Exception("hey what the fuck, give me some values");
		float smallest = values[0];
		int index = 0;
		for (int i = 1; i < values.Length; i++) {
			if (values[i] < smallest) {
				smallest = values[i];
				index = i;
			}
		}
		return index;
	}

	public static int IndexOfLargest(float[] values) {
		if (values == null || values.Length == 0)
			throw new Exception("hey what the fuck, give me some values");
		float largest = values[0];
		int index = 0;
		for (int i = 1; i < values.Length; i++) {
			if (values[i] > largest) {
				largest = values[i];
				index = i;
			}
		}
		return index;
	}

	public static float ExpLerp01(float t, float exp = 10f) => ExpLerp(0, 1, Mathf.Clamp01(t), exp);

	public static float ExpLerp(float a, float b, float t, float exp = 10f) {
		return a + Mathf.Pow(Mathf.InverseLerp(a, b, t), exp) * (b - a);
	}

	public static float AbsMax(float a, float b) {
		if (Mathf.Abs(a) >= Mathf.Abs(b))
			return a;
		return b;
	}

	public static float AbsMin(float a, float b) {
		if (Mathf.Abs(a) <= Mathf.Abs(b))
			return a;
		return b;
	}

	public static float RoundToNearest(float value, float[] snaps) {
		if (snaps == null)
			throw new Exception("um wtf the snaps array u gave me is null :/");
		if (snaps.Length == 0)
			throw new Exception("lmao fucker u gotta put things in the snaps array to snap to");

		float smallestDist = Mathf.Abs(value - snaps[0]);
		int index = 0;
		for (int i = 1; i < snaps.Length; i++) {
			float dist = Mathf.Abs(value - snaps[i]);
			if (dist < smallestDist) {
				smallestDist = dist;
				index = i;
			}
		}
		return snaps[index];
	}

	#endregion

	#region Vector Utils

	public static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 t) {
		return new Vector2(Mathf.InverseLerp(a.x, b.x, t.x), Mathf.InverseLerp(a.y, b.y, t.y));
	}

	public static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t) {
		return new Vector2(Mathf.Lerp(a.x, b.x, t.x), Mathf.Lerp(a.y, b.y, t.y));
	}

	#endregion

	#region Colour Utils

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

	#endregion

	#region Coroutine Utils

	/// <param name="duration">Duration in seconds.</param>
	/// <param name="realtime">Should use real time or scaled time?</param>
	/// <returns></returns>
	public static IEnumerator PauseableWait(float duration, Func<bool> predicate, bool realtime = false) {
		for (float elapsed = 0; elapsed < duration; elapsed += (realtime?Time.unscaledDeltaTime : Time.deltaTime)) {
			while (predicate.Invoke())
				yield return null;
			yield return null;
		}
	}

	/// <summary>
	/// Call a function every frame for a duration.
	/// </summary>
	/// <param name="duration">How long call the function for.</param>
	/// <param name="action">Function to be invoked and passed the elapsed time.</param>
	/// <returns></returns>
	public static IEnumerator DoOverTime(float duration, Action<float> action, bool callAgainOnComplete = true, Func<bool> pausePredicate = null) {
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
			if (pausePredicate != null && pausePredicate.Invoke())
				yield return null;

			action.Invoke(elapsed);
			yield return null;
		}
		if (callAgainOnComplete)
			action.Invoke(duration);
	}
	#endregion

	#region Generic Utils

	public static T DeepCopy<T>(T other) {
		using(MemoryStream ms = new MemoryStream()) {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, other);
			ms.Position = 0;
			return (T)formatter.Deserialize(ms);
		}
	}

	#endregion

	#endregion

	#region EXTENSION METHODS

	#region Value Extensions

	/// <summary>
	/// Is the character an uppercase letter?
	/// </summary>
	public static bool IsUpper(this char c) => c >= 'A' && c <= 'Z';

	/// <summary>
	/// Is the character a lowercase letter?
	/// </summary>
	public static bool IsLower(this char c) => c >= 'a' && c <= 'z';

	/// <summary>
	/// Is the character a letter?
	/// </summary>
	public static bool IsLetter(this char c) => c.IsUpper() || c.IsLower();

	/// <summary>
	/// Trim every string in an array.
	/// </summary>
	public static string[] Trim(this string[] s) {
		for (int i = 0; i < s.Length; i++)
			s[i] = s[i].Trim();
		return s;
	}

	/// <summary>
	/// Puts richtext colour tags around the string.
	/// </summary>
	public static string Colour(this string s, Color colour) {
		string cHex = colour.ToHexRGBA();
		return "<color=#" + cHex + ">" + s + "</color>";
	}

	public static void ClampWithMax(this ref float value, float max) {
		if (value > max)
			value = max;
	}

	public static void ClampWithMin(this ref float value, float min) {
		if (value < min)
			value = min;
	}

	/// <summary>
	/// Is the int even?
	/// </summary>
	public static bool IsEven(this int i) => i % 2 == 0;

	/// <summary>
	/// Is the int odd?
	/// </summary>
	public static bool IsOdd(this int i) => i % 2 != 0;

	/// <summary>
	/// gets u either -1, 0, 1
	/// </summary>
	public static float Direction(this float value) {
		if (value == 0)
			return 0;
		return value / Math.Abs(value);
	}

	/// <summary>
	/// Converts HashSet into an array.
	/// </summary>
	public static T[] ToArray<T>(this HashSet<T> set) {
		T[] array = new T[set.Count];
		int count = 0;
		foreach (T t in set) {
			array[count] = t;
			count++;
		}
		return array;
	}

	#endregion

	#region Enum Extensions

	/// <summary>
	/// Returns how many entries there are in the enum.
	/// </summary>
	public static int LengthOfEnum<TEnum>(this TEnum t)where TEnum : struct => System.Enum.GetNames(typeof(TEnum)).Length;

	/// <summary>
	/// Inserts spaces in between words of an enum.
	/// </summary>
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

	#endregion

	#region Vector Extensions

	public static Vector2 Add(this ref Vector2 v, float x, float y) => v + new Vector2(x, y);

	public static Vector3 ToV3(this Vector2 v, float z) => new Vector3(v.x, v.y, z);

	public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
	public static Vector2Int Abs(this Vector2Int v) => new Vector2Int(Mathf.Abs(v.x), Mathf.Abs(v.y));
	public static Vector3 Abs(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	public static Vector3Int Abs(this Vector3Int v) => new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

	public static Vector2 Round(this ref Vector2 v) => v = v.Rounded();
	public static Vector3 Round(this ref Vector3 v) => v = v.Rounded();
	public static Vector2 Rounded(this Vector2 v) => new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
	public static Vector3 Rounded(this Vector3 v) => new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
	public static Vector2Int RoundedToInt(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
	public static Vector3Int RoundedToInt(this Vector3 v) => new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));

	public static Vector2 Floor(this ref Vector2 v) => v = v.Floored();
	public static Vector3 Floor(this ref Vector3 v) => v = v.Floored();
	public static Vector2 Floored(this Vector2 v) => new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
	public static Vector3 Floored(this Vector3 v) => new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));

	public static Vector2 Ceil(this ref Vector2 v) => v = v.Ceiled();
	public static Vector3 Ceil(this ref Vector3 v) => v = v.Ceiled();
	public static Vector2 Ceiled(this Vector2 v) => new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
	public static Vector3 Ceiled(this Vector3 v) => new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));

	public static Vector2 Clamp(this ref Vector2 v, Vector2 a, Vector2 b) {
		v.x = Mathf.Clamp(v.x, Mathf.Min(a.x, b.x), Mathf.Max(a.x, b.x));
		v.y = Mathf.Clamp(v.y, Mathf.Min(a.y, b.y), Mathf.Max(a.y, b.y));
		return v;
	}

	#endregion

	#region Colour Extensions

	public static Color WithAlpha(this Color c, float alpha) {
		c.a = alpha;
		return c;
	}

	public static string ToHexRGBA(this Color c) {
		string r = ((int)Mathf.Lerp(0, 255, c.r)).ToString("X");
		string g = ((int)Mathf.Lerp(0, 255, c.g)).ToString("X");
		string b = ((int)Mathf.Lerp(0, 255, c.b)).ToString("X");
		string a = ((int)Mathf.Lerp(0, 255, c.a)).ToString("X");
		return (r.Length == 2 ? r : "0" + r) + (g.Length == 2 ? g : "0" + g) + (b.Length == 2 ? b : "0" + b) + (a.Length == 2 ? a : "0" + a);
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

	#endregion

	#region Bounds Extensions

	public static bool IsPointInside(this Bounds bounds, Vector2 point) =>
		point.x <= bounds.max.x && point.x >= bounds.min.x && point.y <= bounds.max.y && point.y >= bounds.min.y;

	#endregion

	#region MonoBehaviour Extensions

	public static Transform GetGrandChild(this Transform trans) {
		if (trans.childCount == 0)
			return trans;
		return trans.GetChild(0).GetGrandChild();
	}

	#endregion

	#endregion

}