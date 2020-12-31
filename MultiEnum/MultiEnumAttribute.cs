using UnityEngine;

// by froodydude
// https://answers.unity.com/questions/486694/default-editor-enum-as-flags-.html?childToView=514102#answer-514102

// how to use cause ik you'll forget:
// [System.Flags]
// enum Weekday {
// 	Su = 1 << 0,
// 	M = 1 << 1,
// 	Tu = 1 << 2,
// 	W = 1 << 3,
// 	Th = 1 << 4,
// 	F = 1 << 5,
// 	Sa = 1 << 6
// }

// [MultiEnum]
// public Weekday day;

public class MultiEnumAttribute : PropertyAttribute {
	public MultiEnumAttribute() {}
}