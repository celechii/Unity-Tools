using System;
using UnityEngine;

// derived from https://www.subsystems.us/uploads/9/8/9/4/98948044/moonphase.pdf initially
// mostly from https://www.timeanddate.com/moon/phases/

public abstract class MoonCalculator : MonoBehaviour {

	public static float MoonCycle { get { return (float) 25101 / 850; } }

	public static float GetPercent() {
		DateTime calibrationDate = new DateTime(2019, 3, 6, 11, 3, 0); // mar 6, 2019 @ 11:03am
		double daysSince = (DateTime.Today - calibrationDate).TotalDays;
		return (float) daysSince / MoonCycle;
	}

	public static float DaysIntoCycle() {
		return GetPercent() * MoonCycle;
	}

	public static MoonPhase GetPhase() {
		float percent = GetPercent();
		int numPhases = Enum.GetValues(typeof(MoonPhase)).Length;
		int closest = Mathf.RoundToInt(percent * (numPhases + 1));
		return (MoonPhase) (closest % numPhases);
	}

	public enum MoonPhase {
		New,
		WaxingCrescent,
		FirstQuarter,
		WaxingGibbous,
		Full,
		WaningGibbous,
		ThirdQuarter,
		WaningCrescent
	}

}
