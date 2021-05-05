using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, label);
		SerializedProperty minProp = property.FindPropertyRelative("min");
		SerializedProperty maxProp = property.FindPropertyRelative("max");

		EditorGUI.BeginChangeCheck();
		float[] values = new float[2] { minProp.floatValue, maxProp.floatValue };
		EditorGUI.MultiFloatField(position, new GUIContent[] { new GUIContent("Min"), new GUIContent("Max") }, values);

		if (EditorGUI.EndChangeCheck()) {
			minProp.floatValue = values[0];
			maxProp.floatValue = values[1];
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUIUtility.singleLineHeight;
	}
}