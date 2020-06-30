using System;
using System.Collections.Generic;

public static class MultiEnumHelper {

	public static bool Has<TEnum>(this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int flagVal = Convert.ToInt32(flag);
		if (flagVal == 0)
			throw new ArgumentException("hey wtf don't pass in a flag with a value of 0 >:(");

		return (Convert.ToInt32(multiEnum) & flagVal) > 0;
	}

	public static bool HasNone<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible => Convert.ToInt32(multiEnum) == 0;

	public static bool HasAll<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		foreach (TEnum flag in Enum.GetValues(typeof(TEnum)))
			if (!multiEnum.Has(flag))
				return false;
		return true;
	}

	/// <summary>
	/// sets all the flags
	/// </summary>
	public static void SetAll<TEnum>(ref this TEnum multiEnum)where TEnum : struct, IConvertible {
		int value = 0;
		int enumSize = Enum.GetValues(typeof(TEnum)).Length;
		for (int i = 0; i < enumSize; i++)
			value += 1 << i;

		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), value);
	}

	/// <summary>
	/// get an array of the individual flags
	/// </summary>
	public static TEnum[] GetArray<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		int enumSize = Enum.GetValues(typeof(TEnum)).Length;
		List<TEnum> flags = new List<TEnum>();

		TEnum[] allFlags = (TEnum[])Enum.GetValues(typeof(TEnum));
		for (int i = 0; i < allFlags.Length; i++) {
			if (multiEnum.Has((TEnum)allFlags.GetValue(i)))
				flags.Add((TEnum)allFlags.GetValue(i));
		}
		return flags.ToArray();
	}

	/// <summary>
	/// gets the number of flags that r currently set
	/// </summary>
	public static int GetNumFlags<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		int enumSize = Enum.GetValues(typeof(TEnum)).Length;
		int bits = Convert.ToInt32(multiEnum);

		int count = 0;
		for (int i = 0; i < enumSize; i++)
			if ((bits & (1 << i)) > 0)
				count++;

		return count;
	}

	public static void Add<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int result = Convert.ToInt32(multiEnum) | Convert.ToInt32(flag);
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	public static void Remove<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int result = Convert.ToInt32(multiEnum) & ~Convert.ToInt32(flag);
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	public static void RemoveAll<TEnum>(ref this TEnum multiEnum)where TEnum : struct, IConvertible {
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), 0);
	}

	public static void Toggle<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int result = Convert.ToInt32(multiEnum) & Convert.ToInt32(flag);
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	public static string ToString<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		TEnum[] array = multiEnum.GetArray();
		if (array.Length == 0)
			return "";
		string s = array[0].ToString();
		for (int i = 1; i < array.Length; i++)
			s += ", " + array[i];
		return s;
	}
}