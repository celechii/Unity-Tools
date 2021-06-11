using System;
using System.Collections.Generic;

public static class PronounSystem {
	public static PronounData[] customPronouns = new PronounData[3];
	private static PronounData theyThem = new PronounData("they/them/their/theirs/themself", true);
	private static PronounData sheHer = new PronounData("she/her/her/hers/herself", false);
	private static PronounData heHim = new PronounData("he/him/his/his/himself", false);
	private static PronounData zeZir = new PronounData("ze/zir/zir/zirs/zirself", false);
	private static PronounData zeHir = new PronounData("ze/hir/hir/hirs/hirself", false);
	private static PronounData itIt = new PronounData("it/it/it/its/itself", false);
	private static PronounData xeyXem = new PronounData("xey/xem/xyr/xyrs/xemself", true);
	private static PronounData eyEm = new PronounData("ey/em/eir/eirs/eirself", true);
	private static PronounData nonePronounsLeftBeef = new PronounData("_____");

	public static string GetShortForm(this Pronoun pronouns) {
		if ((int)pronouns == 0)
			return "No pronouns";
		if (Count(pronouns) <= 1)
			return $"{GetPronounData(pronouns).they}/{GetPronounData(pronouns).them}";

		Pronoun[] pronounArray = GetPronounArray(pronouns);
		string compilarion = GetThey(pronounArray[0]);
		for (int i = 1; i < pronounArray.Length; i++)
			compilarion += "/" + GetThey(pronounArray[i]);
		return compilarion;
	}

	public static string GetThey(this Pronoun pronoun) => GetPronounData(pronoun).they;
	public static string GetThem(this Pronoun pronoun) => GetPronounData(pronoun).them;
	public static string GetTheir(this Pronoun pronoun) => GetPronounData(pronoun).their;
	public static string GetTheirs(this Pronoun pronoun) => GetPronounData(pronoun).theirs;
	public static string GetThemself(this Pronoun pronoun) => GetPronounData(pronoun).themself;
	public static string GetTheyre(this Pronoun pronoun) => GetPronounData(pronoun).they + (GetPronounData(pronoun).pluralize? "\'re": "\'s");
	public static string GetTheyve(this Pronoun pronoun) => GetPronounData(pronoun).they + (GetPronounData(pronoun).pluralize? "\'ve": "\'s");
	public static string GetTheyd(this Pronoun pronoun) => GetPronounData(pronoun).they + "\'d";
	public static string GetTheyll(this Pronoun pronoun) => GetPronounData(pronoun).they + "\'ll";
	public static string GetTheyHave(this Pronoun pronoun) => GetPronounData(pronoun).they + (GetPronounData(pronoun).pluralize? " have": "has");

	public static Pronoun GetRandomPronoun(this Pronoun pronouns) {
		Pronoun[] avaliable = GetPronounArray(pronouns);
		if (avaliable.Length == 0)
			return (Pronoun)0;
		if (avaliable.Length == 1)
			return avaliable[0];
		return avaliable[UnityEngine.Random.Range(0, avaliable.Length)];
	}

	private static PronounData GetPronounData(Pronoun pronoun) {
		switch (pronoun) {
			case Pronoun.TheyThem:
				return theyThem;
			case Pronoun.SheHer:
				return sheHer;
			case Pronoun.HeHim:
				return heHim;
			case Pronoun.ZeZir:
				return zeZir;
			case Pronoun.ZeHir:
				return zeHir;
			case Pronoun.ItIts:
				return itIt;
			case Pronoun.XeyXem:
				return xeyXem;
			case Pronoun.EyEm:
				return eyEm;
			case Pronoun.Custom1:
				return customPronouns[0];
			case Pronoun.Custom2:
				return customPronouns[1];
			case Pronoun.Custom3:
				return customPronouns[2];
			default:
				return nonePronounsLeftBeef;
		}
	}

	private static int Count(Pronoun pronoun) {
		int bits = System.Convert.ToInt32(pronoun);
		int count = 0;
		for (int i = 0; i < 32; i++)
			if ((bits & (1 << i)) > 0)
				count++;
		return count;
	}

	private static Pronoun[] GetPronounArray(Pronoun pronuons) {
		int enumSize = Enum.GetValues(typeof(Pronoun)).Length;
		List<Pronoun> flags = new List<Pronoun>();

		Pronoun[] allFlags = (Pronoun[])Enum.GetValues(typeof(Pronoun));
		for (int i = 0; i < allFlags.Length; i++) {
			if (pronuons.HasFlag((Pronoun)allFlags.GetValue(i)))
				flags.Add((Pronoun)allFlags.GetValue(i));
		}
		return flags.ToArray();
	}
}

[System.Flags]
public enum Pronoun {
	TheyThem = 1 << 0,
	SheHer = 1 << 1,
	HeHim = 1 << 2,
	ZeZir = 1 << 3,
	ZeHir = 1 << 4,
	ItIts = 1 << 5,
	XeyXem = 1 << 6,
	EyEm = 1 << 7,
	Custom1 = 1 << 8,
	Custom2 = 1 << 9,
	Custom3 = 1 << 10
}

public class PronounData {
	public string they;
	public string them;
	public string their;
	public string theirs;
	public string themself;
	public bool pluralize;
	public bool noPronouns;
	public string name;

	// they put some bait on their line
	// the fish looked at them
	// they realized the fish wasnt theirs
	// they laughed to themself

	/// <param name="name">The name to use in place of any pronouns.</param>
	public PronounData(string name) {
		noPronouns = true;
		pluralize = false;
		they = them = themself = this.name = name;
		their = theirs = name + "\'s";
	}

	/// <param name="they">Subjective pronoun</param>
	/// <param name="them">Objective pronoun</param>
	/// <param name="their">Possessive adjective</param>
	/// <param name="theirs">Possessive pronoun</param>
	/// <param name="themselves">Reflecive pronoun</param>
	/// <param name="pluralize">Would it be "themself" or "themselves"?</param>
	public PronounData(string they, string them, string their, string theirs, string themselves, bool pluralize) {
		this.they = they;
		this.them = them;
		this.their = their;
		this.theirs = theirs;
		this.themself = themselves;
		this.pluralize = pluralize;
	}

	/// <param name="singleString">A single string in the form of "they/them/their/theirs/themself".</param>
	/// <param name="pluralize">Would it be "themself" or "themselves"?</param>
	public PronounData(string singleString, bool pluralize) {
		string[] segs = singleString.Split('/');
		if (segs.Length != 5)
			throw new System.Exception("u dont have enough shit for the pronouns!!");
		they = segs[0];
		them = segs[1];
		their = segs[2];
		theirs = segs[3];
		themself = segs[4];
		this.pluralize = pluralize;
	}
}