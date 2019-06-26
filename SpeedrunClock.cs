using UnityEngine;

public class SpeedrunClock : MonoBehaviour {

	public double time;

	private bool timeAdvance = true;

	private void Update() {
		if (timeAdvance)
			time += (double)Time.unscaledDeltaTime;
	}

	public void StopTimer() {
		timeAdvance = false;
	}

	public void ResumeTimer() {
		timeAdvance = true;
	}

	[ContextMenu("Print Time")]
	public string GetStringTime() {
		string ret = System.TimeSpan.FromSeconds(time).ToString("hh\\:mm\\:ss\\.fff");

		print(ret);
		return ret;
	}
}