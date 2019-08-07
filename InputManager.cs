using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public static InputManager control;
	public static InputType CurrentInputType { get { return control.inputType; } set { control.inputType = value; } }

	[SerializeField]
	private InputType inputType;
	[SerializeField]
	private InputActionBinding[] actions;

	[SerializeField]
	private float axisDead = .3f;

	//controller axis shit
	//ps4
	private bool lastDPadUp;
	private bool lastDPadDown;
	private bool lastDPadLeft;
	private bool lastDPadRight;
	//xbox one
	private bool lastTriggerLeft;
	private bool lastTriggerRight;

	[SerializeField]
	private bool isRebindingKey;

	public int GetNumControllers() => Input.GetJoystickNames().Length;

	// private void Awake() {
	// 	// in order to get actions quickly, they need to be sorted
	// 	// this has to be run before anything else can check inputs
	// 	// SO LETS DO A SORT LOL
	// 	int min;
	// 	InputActionBinding temp;
	// 	for (int i = 0; i < actions.Length - 1; i++) {
	// 		min = i;
	// 		for (int c = i + 1; c < actions.Length; c++)
	// 			if ((int)actions[c].action < (int)actions[min].action)
	// 				min = c;
	// 		temp = actions[min];
	// 		actions[min] = actions[i];
	// 		actions[i] = temp;
	// 	}
	// }

	private void Awake() {
		control = this;

		inputType = FindInputType();
	}

	// private void Update() {
	// 	foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode))) {
	// 		if (Input.GetKeyDown(k))
	// 			print(k.ToString() + " " + (int)k);
	// 	}
	// }

	public InputType FindInputType() {
		if (Input.GetJoystickNames().Length == 0)
			return InputType.Keyboard;
		switch (Input.GetJoystickNames()[0]) {
			case "Microsoft Xbox One Wired Controller":
				return InputType.XboxOne;
			case "Sony Interactive Entertainment Wireless Controller":
				return InputType.PS4;
			default:
				return InputType.Keyboard;

		}
	}

	private void LateUpdate() {
		//update the old things
		if (inputType == InputType.PS4) {
			float dpadXAxis = Input.GetAxis("7th axis");
			float dpadYAxis = Input.GetAxis("8th axis");
			lastDPadUp = dpadYAxis > axisDead;
			lastDPadDown = dpadYAxis < -axisDead;
			lastDPadLeft = dpadXAxis < -axisDead;
			lastDPadRight = dpadXAxis > axisDead;

		} else if (inputType == InputType.XboxOne) {
			lastTriggerLeft = Input.GetAxis("5th axis") < -axisDead;
			lastTriggerRight = Input.GetAxis("6th axis") < -axisDead;
		}
	}

	public static bool GetKeyDown(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKeyDown(b.primaryPosKey) || Input.GetKeyDown(b.secondaryPosKey);
				return control.GetControllerButtonDown(b.primaryPosButton) || control.GetControllerButtonDown(b.secondaryPosButton);
			}
		}
		return false;
	}

	public static bool GetNegativeKeyDown(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKeyDown(b.primaryNegKey) || Input.GetKeyDown(b.secondaryNegKey);
				return control.GetControllerButtonDown(b.primaryNegButton) || control.GetControllerButtonDown(b.secondaryNegButton);
			}
		}
		return false;
	}

	private bool GetControllerButtonDown(ControllerButton button) {
		if (inputType == InputType.PS4) {

			if (button == ControllerButton.DPadUp)
				return !lastDPadUp && Input.GetAxis("8th axis") > axisDead;
			else if (button == ControllerButton.DPadDown)
				return !lastDPadDown && Input.GetAxis("8th axis") < -axisDead;
			else if (button == ControllerButton.DPadLeft)
				return !lastDPadLeft && Input.GetAxis("7th axis") < -axisDead;
			else if (button == ControllerButton.DPadRight)
				return !lastDPadRight && Input.GetAxis("7th axis") > axisDead;
			else
				return Input.GetKeyDown(KeyFromPS4(button));

		} else {

			if (button == ControllerButton.TriggerLeft)
				return !lastTriggerLeft && Input.GetAxis("5th axis") < -axisDead;
			else if (button == ControllerButton.TriggerRight)
				return !lastTriggerRight && Input.GetAxis("6th axis") < -axisDead;
			else
				return Input.GetKeyDown(KeyFromXbox(button));
		}
	}

	public static bool GetKey(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKey(b.primaryPosKey) || Input.GetKey(b.secondaryPosKey);
				return control.GetControllerButton(b.primaryPosButton) || control.GetControllerButton(b.secondaryPosButton);
			}
		}
		return false;
	}

	public static bool GetNegativeKey(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKey(b.primaryNegKey) || Input.GetKey(b.secondaryNegKey);
				return control.GetControllerButton(b.primaryNegButton) || control.GetControllerButton(b.secondaryNegButton);
			}
		}
		return false;
	}

	private bool GetControllerButton(ControllerButton button) {
		if (inputType == InputType.PS4) {

			if (button == ControllerButton.DPadUp)
				return Input.GetAxis("8th axis") > axisDead;
			else if (button == ControllerButton.DPadDown)
				return Input.GetAxis("8th axis") < -axisDead;
			else if (button == ControllerButton.DPadLeft)
				return Input.GetAxis("7th axis") < -axisDead;
			else if (button == ControllerButton.DPadRight)
				return Input.GetAxis("7th axis") > axisDead;
			else
				return Input.GetKey(KeyFromPS4(button));

		} else {

			if (button == ControllerButton.TriggerLeft)
				return Input.GetAxis("5th axis") < 1;
			else if (button == ControllerButton.TriggerRight)
				return Input.GetAxis("6th axis") < 1;
			else
				return Input.GetKey(KeyFromXbox(button));
		}
	}

	public static bool GetKeyUp(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKeyUp(b.primaryPosKey) || Input.GetKeyUp(b.secondaryPosKey);
				return control.GetControllerButtonUp(b.primaryPosButton) || control.GetControllerButtonUp(b.secondaryPosButton);
			}
		}
		return false;
	}

	public static bool GetNegativeKeyUp(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {
				if (control.inputType == InputType.Keyboard)
					return Input.GetKeyUp(b.primaryNegKey) || Input.GetKeyUp(b.secondaryNegKey);
				return control.GetControllerButtonUp(b.primaryNegButton) || control.GetControllerButtonUp(b.secondaryNegButton);
			}
		}
		return false;
	}

	private bool GetControllerButtonUp(ControllerButton button) {
		if (inputType == InputType.PS4) {

			if (button == ControllerButton.DPadUp)
				return lastDPadUp && Input.GetAxis("8th axis") <= axisDead;
			else if (button == ControllerButton.DPadDown)
				return lastDPadDown && Input.GetAxis("8th axis") >= -axisDead;
			else if (button == ControllerButton.DPadLeft)
				return lastDPadLeft && Input.GetAxis("7th axis") >= -axisDead;
			else if (button == ControllerButton.DPadRight)
				return lastDPadRight && Input.GetAxis("7th axis") <= axisDead;
			else
				return Input.GetKeyUp(KeyFromPS4(button));

		} else {

			if (button == ControllerButton.TriggerLeft)
				return lastTriggerLeft && Input.GetAxis("5th axis") >= -axisDead;
			else if (button == ControllerButton.TriggerRight)
				return lastTriggerRight && Input.GetAxis("6th axis") >= -axisDead;
			else
				return Input.GetKeyUp(KeyFromXbox(button));
		}
	}

	public static float GetFloatAxis(InputAction action) {
		if (control.inputType == InputType.Keyboard)
			return GetAxis(action);

		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {

				float val = 0;

				if (b.primaryAxis == ControllerAxis.LeftStickX ||
					b.primaryAxis == ControllerAxis.LeftStickY ||
					b.primaryAxis == ControllerAxis.RightStickX ||
					b.primaryAxis == ControllerAxis.RightStickY ||
					b.primaryAxis == ControllerAxis.DPadX ||
					b.primaryAxis == ControllerAxis.DPadY)
					val += control.GetControllerAxisFromEnum(b.primaryAxis);
				if (b.secondaryAxis == ControllerAxis.LeftStickX ||
					b.secondaryAxis == ControllerAxis.LeftStickY ||
					b.secondaryAxis == ControllerAxis.RightStickX ||
					b.secondaryAxis == ControllerAxis.RightStickY ||
					b.secondaryAxis == ControllerAxis.DPadX ||
					b.secondaryAxis == ControllerAxis.DPadY)
					val += control.GetControllerAxisFromEnum(b.secondaryAxis);

				return Mathf.Clamp(val, -1, 1);
			}
		}
		return 0;
	}

	public static int GetAxis(InputAction action) {
		foreach (InputActionBinding b in control.actions) {
			if (b.action == action) {

				if (control.inputType == InputType.Keyboard) {
					int value = 0;
					value += Input.GetKey(b.primaryPosKey) ? 1 : 0;
					value += Input.GetKey(b.primaryNegKey) ? -1 : 0;
					value += Input.GetKey(b.secondaryPosKey) ? 1 : 0;
					value += Input.GetKey(b.secondaryNegKey) ? -1 : 0;
					return Mathf.Clamp(value, -1, 1);

				} else {
					if (b.primaryAxis != ControllerAxis.None || b.secondaryAxis != ControllerAxis.None) {

						int value = 0;

						if (b.primaryAxis == ControllerAxis.DPadX || b.secondaryAxis == ControllerAxis.DPadX)
							value += (control.GetControllerButton(ControllerButton.DPadLeft) ? -1 : 0) + (control.GetControllerButton(ControllerButton.DPadRight) ? 1 : 0);

						if (b.primaryAxis == ControllerAxis.DPadY || b.secondaryAxis == ControllerAxis.DPadY)
							value += (control.GetControllerButton(ControllerButton.DPadDown) ? -1 : 0) + (control.GetControllerButton(ControllerButton.DPadUp) ? 1 : 0);

						if (b.primaryAxis == ControllerAxis.LeftStickX || b.secondaryAxis == ControllerAxis.LeftStickX)
							value += MaxFloat(Input.GetAxis("X axis"));
						if (b.primaryAxis == ControllerAxis.LeftStickY || b.secondaryAxis == ControllerAxis.LeftStickY)
							value += MaxFloat(Input.GetAxis("Y axis"));
						if (b.primaryAxis == ControllerAxis.RightStickX || b.secondaryAxis == ControllerAxis.RightStickX)
							value += MaxFloat(Input.GetAxis("3rd axis"));
						if (b.primaryAxis == ControllerAxis.RightStickY || b.secondaryAxis == ControllerAxis.RightStickY)
							value += MaxFloat(Input.GetAxis("4th axis"));
						return Mathf.Clamp(value, -1, 1);

					} else
						return (GetKey(b.action) ? 1 : 0) + (GetNegativeKey(b.action) ? -1 : 0);
				}
			}
		}
		return 0;
	}

	private static int MaxFloat(float value) {
		int val = 0;
		if (value > control.axisDead)
			val++;
		else if (value < -control.axisDead)
			val--;
		return val;
	}

	private int FindActionIndex(InputAction action) {
		for (int i = 0; i < actions.Length; i++)
			if (actions[i].action == action)
				return i;
		throw new System.Exception("um wtf there is nothing bound to " + action);
	}

	public void RebindPrimaryPosKey(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindKey(FindActionIndex(action), BindingType.PrimaryPosKey));
	}

	public void RebindPrimaryNegKey(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindKey(FindActionIndex(action), BindingType.PrimaryNegKey));

	}

	public void RebindSecondaryPosKey(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindKey(FindActionIndex(action), BindingType.SecondaryPosKey));
	}

	public void RebindSecondaryNegKey(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindKey(FindActionIndex(action), BindingType.SecondaryNegKey));
	}

	public void RebindPrimaryPosButton(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindButton(FindActionIndex(action), BindingType.PrimaryPosButton));
	}

	public void RebindPrimaryNegButton(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindButton(FindActionIndex(action), BindingType.PrimaryNegButton));
	}

	public void RebindSecondaryPosButton(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindButton(FindActionIndex(action), BindingType.SecondaryPosButton));
	}

	public void RebindSecondaryNegButton(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindButton(FindActionIndex(action), BindingType.SecondaryNegButton));
	}

	public void RebindPrimaryAxis(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindAxis(FindActionIndex(action), BindingType.PrimaryAxis));
	}

	public void RebindSecondaryAxis(InputAction action) {
		if (!isRebindingKey)
			StartCoroutine(RebindAxis(FindActionIndex(action), BindingType.SecondaryAxis));
	}

	private IEnumerator RebindKey(int actionIndex, BindingType bindingType) {
		yield return null;

		isRebindingKey = true;
		while (true) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				isRebindingKey = false;
				yield break;
			}

			foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown(k)) {

					if (bindingType == BindingType.PrimaryPosKey)
						actions[actionIndex].primaryPosKey = k;
					else if (bindingType == BindingType.PrimaryNegKey)
						actions[actionIndex].primaryNegKey = k;
					else if (bindingType == BindingType.SecondaryPosKey)
						actions[actionIndex].secondaryPosKey = k;
					else if (bindingType == BindingType.SecondaryNegKey)
						actions[actionIndex].secondaryNegKey = k;
					else
						throw new System.Exception(bindingType + " isn't a key???");
					isRebindingKey = false;
					yield break;
				}
			}

			yield return null;
		}
	}

	private IEnumerator RebindButton(int actionIndex, BindingType bindingType) {
		isRebindingKey = true;

		while (true) {
			if (GetControllerButtonDown(ControllerButton.MenuRight)) {
				isRebindingKey = false;
				yield break;
			}

			for (int i = 0; i < System.Enum.GetValues(typeof(ControllerButton)).Length - 2; i++) {
				if (GetControllerButtonDown((ControllerButton)i)) {
					if (bindingType == BindingType.PrimaryPosButton)
						actions[actionIndex].primaryPosButton = (ControllerButton)i;
					else if (bindingType == BindingType.PrimaryNegButton)
						actions[actionIndex].primaryNegButton = (ControllerButton)i;
					else if (bindingType == BindingType.SecondaryPosButton)
						actions[actionIndex].secondaryPosButton = (ControllerButton)i;
					else if (bindingType == BindingType.SecondaryNegButton)
						actions[actionIndex].secondaryNegButton = (ControllerButton)i;
					else
						throw new System.Exception(bindingType + " isn't a button???");
					isRebindingKey = false;
					yield break;
				}
			}

			yield return null;
		}
	}

	private IEnumerator RebindAxis(int actionIndex, BindingType bindingType) {
		isRebindingKey = true;

		while (true) {
			if (GetControllerButtonDown(ControllerButton.MenuRight)) {
				isRebindingKey = false;
				yield break;
			}

			ControllerAxis toBindTo = ControllerAxis.None;
			if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.LeftStickX)) != 0)
				toBindTo = ControllerAxis.LeftStickX;
			else if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.LeftStickY)) != 0)
				toBindTo = ControllerAxis.LeftStickY;
			else if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.RightStickX)) != 0)
				toBindTo = ControllerAxis.RightStickX;
			else if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.RightStickY)) != 0)
				toBindTo = ControllerAxis.RightStickY;
			else if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.DPadX)) != 0)
				toBindTo = ControllerAxis.DPadX;
			else if (MaxFloat(GetControllerAxisFromEnum(ControllerAxis.DPadY)) != 0)
				toBindTo = ControllerAxis.DPadY;

			if (toBindTo != ControllerAxis.None) {
				if (bindingType == BindingType.PrimaryAxis)
					actions[actionIndex].primaryAxis = toBindTo;
				else
					actions[actionIndex].secondaryAxis = toBindTo;
				isRebindingKey = false;
				yield break;

			}
			yield return null;
		}
	}

	private KeyCode KeyFromPS4(ControllerButton button) {
		switch (button) {
			case ControllerButton.FaceUp:
				return KeyCode.Joystick1Button3;
			case ControllerButton.FaceDown:
				return KeyCode.Joystick1Button1;
			case ControllerButton.FaceLeft:
				return KeyCode.Joystick1Button0;
			case ControllerButton.FaceRight:
				return KeyCode.Joystick1Button2;
			case ControllerButton.TriggerLeft:
				return KeyCode.Joystick1Button6;
			case ControllerButton.TriggerRight:
				return KeyCode.Joystick1Button7;
			case ControllerButton.BumperLeft:
				return KeyCode.Joystick1Button4;
			case ControllerButton.BumperRight:
				return KeyCode.Joystick1Button5;
			case ControllerButton.MenuLeft:
				return KeyCode.Joystick1Button8;
			case ControllerButton.MenuRight:
				return KeyCode.Joystick1Button9;
			case ControllerButton.StickLeft:
				return KeyCode.Joystick1Button10;
			case ControllerButton.StickRight:
				return KeyCode.Joystick1Button11;
			default:
				return KeyCode.None;
		}
	}

	private KeyCode KeyFromXbox(ControllerButton button) {
		switch (button) {
			case ControllerButton.FaceUp:
				return KeyCode.Joystick1Button19;
			case ControllerButton.FaceDown:
				return KeyCode.Joystick1Button16;
			case ControllerButton.FaceLeft:
				return KeyCode.Joystick1Button18;
			case ControllerButton.FaceRight:
				return KeyCode.Joystick1Button17;
			case ControllerButton.BumperLeft:
				return KeyCode.Joystick1Button13;
			case ControllerButton.BumperRight:
				return KeyCode.Joystick1Button14;
			case ControllerButton.MenuLeft:
				return KeyCode.Joystick1Button10;
			case ControllerButton.MenuRight:
				return KeyCode.Joystick1Button9;
			case ControllerButton.StickLeft:
				return KeyCode.Joystick1Button11;
			case ControllerButton.StickRight:
				return KeyCode.Joystick1Button12;
			case ControllerButton.DPadUp:
				return KeyCode.Joystick1Button5;
			case ControllerButton.DPadDown:
				return KeyCode.Joystick1Button6;
			case ControllerButton.DPadLeft:
				return KeyCode.Joystick1Button7;
			case ControllerButton.DPadRight:
				return KeyCode.Joystick1Button8;
			default:
				return KeyCode.None;
		}
	}

	private float GetControllerAxisFromEnum(ControllerAxis axis) {
		if (axis == ControllerAxis.LeftStickX)
			return Input.GetAxis("X axis");
		else if (axis == ControllerAxis.LeftStickY)
			return Input.GetAxis("Y axis");
		else if (axis == ControllerAxis.RightStickX)
			return Input.GetAxis("3rd axis");
		else if (axis == ControllerAxis.RightStickY)
			return Input.GetAxis("4th axis");
		else if (axis == ControllerAxis.DPadX)
			return (control.GetControllerButton(ControllerButton.DPadLeft) ? -1 : 0) + (control.GetControllerButton(ControllerButton.DPadRight) ? 1 : 0);
		else if (axis == ControllerAxis.DPadY)
			return (control.GetControllerButton(ControllerButton.DPadDown) ? -1 : 0) + (control.GetControllerButton(ControllerButton.DPadUp) ? 1 : 0);
		else return 0;
	}

	[System.Serializable]
	private struct InputActionBinding {
		public string name;
		public InputAction action;
		[Header("Keyboard")]
		public KeyCode primaryPosKey;
		public KeyCode primaryNegKey;
		public KeyCode secondaryPosKey;
		public KeyCode secondaryNegKey;
		[Header("Controller")]
		//if not axis
		public ControllerButton primaryPosButton;
		public ControllerButton primaryNegButton;
		public ControllerButton secondaryPosButton;
		public ControllerButton secondaryNegButton;
		//if axis
		public ControllerAxis primaryAxis;
		public ControllerAxis secondaryAxis;
	}

	public enum InputType {
		Keyboard,
		PS4,
		XboxOne
	}

	public enum ControllerButton {
		None,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,
		FaceUp,
		FaceDown,
		FaceLeft,
		FaceRight,
		TriggerLeft,
		TriggerRight,
		BumperLeft,
		BumperRight,
		StickLeft,
		StickRight,
		MenuLeft,
		MenuRight
	}

	public enum ControllerAxis {
		None,
		LeftStickX,
		LeftStickY,
		RightStickX,
		RightStickY,
		DPadX,
		DPadY
	}

	public enum BindingType {
		PrimaryPosKey,
		PrimaryNegKey,
		SecondaryPosKey,
		SecondaryNegKey,
		PrimaryPosButton,
		PrimaryNegButton,
		SecondaryPosButton,
		SecondaryNegButton,
		PrimaryAxis,
		SecondaryAxis
	}
}

//put input actions here
public enum InputAction {
	Escape,
	MoveX,
	MoveY,
	Interact,
	Inventory,
	Jump,
	Submit
}