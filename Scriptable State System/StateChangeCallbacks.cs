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

using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class StateChangeCallbacks : MonoBehaviour {

	[SerializeField]
	private StateCallback[] callbacks;

	private void Awake() {
		for (int i = 0; i < callbacks.Length; i++)
			callbacks[i].RegisterEvents();
	}

	private void OnDestroy() {
		for (int i = 0; i < callbacks.Length; i++)
			callbacks[i].UnregisterEvents();
	}

	[System.Serializable]
	private struct StateCallback : ISerializationCallbackReceiver {

		[HideInInspector]
		[SerializeField]
		private string name;

		public State state;

		public UnityEvent onEntered;

		public UnityEvent onLeft;

		public void RegisterEvents() {
			state.Entered += InvokeEntered;
			state.Left += InvokeLeft;
		}

		public void UnregisterEvents() {
			state.Entered -= InvokeEntered;
			state.Left -= InvokeLeft;
		}

		private void InvokeEntered() => onEntered.Invoke();

		private void InvokeLeft() => onLeft.Invoke();

		public void OnBeforeSerialize() {
			string name = state == null ? "Nothing" : state.name;
			int enterCount = onEntered.GetPersistentEventCount();
			int exitCount = onLeft.GetPersistentEventCount();

			if (enterCount + exitCount > 0) {
				StringBuilder sb = new StringBuilder("On ");

				if (enterCount > 0 && exitCount > 0)
					sb.Append("Enter/Exit ");
				else if (enterCount > 0)
					sb.Append("Enter ");
				else if (exitCount > 0)
					sb.Append("Exit ");
				sb.Append(state.name);

				this.name = sb.ToString();
			}

			this.name = name;
		}

		public void OnAfterDeserialize() {
			// do nothing
		}
	}

}