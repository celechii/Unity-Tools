using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour {

	public NEvent saveFunction;
	public NEvent loadFunction;

	private static string path;

	private void Awake() {
		SetPath();
	}

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
		string stringData = JsonUtility.ToJson(data);
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
		string stringData = Encrypt(JsonUtility.ToJson(data));
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
		string stringData = Decrypt((string) binaryFormatter.Deserialize(file));
		file.Close();

		return JsonUtility.FromJson<T>(stringData);
	}

	/// <summary>
	/// Tell all GameObjects with a SaveSystem component to save.
	/// </summary>
	public static void SaveGame() {
		SaveSystem[] saveSystems = FindObjectsOfType<SaveSystem>();
		string saveNames = "";
		foreach (SaveSystem s in saveSystems) {
			s.saveFunction.Invoke();
			if (saveNames != "")
				saveNames += ", ";
			saveNames += s.name;
		}
	}

	/// <summary>
	/// Tell all GameObjects with a SaveSystem component to load.
	/// </summary>
	public static void LoadGame() {
		SaveSystem[] saveSystems = FindObjectsOfType<SaveSystem>();
		string saveNames = "";
		foreach (SaveSystem s in saveSystems) {
			s.loadFunction.Invoke();
			if (saveNames != "")
				saveNames += ", ";
			saveNames += s.name;
		}
	}

	[ContextMenu("Invoke Load Function")]
	private void InvokeLoad() {
		loadFunction.Invoke();
	}

	[ContextMenu("Invoke Save Function")]
	private void InvokeSaveFunction() {
		saveFunction.Invoke();
	}

	private static string Encrypt(string s) {

		string ns = "";
		for (int i = s.Length - 1; i >= 0; i--)
			ns += (char) (s[i] + 3);
		s = ns;

		string nns = "";
		for (int i = 0; i < s.Length; i++)
			nns += (char) (s[i] + (i % 2 == 0 ? 13 : 7));
		s = nns;
		return s;
	}

	private static string Decrypt(string s) {
		string ns = "";
		for (int i = 0; i < s.Length; i++)
			ns += (char) (s[i] - (i % 2 == 0 ? 13 : 7));
		s = ns;

		string nns = "";
		for (int i = s.Length - 1; i >= 0; i--)
			nns += (char) (s[i] - 3);

		return nns;
	}
}
