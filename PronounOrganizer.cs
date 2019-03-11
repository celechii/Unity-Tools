public static class PronounOrganizer {

	private static string[] pronounsNotOnList = { "they", "them", "their", "theirs", "themself" };


	/// <summary>
	/// input in the format of "they/them/their/theirs/themself"
	/// </summary>
	public static void SetOfflistPronouns(string pronounList) {
		string[] pronouns = pronounList.Split('/');

		for (int i = 0; i < System.Math.Min(pronouns.Length, pronounsNotOnList.Length); i++)
			pronounsNotOnList[i] = pronouns[i];
	}

	public static string GetPronouns(Pronoun pronoun) {
		if (pronoun == Pronoun.ItIts)
			return "It/Its";

		string they = GetThey(pronoun);
		they = they.Replace(they[0], (char) (they[0] - 32));

		string them = GetThem(pronoun);
		them = them.Replace(them[0], (char) (them[0] - 32));

		return they + "/" + them;
	}

	public static string GetThey(Pronoun pronoun) {
		switch ((int) pronoun) {
			case 0:
				return "they";
			case 1:
				return "she";
			case 2:
				return "he";
			case 3:
				return "xe";
			case 4:
				return "ze";
			case 5:
				return "ze";
			case 6:
				return "it";
			default:
				return pronounsNotOnList[0];
		}
	}

	public static string GetThem(Pronoun pronoun) {
		switch ((int) pronoun) {
			case 0:
				return "them";
			case 1:
				return "her";
			case 2:
				return "him";
			case 3:
				return "xem";
			case 4:
				return "zir";
			case 5:
				return "hir";
			case 6:
				return "it";
			default:
				return pronounsNotOnList[1];
		}
	}

	public static string GetTheir(Pronoun pronoun) {
		switch ((int) pronoun) {
			case 0:
				return "they";
			case 1:
				return "her";
			case 2:
				return "his";
			case 3:
				return "xyr";
			case 4:
				return "zir";
			case 5:
				return "hir";
			case 6:
				return "its";
			default:
				return pronounsNotOnList[2];
		}
	}

	public static string GetTheirs(Pronoun pronoun) {
		switch ((int) pronoun) {
			case 0:
				return "theirs";
			case 1:
				return "hers";
			case 2:
				return "his";
			case 3:
				return "xyrs";
			case 4:
				return "zirs";
			case 5:
				return "hirs";
			case 6:
				return "its";
			default:
				return pronounsNotOnList[3];
		}
	}

	public static string GetThemself(Pronoun pronoun) {
		switch ((int) pronoun) {
			case 0:
				return "themself";
			case 1:
				return "herself";
			case 2:
				return "himself";
			case 3:
				return "xemself";
			case 4:
				return "zirself";
			case 5:
				return "hirself";
			case 6:
				return "itself";
			default:
				return pronounsNotOnList[4];
		}
	}
}

public enum Pronoun {
	TheyThem = 0,
	SheHer = 1,
	HeHim = 2,
	XeXir = 3,
	ZeZir = 4,
	ZeHir = 5,
	ItIts = 6,
	NotOnList = 7
}