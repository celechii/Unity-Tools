using UnityEngine;

public static class Extensions {

	public static string ToHexRGBA(this Color c) {
		string r = ((int) Mathf.Lerp(0, 255, c.r)).ToString("X");
		string g = ((int) Mathf.Lerp(0, 255, c.g)).ToString("X");
		string b = ((int) Mathf.Lerp(0, 255, c.b)).ToString("X");
		string a = ((int) Mathf.Lerp(0, 255, c.a)).ToString("X");
		return (r.Length == 2 ? r : "0" + r) + (g.Length == 2 ? g : "0" + g) + (b.Length == 2 ? b : "0" + b) + (a.Length == 2 ? a : "0" + a);
	}

	public static string Colour(this string s, Color c) {
		string cHex = c.ToHexRGBA();
		return "<color=#" + cHex + ">" + s + "</color>";
	}

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

	public static bool IsUpper(this char c) => c >= 'A' && c <= 'Z';

	public static bool IsLower(this char c) => c >= 'a' && c <= 'z';


	// from freya holmér <3
	// https://twitter.com/FreyaHolmer/status/1068280371907883008
	public static float Elerp(float a, float b, float t) {
		float a1 = Mathf.Log10(a);
		float b1 = Mathf.Log10(b);
		float exp = Mathf.LerpUnclamped(a1, b1, t) * 3.321922809489f;
		return Mathf.Pow(2, exp);
	}

	// from ashley @khyperia <3
	// https://twitter.com/khyperia
	// https://twitter.com/FreyaHolmer/status/1068293398073929728
	public static float Eerp(float a, float b, float t) {
		return a * Mathf.Pow(b / a, t);
	}
}
