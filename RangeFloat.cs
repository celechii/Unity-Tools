using UnityEngine;

[System.Serializable]
public class RangeFloat {
	[SerializeField]
	public float min, max;
	private float value;

	public float Value { get { return value; } set { this.value = Mathf.Clamp(value, min, max); } }

	public RangeFloat() {
		min = 0;
		max = 1;
	}

	public RangeFloat(float min, float max) {
		this.min = min;
		this.max = max;
	}

	public RangeFloat(float min, float max, float value) {
		this.min = min;
		this.max = max;
		this.value = value;
	}

	/// <summary>
	/// Clamps the value between the min and max.
	/// </summary>
	public void ClampValue() {
		value = Mathf.Clamp(value, min, max);
	}

	/// <summary>
	/// Adds or subtracts from the value.
	/// </summary>
	private void AddToValue(float amount) {
		value += amount;
	}

	/// <summary>
	/// Adds or subtracts from the value.
	/// </summary>
	private void AddToValue(int amount) {
		value += amount;
	}

	/// <summary>
	/// Sets the value to the max and returns it.
	/// </summary>
	public float ToMax() {
		value = max;
		return max;
	}

	/// <summary>
	/// Sets the value to the min and returns it.
	/// </summary>
	public float ToMin() {
		value = min;
		return min;
	}

	/// <summary>
	/// Sets a random float value between the min and the max (inclusive).
	/// </summary>
	public void SetValueToRandomFloat() {
		value = GetRandomFloat();
	}

	/// <summary>
	/// Sets a random int value between the min and the max (inclusive).
	/// </summary>
	public void SetValueToRandomInt() {
		value = (float)GetRandomInt();
	}

	/// <summary>
	/// Sets the value to a percent lerp between the min and max.
	/// </summary>
	public void SetValueToPercent(float percent) {
		value = GetAt(percent);
	}

	/// <summary>
	/// Sets the value to an unclamped percent lerp between the min and max.
	/// </summary>
	public void SetValueToUnclampedPercent(float percent) {
		value = GetUnclampedAt(percent);
	}

	/// <summary>
	/// Returns the min and max range.
	/// </summary>
	public float GetRange() {
		return max - min;
	}

	/// <summary>
	/// Returns a value t% between the min and max.
	/// </summary>
	public float GetAt(float t) {
		return Mathf.Lerp(min, max, t);
	}

	/// <summary>
	/// Returns a value t% between the min and max according to an animation curve.
	/// </summary>
	public float GetAtWith(float t, AnimationCurve curve) {
		return GetAt(curve.Evaluate(t));
	}

	/// <summary>
	/// Returns a value t% between the min and max, but also maybe outside of them, cause unclamped and stuff.
	/// </summary>
	public float GetUnclampedAt(float t) {
		return Mathf.LerpUnclamped(min, max, t);
	}

	/// <summary>
	/// Returns a value t% between the min and max, but also maybe outside of them, cause unclamped and stuff.
	/// </summary>
	public float GetUnclampedAtWith(float t, AnimationCurve curve) {
		return curve.Evaluate(GetUnclampedAt(t));
	}

	/// <summary>
	/// Returns how far into the range a value is.
	/// </summary>
	public float GetPercent(float value) {
		return Mathf.InverseLerp(min, max, value);
	}

	/// <summary>
	/// Returns how far into the range a value is, then scales it according to an animation curve.
	/// </summary>
	public float GetPercentWith(float value, AnimationCurve curve) {
		return curve.Evaluate(GetPercent(value));
	}

	/// <summary>
	/// Returns how far in % the value is between the min and max.
	/// </summary>
	public float GetPercentOfValue() {
		return GetPercent(value);
	}

	/// <summary>
	/// Returns how far in % the value is between the min and max, scaling with an animation curve.
	/// </summary>
	public float GetPercentOfValueWith(AnimationCurve curve) {
		return curve.Evaluate(GetPercent(value));
	}

	/// <summary>
	/// Returns either min or the max, at 50/50 chance each.
	/// </summary>
	public float GetExtreme() {
		return GetExtreme(.5f);
	}

	/// <summary>
	/// Returns either the min or the max, given the chance distribution.
	/// 0 will give the min, 1 will give the max. 
	/// </summary>
	public float GetExtreme(float distribution) {
		distribution = Mathf.Clamp01(distribution);
		return Random.value > distribution ? max : min;
	}

	/// <summary>
	/// Determines whether the value passed is in range the specified min and max (inclusive).
	/// </summary>
	public bool IsInRange(float value) {
		return value >= min && value <= max;
	}

	/// <summary>
	/// Returns a random float between the min and max (inclusive).
	/// </summary>
	public float GetRandomFloat() {
		return Mathf.Lerp(min, max, Random.value);
	}

	/// <summary>
	/// Returns a random int between the min and max (inclusive).
	/// Should the min or max be real numbers, this may return the number either above the max or below the min.
	/// </summary>
	public int GetRandomInt() {
		return Mathf.RoundToInt(GetRandomFloat());
	}

	/// <summary>
	/// Retuns a Vector2 with x and y components at random positions between the min and max.
	/// </summary>
	public Vector2 GetRandomV2() {
		return new Vector2(GetRandomFloat(), GetRandomFloat());
	}

	/// <summary>
	/// Retuns a Vector3 with x, y, and z components at random positions between the min and max.
	/// </summary>
	public Vector3 GetRandomV3() {
		return new Vector3(GetRandomFloat(), GetRandomFloat(), GetRandomFloat());
	}

	/// <summary>
	/// Returns a Vector2 with a random direction, and a magnitude in between the min and max (inclusive).
	/// </summary>
	public Vector2 GetRandomDistance() {
		return (new Vector2(Random.value * 2 - 1, Random.value * 2 - 1)) * GetRandomFloat();
	}

	public static RangeFloat operator +(RangeFloat r1, float val) {
		r1.AddToValue(val);
		return r1;
	}

	public static RangeFloat operator +(RangeFloat r1, int val) {
		r1.AddToValue(val);
		return r1;
	}

	public static RangeFloat operator +(RangeFloat r1, RangeFloat r2) {
		r1.AddToValue(r2.Value);
		return r1;
	}

	public static RangeFloat operator -(RangeFloat r1, float val) {
		r1.AddToValue(-val);
		return r1;
	}

	public static RangeFloat operator -(RangeFloat r1, int val) {
		r1.AddToValue(-val);
		return r1;
	}

	public static RangeFloat operator -(RangeFloat r1, RangeFloat r2) {
		r1.AddToValue(-r2.Value);
		return r1;
	}

	public static RangeFloat operator *(RangeFloat r1, float val) {
		r1.Value = r1.Value * val;
		return r1;
	}

	public static RangeFloat operator *(RangeFloat r1, int val) {
		r1.Value = r1.Value * val;
		return r1;
	}

	public static RangeFloat operator *(RangeFloat r1, RangeFloat r2) {
		r1.Value = r1.Value * r2.Value;
		return r1;
	}

	public static RangeFloat operator /(RangeFloat r1, float val) {
		r1.Value = r1.Value / val;
		return r1;
	}

	public static RangeFloat operator /(RangeFloat r1, int val) {
		r1.Value = r1.Value / val;
		return r1;
	}

	public static RangeFloat operator /(RangeFloat r1, RangeFloat r2) {
		r1.Value = r1.Value / r2.Value;
		return r1;
	}

	public override string ToString() => $"{min} -> {max} @ {value} ({(int)(GetPercentOfValue()*100)}%)";
}