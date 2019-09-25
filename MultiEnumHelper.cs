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
		foreach (TEnum flag in Enum.GetValues(typeof(TEnum))) {
			if (!multiEnum.Has(flag))
				return false;
		}
		return true;
	}

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

	public static void Add<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		if (!typeof(TEnum).IsEnum)
			throw new ArgumentException("uh wtf this isn't an enum???");

		int result = Convert.ToInt32(multiEnum) | Convert.ToInt32(flag);

		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	public static void Remove<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		if (!typeof(TEnum).IsEnum)
			throw new ArgumentException("uh wtf this isn't an enum???");

		int result = Convert.ToInt32(multiEnum) & ~Convert.ToInt32(flag);

		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	public static void Toggle<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		if (!typeof(TEnum).IsEnum)
			throw new ArgumentException("uh wtf this isn't an enum???");

		int result = Convert.ToInt32(multiEnum) & Convert.ToInt32(flag);

		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}
}