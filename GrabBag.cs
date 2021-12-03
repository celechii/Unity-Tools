/*
MIT License
Copyright (c) 2021 Noé Charron
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

public class GrabBag {

	private int[] indecesPulled;
	private int numListInstances;
	private Random rnd;
	private int pullsLeft;

	public GrabBag(int listLength) : this(listLength, 1) {}

	public GrabBag(int listLength, int numListInstances) : this(listLength, numListInstances, new Random()) {}

	public GrabBag(int listLength, int numListInstances, int randomSeed) : this(listLength, numListInstances, new Random(randomSeed)) {}

	private GrabBag(int listLength, int numListInstances, Random randomInstance) {
		if (numListInstances < 1)
			throw new ArgumentException($"{nameof(numListInstances)} must be greater than or equal to 1");
		if (listLength < 1)
			throw new ArgumentException($"{nameof(listLength)} must be greater than or equal to 1");

		indecesPulled = new int[listLength];
		this.numListInstances = numListInstances;
		rnd = randomInstance;

		Reset();
	}

	public int Pull() {
		if (pullsLeft == 0)
			Reset();

		int nextIndex;
		do {
			nextIndex = rnd.Next(indecesPulled.Length);
		} while (indecesPulled[nextIndex] == numListInstances);

		indecesPulled[nextIndex]++;
		pullsLeft--;

		return nextIndex;
	}

	private void Reset() {
		pullsLeft = indecesPulled.Length * numListInstances;
		for (int i = 0; i < indecesPulled.Length; i++)
			indecesPulled[i] = 0;
	}
}

public class GrabBag<T> : GrabBag {
	private T[] list;

	public GrabBag(T[] list) : this(list, 1) {}

	public GrabBag(T[] list, int numListInstances) : base(list.Length, numListInstances) {
		this.list = list;
	}

	public GrabBag(T[] list, int numListInstances, int randomSeed) : base(list.Length, numListInstances, randomSeed) {
		this.list = list;
	}

	public new T Pull() => list[base.Pull()];
}