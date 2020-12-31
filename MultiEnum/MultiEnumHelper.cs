using System;
using System.Collections.Generic;

public static class MultiEnumHelper {

	/// <summary>
	/// Does the enum contain the flag?
	/// </summary>
	public static bool Has<TEnum>(this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		return (Convert.ToInt32(multiEnum) & Convert.ToInt32(flag)) > 0;
	}

	/// <summary>
	/// Does this enum have each flag?
	/// </summary>
	public static bool HasNone<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible => Convert.ToInt32(multiEnum) == 0;

	/// <summary>
	/// Does this enum have each flag?
	/// </summary>
	public static bool HasAll<TEnum>(this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		return multiEnum.Has<TEnum>(flag);
	}

	/// <summary>
	/// Does this enum have each of the flags set?
	/// </summary>
	public static bool HasAll<TEnum>(this TEnum multiEnum, params TEnum[] flags)where TEnum : struct, IConvertible {
		foreach (TEnum flag in flags)
			if (!multiEnum.Has(flag))
				return false;
		return true;
	}

	/// <summary>
	/// Does this enum have every flag set?
	/// </summary>
	public static bool HasAll<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		foreach (TEnum flag in Enum.GetValues(typeof(TEnum)))
			if (!multiEnum.Has(flag))
				return false;
		return true;
	}

	/// <summary>
	/// Gets a combined enum with all the specified flags set
	/// </summary>
	public static TEnum Combine<TEnum>(params TEnum[] flags)where TEnum : struct, IConvertible {
		if (flags.Length == 0)
			throw new ArgumentOutOfRangeException();
		if (flags == null)
			throw new ArgumentNullException();

		TEnum multiEnum = flags[0];
		for (int i = 1; i < flags.Length; i++)
			multiEnum.Add(flags[i]);
		return multiEnum;
	}

	/// <summary>
	/// Get an array of the individual flags.
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
	/// Gets the number of flags that are currently set.
	/// </summary>
	public static int Count<TEnum>(this TEnum multiEnum)where TEnum : struct, IConvertible {
		int bits = Convert.ToInt32(multiEnum);
		int count = 0;
		for (int i = 0; i < 32; i++)
			if ((bits & (1 << i)) > 0)
				count++;
		return count;
	}

	/// <summary>
	/// Add a flag to the enum.
	/// </summary>
	public static void Add<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int result = Convert.ToInt32(multiEnum) | Convert.ToInt32(flag);
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	/// <summary>
	/// Sets all the flags.
	/// </summary>
	public static void AddAll<TEnum>(ref this TEnum multiEnum)where TEnum : struct, IConvertible {
		int value = 0;
		int enumSize = Enum.GetValues(typeof(TEnum)).Length;
		for (int i = 0; i < enumSize; i++)
			value += 1 << i;

		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), value);
	}

	/// <summary>
	/// Removes the flag on the enum if it has it.
	/// </summary>
	public static void Remove<TEnum>(ref this TEnum multiEnum, TEnum flag)where TEnum : struct, IConvertible {
		int result = Convert.ToInt32(multiEnum) & ~Convert.ToInt32(flag);
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), result);
	}

	/// <summary>
	/// Removes all the flags on the enum.
	/// </summary>
	public static void RemoveAll<TEnum>(ref this TEnum multiEnum)where TEnum : struct, IConvertible {
		multiEnum = (TEnum)Enum.ToObject(typeof(TEnum), 0);
	}

	/// <summary>
	/// Either sets or removes all the flags on the enum.
	/// </summary>
	public static void SetAll<TEnum>(ref this TEnum multiEnum, bool on)where TEnum : struct, IConvertible {
		if (on)
			multiEnum.AddAll();
		else
			multiEnum.RemoveAll();
	}

	/// <summary>
	/// Flip a flag on the enum.
	/// </summary>
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