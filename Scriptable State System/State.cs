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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "States/New State")]
public class State : ScriptableObject {

	[Tooltip("States that this state can transition to.")]
	public List<State> childStates = new List<State>();

	public event Action Entered;
	public event Action Left;
	public event Action LeftToParent;
	public event Action LeftToSibling;
	public event Action LeftToChild;

	public void RaiseEnterEvent() {
		OnEnterEventRaised();
		Entered?.Invoke();
	}

	public void RaiseLeftToParentEvent() {
		OnLeftToParentEventRaised();
		LeftToParent?.Invoke();

		OnLeftEventRaised();
		Left?.Invoke();
	}

	public void RaiseLeftToSiblingEvent() {
		OnLeftToSiblingEventRaised();
		LeftToSibling?.Invoke();

		OnLeftEventRaised();
		Left?.Invoke();
	}

	public void RaiseLeftToChildEvent() {
		OnLeftToChildEventRaised();
		LeftToChild?.Invoke();

		OnLeftEventRaised();
		Left?.Invoke();
	}

	protected virtual void OnEnterEventRaised() {}
	protected virtual void OnLeftEventRaised() {}
	protected virtual void OnLeftToParentEventRaised() {}
	protected virtual void OnLeftToSiblingEventRaised() {}
	protected virtual void OnLeftToChildEventRaised() {}

	public void PrintEventSubscribers() {
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append($"{name} State Events:\n");
		Delegate[] subscribers = Entered.GetInvocationList();
		AddSubscribers("Enter state", Entered);
		AddSubscribers("Exit state", Left);

		Debug.Log(stringBuilder.ToString());

		void AddSubscribers(string title, Action action) {
			stringBuilder.Append($"{title}: [");
			for (int iSub = 0; iSub < subscribers.Length; ++iSub) {
				stringBuilder.Append(subscribers[iSub] + (iSub == subscribers.Length - 1 ? "]\n" : ", "));
			}
		}
	}

	#if UNITY_EDITOR

	[ContextMenu("Create New Child State")]
	private void CreateNewChildState() {
		State newState = ScriptableObject.CreateInstance<State>();
		string path = AssetDatabase.GetAssetPath(GetInstanceID());
		path = AssetDatabase.GenerateUniqueAssetPath(path.Substring(0, path.LastIndexOf('/') + 1) + "New Game State.asset");
		AssetDatabase.CreateAsset(newState, path);
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = newState;

		for (int i = 0; i < childStates.Count; i++) {
			if (childStates[i] == null) {
				childStates[i] = newState;
				return;
			}
		}
		childStates.Add(newState);
	}

	#endif
}