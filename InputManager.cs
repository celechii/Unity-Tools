using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public bool isRebinding;
	[SerializeField]
	private Keybinds keyBinds;

	private Dictionary<string, InputAction> lookup;
	private int numKeyCodes;

	private void Awake() {
		numKeyCodes = System.Enum.GetNames(typeof(KeyCode)).Length;

		LoadKeyBinds();
		BuildLookup();
	}t

	public bool GetKeyDown(string name, bool negative = false) {
		CheckNameValid(name);
		InputAction action = lookup[name];

		return Input.GetKeyDown(negative?action.modifiedNegative : action.modifiedPositive);
	}

	public bool GetKey(string name, bool negative = false) {
		CheckNameValid(name);
		InputAction action = lookup[name];

		return Input.GetKey(negative?action.modifiedNegative : action.modifiedPositive);
	}

	public bool GetKeyUp(string name, bool negative = false) {
		CheckNameValid(name);
		InputAction action = lookup[name];

		return Input.GetKeyUp(negative?action.modifiedNegative : action.modifiedPositive);
	}

	public KeyCode GetBinding(string name, bool negative = false) => negative ? lookup[name].modifiedNegative : lookup[name].modifiedPositive;

	public int GetAxis(string name) => (GetKey(name) ? 1 : 0) + (GetKey(name, true) ? 1 : 0);

	public bool GetAxisDown(string name) => GetKeyDown(name) || GetKeyDown(name, true);

	public void ResetAllBindings() {
		for (int i = 0; i < keyBinds.actions.Length; i++)
			keyBinds.actions[i].ResetBindings();
		SaveKeyBinds();
	}

	private void CheckNameValid(string name) {
		if (!lookup.ContainsKey(name))
			throw new System.Exception($"hmm fuck, dont seem to have any binding w the name {name}???");
	}

	public void ResetBinding(string name, bool negative = false) {
		CheckNameValid(name);

		lookup[name].ResetBinding(negative);
		SaveKeyBinds();
	}

	// to b used w SaveSystem.cs on github:
	// https://github.com/celechii/Unity-Tools/blob/7d1182ea0f34b9b517bdbd67f88cb275316f1c2e/SaveSystem.cs
	// <3
	public void SaveKeyBinds() {
		SaveSystem.SaveTxt("Keybinds", keyBinds);
	}

	public void LoadKeyBinds() {
		if (SaveSystem.SaveExists("Keybinds"))
			keyBinds = SaveSystem.LoadTxt<Keybinds>("Keybinds");
		else
			ResetAllBindings();
		BuildLookup();
	}

	private void BuildLookup() {
		if (lookup == null)
			lookup = new Dictionary<string, InputAction>();
		else
			lookup.Clear();

		for (int i = 0; i < keyBinds.actions.Length; i++)
			lookup.Add(keyBinds.actions[i].name, keyBinds.actions[i]);
	}

	public void RebindKey(string name, System.Action callback = null, bool negative = false) {
		CheckNameValid(name);
		StartCoroutine(RebindInput(name, callback, negative));
	}

	private IEnumerator RebindInput(string name, System.Action callback = null, bool negative = false) {
		isRebinding = true;
		yield return null;

		while (isRebinding) {
			if (Input.anyKeyDown) {
				for (int i = 0; i < numKeyCodes; i++) {
					if (Input.GetKeyDown((KeyCode)i)) {
						lookup[name].SetKey((KeyCode)i, negative);
						isRebinding = false;
					}
				}
			}
			yield return null;
		}
		SaveKeyBinds();
		callback?.Invoke();
	}

	[System.Serializable]
	private struct Keybinds {
		public InputAction[] actions;
	}

	[System.Serializable]
	public class InputAction {
		public string name;
		public KeyCode positive;
		public KeyCode negative;
		[HideInInspector]
		public KeyCode modifiedPositive;
		[HideInInspector]
		public KeyCode modifiedNegative;

		public void SetKey(KeyCode key, bool negative = false) {
			if (negative)
				modifiedNegative = key;
			else
				modifiedPositive = key;
		}

		public void ResetBindings() {
			modifiedPositive = positive;
			modifiedNegative = negative;
		}

		public void ResetBinding(bool negative = false) {
			if (negative)
				modifiedNegative = this.negative;
			else
				modifiedPositive = positive;
		}
	}
}