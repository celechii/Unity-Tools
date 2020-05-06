using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {

	private static string path;

	private static void SetPath() {
		path = Application.dataPath + "/Resources/Saves";
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
	}

	/// <summary>
	/// Checks if the save exists.
	/// </summary>
	/// <param name="fileName">The file name, INCLUDING the extension, even for txts.</param>
	public static bool SaveExists(string fileName) {
		SetPath();
		return File.Exists(path + "/" + fileName);
	}

	/// <summary>
	/// Saves to text file in the resources folder.
	/// </summary>
	/// <param name="fileName">File name, not including the extension. Saves as a .txt</param>
	/// <param name="data">JSON data string.</param>
	public static void SaveTxt(string fileName, object data) {
		SetPath();
		string stringData = JsonUtility.ToJson(data, true);
		StreamWriter writer = new StreamWriter(path + "/" + fileName + ".txt");
		writer.Write(stringData);
		writer.Close();
	}

	/// <summary>
	/// Loads a text from text file.
	/// </summary>
	/// <returns>The from text file.</returns>
	/// <param name="fileName">File name excluding the extention.</param>
	public static T LoadTxt<T>(string fileName) {
		SetPath();
		StreamReader reader = new StreamReader(path + "/" + fileName + ".txt");
		string stringData = reader.ReadToEnd();
		reader.Close();
		return JsonUtility.FromJson<T>(stringData);
	}

	/// <summary>
	/// Handles the actual saving of objects to a file.
	/// </summary>
	/// <param name="fileName">Name for the file (including the extension) to be saved under, and later searched for.</param>
	/// <param name="data">A JSON string for the object.</param>
	public static void SaveBin(string fileName, object data) {
		SetPath();
		string stringData = JsonUtility.ToJson(data);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(path + "/" + fileName);
		binaryFormatter.Serialize(file, stringData);
		file.Close();
	}

	/// <summary>
	/// Loads a JSON string from a binary file.
	/// </summary>
	/// <returns>The JSON string contained in the specified file.</returns>
	/// <param name="fileName">Name of the file (including the extension) to be opened.</param>
	public static T LoadBin<T>(string fileName) {
		SetPath();
		string fullPath = path + "/" + fileName;

		if (!File.Exists(fullPath))
			throw new Exception("wtf? " + fullPath + " doesn't exist???");

		FileStream file = File.Open(fullPath, FileMode.Open);
		BinaryFormatter binaryFormatter = new BinaryFormatter();

		//json string to return after being deserialized
		string stringData = (string)binaryFormatter.Deserialize(file);
		file.Close();

		return JsonUtility.FromJson<T>(stringData);
	}
}