public struct MultiBool {

	private int count;
	private bool hasMin;
	private bool hasMax;
	private int max;
	private int min;
	private bool defaultValue;

	public MultiBool(bool defaultValue) : this(defaultValue, false, 0, false, 0) {}
	public MultiBool(bool defaultValue, bool hasMin, int min, bool hasMax, int max) {
		count = 0;
		this.defaultValue = defaultValue;
		this.hasMin = hasMin;
		this.min = min;
		this.hasMax = hasMax;
		this.max = max;
	}

	public static MultiBool WithMin(int min) => WithMin(min, true);
	public static MultiBool WithMin(int min, bool defaultValue) {
		MultiBool b = new MultiBool(defaultValue);
		b.hasMin = true;
		b.min = min;
		return b;
	}

	public static MultiBool WithMax(int max) => WithMax(max, true);
	public static MultiBool WithMax(int max, bool defaultValue) {
		MultiBool b = new MultiBool(defaultValue);
		b.hasMax = true;
		b.max = max;
		return b;
	}

	public static MultiBool WithMinAndMax(int min, int max) => WithMinAndMax(min, max, true);
	public static MultiBool WithMinAndMax(int min, int max, bool defaultValue) {
		MultiBool b = new MultiBool(defaultValue);
		b.hasMin = true;
		b.hasMax = true;
		if (min <= max) {
			b.min = min;
			b.max = max;
		} else {
			b.min = max;
			b.max = min;
		}
		return b;
	}

	public void Set(int amount) {
		count = amount;
		ClampCount();
	}

	public void Reset() {
		count = 0;
		ClampCount();
	}

	public void AddTrue() {
		count++;
		ClampCount();
	}

	public void AddFalse() {
		count--;
		ClampCount();
	}

	public void Add(bool value) {
		Add(value ? 1 : -1);
	}

	public void Add(int amount) {
		count += amount;
		ClampCount();
	}

	private void ClampCount() {
		if (hasMin && count < min)
			count = min;
		else if (hasMax && count > max)
			count = max;
	}

	public static implicit operator bool(MultiBool b) => b.count > 0 || b.defaultValue && b.count == 0;
	public static implicit operator MultiBool(bool b) => new MultiBool(b);

	public static MultiBool operator ++(MultiBool b) {
		b.AddTrue();
		return b;
	}

	public static MultiBool operator --(MultiBool b) {
		b.AddFalse();
		return b;
	}
}