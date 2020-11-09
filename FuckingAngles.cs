using UnityEngine;

public static class FuckingAngles {

	/// <summary>
	/// Converts any angle to an unsigned angle.
	/// (Range of 0º to 359º) 
	/// </summary>
	/// <param name="angle">Signed angle.</param>
	public static float Angle(float angle) {
		angle = angle % 360f;
		if (angle < 0)
			angle = 360 - angle;
		return angle;
	}

	/// <summary>
	/// Adds degrees to an angle accounting for wrapping over 360º and under 0º
	/// </summary>
	/// <param name="angle">The initial angle.</param>
	/// <param name="amount">Degrees to add.</param>
	public static float AddToAngle(float angle, float amount) {
		return Angle(angle + amount);
	}

	/// <summary>
	/// Returns a normalized Vector2 direction.
	/// </summary>
	/// <param name="angleInDeg">Angle in degrees.</param>
	public static Vector2 DirFromAngle(float angleInDeg, float offset = 0) {
		if (offset != 0)
			angleInDeg = AddToAngle(angleInDeg, offset);
		return new Vector2(Mathf.Cos(angleInDeg * Mathf.Deg2Rad), Mathf.Sin(angleInDeg * Mathf.Deg2Rad));
	}

	/// <summary>
	/// Returns the angle of the vector direction.
	/// (Range of 0º to 359º)
	/// </summary>
	/// <param name="dir">The direction.</param>
	/// <param name="offset">Offset angle.</param>
	public static float AngleFromDir(Vector2 dir, float offset = 0) {
		return Angle(Vector2.SignedAngle(Vector2.right, dir) + offset);
	}

	/// <summary>
	/// Returns the angle between two vector positions.
	/// (Range of 0º to 359º)
	/// </summary>
	/// <param name="a">The start position.</param>
	/// <param name="b">The end position.</param>
	/// <param name="offset">Offset angle.</param>
	public static float AngleBetweenPoints(Vector2 a, Vector2 b, float offset = 0) {
		Vector2 difference = b - a;
		return AngleFromDir(difference, offset);
	}

	/// <summary>
	/// Converts any angle to a signed angle.
	/// (Range of 180º to -179º)
	/// </summary>
	/// <returns>The signed angle.</returns>
	/// <param name="angle">Angle.</param>
	public static float SignedAngle(float angle) {
		angle = Angle(angle);
		if (angle > 180)
			angle -= 180;
		return angle;
	}

	/// <summary>
	/// Adds degrees to an angle accounting for wrapping over 180º and under -179º
	/// </summary>
	/// <param name="angle">The initial angle.</param>
	/// <param name="amount">Degrees to add.</param>
	public static float AddToSignedAngle(float angle, float amount) {
		return SignedAngle(angle + amount);
	}

	/// <summary>
	/// Returns the signed angle of the vector direction.
	/// (Range of -180º to 180º)
	/// </summary>
	/// <param name="direction">The direction.</param>
	public static float SignedAngleFromDir(Vector2 direction, float offset = 0) {
		return SignedAngle(AngleFromDir(direction, offset));
	}

	/// <summary>
	/// Returns the signed angle between the two vector positions.
	/// (Range of -180º to 180º)
	/// </summary>
	/// <param name="a">The start position.</param>
	/// <param name="b">The end position.</param>
	public static float SignedAngleBetweenPoints(Vector2 a, Vector2 b) {
		Vector2 diference = b - a;
		return SignedAngleFromDir(diference);
	}

	/// <summary>
	/// Returns a perpendicular vector to the vector provided.
	/// </summary>
	/// <param name="dir">The direction.</param>
	/// <param name="direction">Negative value for left, positive value for right.</param>
	public static Vector2 GetPerpendicular(Vector2 dir, int direction = -1) => new Vector2(dir.y * Mathf.Sign(direction), dir.x * -Mathf.Sign(direction));

	/// <summary>
	/// Rotates a Transform to point the right side towards the position of a target Transform.
	/// </summary>
	/// <param name="trans">The transform to be rotated.</param>
	/// <param name="target">The target position to be pointing towards.</param>
	/// <param name="maxDegrees">The maximum degrees to rotate towards.</param>
	/// <param name="offset">Degrees to offset transform forward. 0 = right, 90 = up, 180 = left, 270 = down</param>
	/// <param name="selfWorldSpace">Use the ref transform's worldspace position?</param>
	/// <param name="targetWorldSpace">Use the target transform's worldspace position?</param>
	public static void RotateTowards(ref Transform trans, Transform target, float maxDegrees = Mathf.Infinity, float offset = 0, bool selfWorldSpace = false, bool targetWorldSpace = false) {
		RotateTowards(ref trans, targetWorldSpace ? target.position : target.localPosition, offset, maxDegrees, selfWorldSpace);
	}

	/// <summary>
	/// Rotates a Transform to point the right side towards a target position.
	/// </summary>
	/// <param name="trans">The transform to be rotated.</param>
	/// <param name="target">The target position to be pointing towards.</param>
	/// <param name="maxDegrees">The maximum degrees to rotate towards.</param>
	/// <param name="offset">Degrees to offset transform forward. 0 = right, 90 = up, 180 = left, 270 = down</param>
	/// <param name="worldSpace">Use the transform's worldspace position?</param>
	public static void RotateTowards(ref Transform trans, Vector2 target, float maxDegrees = Mathf.Infinity, float offset = 0, bool worldSpace = false) {
		Vector2 selfPos = worldSpace ? trans.position : trans.localPosition;
		Vector2 currentDir = DirFromAngle(worldSpace ? trans.eulerAngles.z : trans.localEulerAngles.z, offset);
		Vector2 targetDir = target - selfPos;
		trans.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(currentDir, targetDir), -maxDegrees, maxDegrees), Space.Self);
	}
}