using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NameElementsAttribute))]
public class NameElementsDrawer : PropertyDrawer {

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {

		object obj = fieldInfo.GetValue(property.serializedObject.targetObject);

		if (obj != null && obj.GetType().IsArray) {
			int index = int.Parse(property.propertyPath.Split('[', ']')[1]);
			string name = "";
			if (((NameElementsAttribute)attribute).drawIndex)
				name = index + ": ";

			IList list = (IList)obj;

			if (index < list.Count && index >= 0)
				name += list[index] == null ? "Empty" : list[index].ToString();
			else
				return;

			label.text = name;

			EditorGUI.BeginProperty(rect, label, property);
			EditorGUI.PropertyField(rect, property, label, true);

			EditorGUI.EndProperty();
		} else {
			EditorGUI.PropertyField(rect, property);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight(property, true);
	}
}