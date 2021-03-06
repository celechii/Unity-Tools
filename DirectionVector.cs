﻿using System;
using UnityEngine;

public struct DirectionVector {

	/// <summary>
	/// The normalized direction. Set this property to change direction.
	/// </summary>
	public Vector2 Direction {
		get => dir;
		set {
			dir = value.normalized;
			if (dir != Vector2.zero) {
				if (additionalNonZeroCheck == null || additionalNonZeroCheck.Invoke())
					nonZero = dir;
			}
		}
	}

	/// <summary>
	/// The last direction value that wasn't zero.
	/// </summary>
	public Vector2 NonZero {
		get => nonZero;
	}

	/// <summary>
	/// The closest cardinal vector direction. Can be zero.
	/// </summary>
	public Vector2 Cardinal {
		get {
			if (dir == Vector2.zero)
				return dir;
			return Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) ? Vector2.right * Mathf.Sign(dir.x) : Vector2.up * Mathf.Sign(dir.y);
		}
	}

	/// <summary>
	/// The closest cardinal/intercardinal vector direction. Can be zero.
	/// </summary>
	public Vector2 EightDir {
		get {
			float[] snaps = new float[] {-1, 0, 1 };
			return new Vector2(RoundToNearest(dir.x, snaps), RoundToNearest(dir.y, snaps)).normalized;
		}
	}

	private Vector2 dir;
	private Vector2 nonZero;
	private Func<bool> additionalNonZeroCheck;

	/// <param name="direction">The direction to be normalized.</param>
	public DirectionVector(Vector2 direction) : this(direction, Vector2.down, null) {}

	/// <param name="direction">The direction to be normalized.</param>
	/// <param name="nonZeroDefault">The default non-zero direction if the direction has no magnitude.</param>
	public DirectionVector(Vector2 direction, Vector2 nonZeroDefault) : this(direction, nonZeroDefault, null) {}

	/// <param name="direction">The direction to be normalized.</param>
	/// <param name="additionalNonZeroCheck">An optional additional check to allow the non-zero value to be updated.</param>
	public DirectionVector(Vector2 direction, Func<bool> additionalNonZeroCheck) : this(direction, Vector2.down, additionalNonZeroCheck) {}

	/// <param name="direction">The direction to be normalized.</param>
	/// <param name="nonZeroDefault">The default non-zero direction if the direction has no magnitude.</param>
	/// <param name="additionalNonZeroCheck">An optional additional check to allow the non-zero value to be updated.</param>
	public DirectionVector(Vector2 direction, Vector2 nonZeroDefault, Func<bool> additionalNonZeroCheck) {
		dir = direction.normalized;
		if (dir == Vector2.zero)
			nonZero = nonZeroDefault;
		else
			nonZero = dir;
		this.additionalNonZeroCheck = additionalNonZeroCheck;
		Direction = direction;
	}

	private float RoundToNearest(float value, float[] snaps) {
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

	public static implicit operator Vector2(DirectionVector dv) => dv.dir;

	public static Vector2 operator *(DirectionVector dv, float f) => dv.dir * f;
	public static Vector2 operator /(DirectionVector dv, float f) => dv.dir / f;
	public static bool operator ==(DirectionVector lhs, DirectionVector rhs) => lhs.dir == rhs.dir && lhs.nonZero == rhs.nonZero;
	public static bool operator ==(DirectionVector dv, Vector2 v) => dv.dir == v;
	public static bool operator !=(DirectionVector lhs, DirectionVector rhs) => !(lhs == rhs);
	public static bool operator !=(DirectionVector dv, Vector2 v) => !(dv == v);

	public override string ToString() => $"{dir} -> {nonZero}";

	public override int GetHashCode() => dir.GetHashCode() + nonZero.GetHashCode();
	public override bool Equals(object obj) {
		if ((obj == null) || !GetType().Equals(obj.GetType()) && !GetType().Equals(typeof(Vector2)))
			return false;
		return this == (DirectionVector)obj || (Vector2)this == (Vector2)obj;
	}

}