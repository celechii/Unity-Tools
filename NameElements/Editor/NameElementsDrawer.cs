/*
MIT License

Copyright (c) 2021 Noé Charron

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NameElementsAttribute))]
public class NameElementsDrawer : PropertyDrawer {

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {

		object obj = fieldInfo.GetValue(property.serializedObject.targetObject);

		if (obj != null && (obj.GetType().IsArray || typeof(IList).IsAssignableFrom(obj.GetType()))) {
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
		}

		EditorGUI.PropertyField(rect, property, label, true);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight(property, true);
	}
}