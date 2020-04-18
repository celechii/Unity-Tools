// #if UNITY_EDITOR
using UnityEngine;

// by froodydude
// https://answers.unity.com/questions/486694/default-editor-enum-as-flags-.html?childToView=514102#answer-514102

// how to use cause ik you'll forget:
// [System.Flags] enum Weekday { Su = 1, M = 2, Tu = 4, W = 8, Th = 16, F = 32, Sa = 64}
// then
// [MultiEnum] public Weekday day;

public class MultiEnumAttribute : PropertyAttribute {
	public MultiEnumAttribute() {}
}
// #endif