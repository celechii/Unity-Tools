using UnityEngine;

public class Randomizer {

	public static int GetRandomIndex(float[] probabilities) => GetRandomIndex(probabilities, Random.value);

	public static int GetRandomIndex(float[] probabilities, float randomVal) {
		if (probabilities.Length == 0)
			throw new System.Exception("hey fucker wanna give me literally anything to choose from");

		float scalar = 0;
		for (int i = 0; i < probabilities.Length; i++)
			scalar += probabilities[i];

		for (int i = 0; i < probabilities.Length; i++) {
			if (randomVal <= probabilities[i] / scalar)
				return i;
			else
				randomVal -= probabilities[i] / scalar;
		}
		throw new System.Exception("oh uh fuck, i couldnt pick an index for some reason oops");
	}
}