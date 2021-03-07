using UnityEngine;

public class RndVal {

	private int[] list;
	private int offset;
	private int shuffleCount;
	private int totalCount;

	private int lastIndex;
	public int LastValue {
		get => lastIndex + offset;
	}

	public RndVal(int min, int max, int shuffleCount) {

		if (min > max)
			(min, max) = (max, min);
		else if (min == max)
			throw new System.ArgumentOutOfRangeException();

		this.shuffleCount = shuffleCount;

		offset = min;
		list = new int[Mathf.Abs(max - min)];

		Next();
	}

	public int Next() {
		if (totalCount == shuffleCount * list.Length)
			ResetList();

		int index;
		do {
			index = Mathf.Clamp(Mathf.RoundToInt(Random.value * list.Length - 1), 0, list.Length - 1);
		} while (list[index] == shuffleCount || index == lastIndex);
		totalCount++;
		list[index]++;
		lastIndex = index;

		return index + offset;
	}

	private void ResetList() {
		totalCount = 0;
		for (int i = 0; i < list.Length; i++)
			list[i] = 0;
	}

	public static implicit operator int(RndVal rnd) => rnd.LastValue;
}