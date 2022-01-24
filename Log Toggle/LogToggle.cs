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

using UnityEngine;

[System.Serializable]
public struct LogToggle {

	[SerializeField]
	private bool show;

	public LogToggle(bool show) {
		this.show = show;
	}

	public void Log(object message) => Log(message, null);
	public void Log(object message, Object context) {
		if (show)
			Debug.Log(message, context);
	}

	public void LogWarning(object message) => LogWarning(message, null);
	public void LogWarning(object message, Object context) {
		if (show)
			Debug.LogWarning(message, context);
	}

	public void LogError(string message) => LogError(message, null);
	public void LogError(object message, Object context) {
		if (show)
			Debug.LogError(message, context);
	}

	public static implicit operator bool(LogToggle tl) => tl.show;

	public static implicit operator LogToggle(bool value) => new LogToggle(value);
}