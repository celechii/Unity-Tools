using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {

	private static InputManager control;

	private void Awake() {
		control = this;
	}

	[SerializeField]
	private KeyInput[] inputs;

	public static string GetKeyString(string input) {
		return GetKeyString(input, InputType.Positive);
	}

	public static string GetKeyString(string input, InputType inputType) {
		foreach (KeyInput s in control.inputs) {
			if (input == s.name) {
				if (inputType == InputType.Positive)
					return s.positive.ToString();
				else if (inputType == InputType.Negative)
					return s.negative.ToString();
				else if (inputType == InputType.PositiveAlt)
					return s.positiveAlt.ToString();
				else if (inputType == InputType.NegativeAlt)
					return s.negativeAlt.ToString();
				else break;
			}
		}
		return "shit, no input called " + input + " was set up :(";
	}

	public static bool GetKeyDown(string key) {
		return Input.GetKeyDown(control.SearchForKey(key).positive) || Input.GetKeyDown(control.SearchForKey(key).positiveAlt);
	}

	public static bool GetKey(string key) {
		return Input.GetKey(control.SearchForKey(key).positive) || Input.GetKey(control.SearchForKey(key).positiveAlt);
	}

	public static bool GetKeyUp(string key) {
		return Input.GetKeyUp(control.SearchForKey(key).positive) || Input.GetKeyUp(control.SearchForKey(key).positiveAlt);
	}

	public static bool GetKeyDown(string key, bool getNegative) {
		KeyInput input = control.SearchForKey(key);
		return Input.GetKeyDown(getNegative ? input.negative : input.positive) || Input.GetKeyDown(getNegative ? input.negativeAlt : input.positiveAlt);
	}

	public static bool GetKey(string key, bool getNegative) {
		KeyInput input = control.SearchForKey(key);
		return Input.GetKey(getNegative ? input.negative : input.positive) || Input.GetKey(getNegative ? input.negativeAlt : input.positiveAlt);
	}

	public static bool GetKeyUp(string key, bool getNegative) {
		KeyInput input = control.SearchForKey(key);
		return Input.GetKeyUp(getNegative ? input.negative : input.positive) || Input.GetKeyUp(getNegative ? input.negativeAlt : input.positiveAlt);
	}

	public static int GetAxis(string axis) {
		KeyInput input = control.SearchForKey(axis);
		return (Input.GetKey(input.positive) || Input.GetKey(input.positiveAlt) ? 1 : 0) - (Input.GetKey(input.negative) || Input.GetKey(input.negativeAlt) ? 1 : 0);
	}

	public static bool GetAxisDown(string axis) {
		KeyInput input = control.SearchForKey(axis);
		return Input.GetKeyDown(input.positive) || Input.GetKeyDown(input.negative) || Input.GetKeyDown(input.positiveAlt) || Input.GetKeyDown(input.negativeAlt);
	}

	public static void RemapPositive(string key, KeyCode val) {
		control.inputs[control.SearchForIndex(key)].positive = val;
	}

	public static void RemapNegative(string key, KeyCode val) {
		control.inputs[control.SearchForIndex(key)].negative = val;
	}

	public static void RemapSecondaryPositive(string key, KeyCode val) {
		control.inputs[control.SearchForIndex(key)].positiveAlt = val;
	}

	public static void RemapSecondaryNegative(string key, KeyCode val) {
		control.inputs[control.SearchForIndex(key)].negativeAlt = val;
	}

	public static void RemapPositive(string key) {
		control.StartCoroutine(control.RemapKey(control.SearchForIndex(key), false, false));
	}

	public static void RemapNegative(string key) {
		control.StartCoroutine(control.RemapKey(control.SearchForIndex(key), true, false));
	}

	public static void RemapPositive(string key, bool secondary) {
		control.StartCoroutine(control.RemapKey(control.SearchForIndex(key), false, secondary));
	}

	public static void RemapNegative(string key, bool secondary) {
		control.StartCoroutine(control.RemapKey(control.SearchForIndex(key), true, secondary));
	}

	private IEnumerator RemapKey(int index, bool negative, bool secondary) {
		yield return null;

		yield return new WaitUntil(() => Input.anyKeyDown);
		KeyCode newKey;
		string input = Input.inputString;

		if (input[0] == '\b')
			newKey = KeyCode.Backspace;
		else if (input[0] == '\n')
			newKey = KeyCode.KeypadEnter;
		else if (input[0] == '\r')
			newKey = KeyCode.Return;
		else
			newKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), input[0].ToString());

		if (!secondary) {
			if (negative)
				inputs[index].negative = newKey;
			else
				inputs[index].positive = newKey;
		} else {
			if (negative)
				inputs[index].negativeAlt = newKey;
			else
				inputs[index].positiveAlt = newKey;
		}

	}

	private KeyInput SearchForKey(string keyName) {
		foreach (KeyInput k in inputs)
			if (k.name == keyName)
				return k;
		Debug.LogError("o heck i can't find " + keyName);
		return new KeyInput();
	}

	private int SearchForIndex(string keyName) {
		for (int i = 0; i < inputs.Length; i++)
			if (inputs[i].name == keyName)
				return i;
		return 0;
	}

	[System.Serializable]
	private struct KeyInput {
		public string name;
		public KeyCode positive;
		public KeyCode negative;
		public KeyCode positiveAlt;
		public KeyCode negativeAlt;
	}

	public enum InputType {
		Positive,
		Negative,
		PositiveAlt,
		NegativeAlt
	}
}
