/*
MIT License
Copyright (c) 2022 Noé Charron
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

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateNav : MonoBehaviour {

	private enum BaseStateEnterInvocation { DontInvoke, OnAwake, OnStart }

	private enum LogDetail { DontLog, OnlyStateChanges, All }

	/// <summary>
	/// The current state the nav is in.
	/// </summary>
	public State CurrentState => path.Count > 0 ? path[path.Count - 1] : null;
	public State PreviousState { get; private set; }
	public bool IsInBaseState => CurrentState == baseState;

	/// <summary>
	/// Event called every time the nav switches to a new state.
	/// Provides the previous state, then the new state.
	/// </summary>
	public event Action<State, State> SwitchedStates = delegate {};

	[SerializeField]
	private State baseState;
	public State BaseState => baseState;

	[Tooltip("States that can be transitioned to from any state.")]
	[SerializeField]
	private List<State> fromAnyState = new List<State>();

	/// <summary>
	/// Should this invoke the base state's enter on start?
	/// </summary>
	[Space]
	[Tooltip("Should this invoke the base state's enter on start?")]
	[SerializeField]
	private BaseStateEnterInvocation baseStateEnterInvocation = BaseStateEnterInvocation.OnStart;

	/// <summary>
	/// Set this to true if you want the path to clear upon entering the base state.
	/// This allows you to have loops in your state flow while preventing you from
	/// leaving the base state back to its parent state.
	/// 
	/// You can do this on a case by case basis by calling <see cref="ClearPath"> after
	/// transitioning to the base state.
	/// </summary>
	public bool clearPathOnEnterBaseState;
	[Space]
	[SerializeField]
	private LogDetail logDetail = LogDetail.OnlyStateChanges;
	[SerializeField]
	private bool drawPath;

	private List<State> path = new List<State>();

	private void Awake() {
		path.Clear();

		if (baseState != null) {
			path.Add(baseState);

			if (baseStateEnterInvocation == BaseStateEnterInvocation.OnAwake) {
				baseState.RaiseEnterEvent();
				DetailedLog("Entered base state on Awake");
			}
		}
	}

	private void Start() {
		if (baseState != null && baseStateEnterInvocation == BaseStateEnterInvocation.OnStart) {
			baseState.RaiseEnterEvent();
			DetailedLog("Entered base state on Start");
		}
	}

	/// <summary>
	/// Moves to a new state if the previous state has it as a child state.
	/// </summary>
	/// <param name="state">The state to move to.</param>
	/// <returns>Returns true if move was successful.</returns>
	public bool MoveTo(State state) {
		if (CanMoveTo(state, true)) {
			PreviousState = CurrentState;
			path.Add(state);

			TryClearPathIfBase(state);

			PreviousState.RaiseLeftToChildEvent();
			CurrentState.RaiseEnterEvent();

			SwitchedStates.Invoke(PreviousState, CurrentState);

			Log($"Moved from {PreviousState} to {CurrentState}");
			return true;
		}

		DetailedLog($"Could not move to {state} from {CurrentState}");
		return false;
	}

	/// <summary>
	/// Leaves the state if it's the nav's current state and returns to the previous state.
	/// </summary>
	/// <param name="state">The state to leave.</param>
	/// <returns>Returns true if leaving was successful.</returns>
	public bool Leave(State state) {
		if (CanLeave(state, true)) {
			PreviousState = CurrentState;

			path.RemoveAt(path.Count - 1);

			TryClearPathIfBase(state);

			PreviousState.RaiseLeftToParentEvent();
			CurrentState.RaiseEnterEvent();

			SwitchedStates.Invoke(PreviousState, CurrentState);

			Log($"Left {PreviousState} to fall back to {CurrentState}");
			return true;
		}

		DetailedLog($"Could not leave {state} from {CurrentState}");
		return false;
	}

	/// <summary>
	/// Switches to a new state if it is also the child of the current state's parent.
	/// </summary>
	/// <param name="state">The state to switch to.</param>
	/// <returns>Returns true if switching was successful.</returns>
	public bool SwitchTo(State state) {
		if (CanSwitchTo(state, true)) {
			PreviousState = CurrentState;
			path[path.Count - 1] = state;

			TryClearPathIfBase(state);

			PreviousState.RaiseLeftToSiblingEvent();
			CurrentState.RaiseEnterEvent();

			SwitchedStates.Invoke(PreviousState, CurrentState);

			Log($"Switched from {PreviousState} to {CurrentState}");
			return true;
		}

		DetailedLog($"Could not switch to {state} from {CurrentState}");
		return false;
	}

	public bool CanMoveTo(State state) => CanMoveTo(state, false);

	public bool CanLeave(State state) => CanLeave(state, false);

	public bool CanSwitchTo(State state) => CanSwitchTo(state, false);

	private bool CanMoveTo(State state, bool throwExceptions) {
		if (CurrentState == null) {
			if (throwExceptions)
				throw new NullReferenceException("Can't move to a new state from a null state!");
			return false;
		}

		if (throwExceptions && state == null)
			throw new System.NullReferenceException("Can't move to a null state dingus!! >:0");
		return CurrentState.childStates.Contains(state) || fromAnyState.Contains(state);
	}

	private bool CanLeave(State state, bool throwExceptions) {
		if (throwExceptions) {
			if (path[path.Count - 2] == null)
				throw new NullReferenceException("Can't leave to a null parent state!");
			if (state == null)
				throw new NullReferenceException("Can't leave a null state dingus!! >:0");
			if (path.Count == 1 && state == CurrentState)
				throw new Exception("You can't leave the base state!");
		}
		return state == CurrentState && path.Count != 1;
	}

	private bool CanSwitchTo(State state, bool throwExceptions) {
		State parentState = path[path.Count - 2];

		if (throwExceptions) {
			if (parentState == null)
				throw new NullReferenceException("Can't switch from a null parent state!");
			if (state == null)
				throw new NullReferenceException("Can't switch from a null state dingus!! >:0");
			if (path.Count == 1)
				throw new Exception("You can't switch out of the base state! Try just regular ol' MoveTo()");
		}
		return parentState.childStates.Contains(state);
	}

	public bool InState(State state) => CurrentState == state;

	/// <summary>
	/// Returns true if the parent state is in the path.
	/// </summary>
	/// <param name="parentState">The state to check in the path.</param>
	/// <param name="includeCurrentState">Does the current state count?</param>
	public bool IsInChildStateOf(State parentState, bool includeCurrentState = false) {
		for (int i = path.Count - (includeCurrentState ? 1 : 2); i >= 0; i--) {
			if (path[i] == parentState)
				return true;
		}
		return false;
	}

	/// <summary>
	/// Clears the path if currently in the base state.
	/// </summary>
	/// <returns>Returns true if cleared.</returns>
	public bool ClearPath() {
		if (CurrentState == baseState) {
			path.Clear();
			path.Add(baseState);

			DetailedLog("Path cleared");
			return true;
		}
		DetailedLog($"Path not cleared as current state ({CurrentState}) is not the base state ({BaseState})");
		return false;
	}

	private void TryClearPathIfBase(State newState) {
		if (clearPathOnEnterBaseState && newState == baseState)
			ClearPath();
	}

	/// <summary>
	/// Returns the current path from the base state to the current state.
	/// </summary>
	public string GetCurrentPath() {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < path.Count; i++)
			sb.Append($" > {path[i].name}");
		return sb.ToString().Trim();
	}

	/// <summary>
	/// Gets a copy of the path using ToArray().
	/// </summary>
	public State[] GetCopyOfPath() {
		return path.ToArray();
	}

	/// <summary>
	/// Initializes the nav with a path of n > 0 states.
	/// The state at index 0 will become the base state, and the state at index n - 1 will become the current state.
	/// </summary>
	/// <param name="path">The path to initialize the nav with.</param>
	/// <param name="raiseCurrentStateEnter">Should the current state's Entered event be raised after initialization?</param>
	public void InitializeWithPath(State[] path, bool raiseCurrentStateEnter) {
		if (path == null)
			throw new ArgumentNullException("Provided path was null");
		if (path.Length == 0)
			throw new System.Exception("Path must have at least 1 state for the base state!");

		baseState = path[0];
		this.path = new List<State>(path);

		DetailedLog($"Initialized nav with new path of {path.Length} state{(path.Length == 1 ? "" : "s")}");

		if (raiseCurrentStateEnter)
			CurrentState.RaiseEnterEvent();
	}

	/// <summary>
	/// Initializes the nav with a base state.
	/// Note that if the nav is already initialized, this will replace the base state and not throw any left events on the current state.
	/// </summary>
	/// <param name="baseState">The state to use as the base state for the nav.</param>
	/// <param name="raiseCurrentStateEnter">Should the base state's Entered event be raised after initialization?</param>
	public void InitializeWithState(State baseState, bool raiseCurrentStateEnter) {
		if (path == null)
			throw new ArgumentNullException("Base state cannot be initialized with null");

		this.baseState = baseState;

		path.Clear();
		path.Add(baseState);

		DetailedLog($"Initialized nav with base state of {baseState.name}");

		if (raiseCurrentStateEnter)
			CurrentState.RaiseEnterEvent();
	}

	private void Log(string message) {
		if (logDetail != LogDetail.DontLog)
			Debug.Log($"{message}\nState Path: {GetCurrentPath()}");
	}

	private void DetailedLog(string message) {
		if (logDetail == LogDetail.All)
			Debug.Log($"{message}\nState path: {GetCurrentPath()}");
	}

	public static implicit operator State(StateNav stateTree) => stateTree.CurrentState;

	#if UNITY_EDITOR
	private Texture2D backgroundTexture;

	private void OnGUI() {
		if (!drawPath)
			return;

		if (backgroundTexture == null) {
			backgroundTexture = new Texture2D(1, 1);
			backgroundTexture.SetPixel(0, 0, Color.black);
			backgroundTexture.Apply();
		}

		GUIStyle style = new GUIStyle();
		style.normal.background = backgroundTexture;
		style.normal.textColor = new Color(.8f, .7f, 0);
		style.padding = new RectOffset(10, 10, 10, 10);
		style.fontSize = 35;

		GUILayout.Box(GetCurrentPath(), style);
	}
	#endif
}
