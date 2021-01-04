using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class SaveSystem {

	/*
	
	HEY FUTURE NOÉ READ THIS BEFORE U TRY TO ADD MORE SHIT TO THE SAVE FILE
	idk just some things to remember before u stress urself out trying to find why smth's not working
	
	- WorldControl calls Load on Start() n Save() on OnDestroy()
	- shit w Save/Load methods do the SaveSystem.savables.Add(this); BEFORE WorldControl calls Start()
	- the ICanBeSaved interface is for reading from n writing to SaveSystem.Data!!!
	- load is only called when there's a loadable save file so remember to set defaults!!
	- i love u
	
	adding new shit?? here r ur steps:
	1. add the data to b saved in the SaveData class
	2. add n implement the ICanBeSaved interface on the class u want to do the saving
	3. in the Load() of the ICanBeSaved, pull the data from SaveSystem.Data, sort it out
	4. in the Save() of the ICanBeSaved, compile the data into the structure u wanna save in then set it in SaveSystem.Data
	5. remember to account for when load DOESNT get called (when there is no save data)
	6. smile cause u fucked this up a lot but not this time :)
	
	*/

	public static int currentSaveSlot = 1;
	private static string path;
	private static string saveFileName = $"Save {currentSaveSlot}";

	private static SaveData data;
	public static SaveData Data {
		get {
			if (data == null)
				Load();
			return data;
		}
	}

	public static List<ICanBeSaved> savables = new List<ICanBeSaved>();

	private static void SetPath() {
		path = Path.Combine(Application.persistentDataPath, "Saves");
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Open Save Location")]
	private static void OpenSavePathInFinder() {
		SetPath();
		OpenInFileBrowser.Open(path);
	}
	#endif

	/// <summary>
	/// Checks if the save exists.
	/// </summary>
	/// <param name="fileName">The file name, INCLUDING the extension, even for txts.
	/// Include '#' to be replaced with the current save slot.</param>
	public static bool SaveExists(string fileName) {
		fileName = fileName.Replace("#", currentSaveSlot.ToString());
		SetPath();
		return File.Exists(Path.Combine(path, fileName));
	}

	/// <summary>
	/// Saves to text file in the resources folder.
	/// </summary>
	/// <param name="fileName">File name, not including the extension.
	/// Include '#' to be replaced with the current save slot.</param>
	/// <param name="data">JSON data string.</param>
	public static void SaveTxt(string fileName, object data) {
		fileName = fileName.Replace("#", currentSaveSlot.ToString());
		SetPath();
		string stringData = JsonUtility.ToJson(data, true);
		StreamWriter writer = new StreamWriter(Path.Combine(path, fileName + ".txt"));
		writer.Write(stringData);
		writer.Close();
	}

	/// <summary>
	/// Loads a text from text file.
	/// </summary>
	/// <returns>The from text file.</returns>
	/// <param name="fileName">File name excluding the extention.
	/// Include '#' to be replaced with the current save slot.</param>
	public static T LoadTxt<T>(string fileName) {
		fileName = fileName.Replace("#", currentSaveSlot.ToString());
		SetPath();
		StreamReader reader = new StreamReader(Path.Combine(path, fileName + ".txt"));
		string stringData = reader.ReadToEnd();
		reader.Close();
		return JsonUtility.FromJson<T>(stringData);
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Open/Current Save Game")]
	private static void OpenSave() => OpenFile("Save #.txt");

	public static void OpenFile(string fileName) {
		fileName = fileName.Replace("#", currentSaveSlot.ToString());
		SetPath();
		string fullPath = Path.Combine(path, fileName);
		if (File.Exists(fullPath))
			Process.Start(fullPath);
		else
			throw new System.Exception($"wtf {fullPath} isnt a thing???");
	}
	#endif

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Load/Game Save")]
	#endif
	public static void Load() {
		bool validSaveFound = false;

		if (SaveExists(saveFileName + ".txt")) {
			data = LoadTxt<SaveData>(saveFileName);
			if (data == null)
				data = new SaveData();
			else
				validSaveFound = true;
		} else
			data = new SaveData();

		foreach (ICanBeSaved s in savables) {
			if (validSaveFound)
				s.Load();
			else
				s.Initialize();
		}
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Save/Game Save")]
	#endif
	public static void Save() {
		foreach (ICanBeSaved s in savables)
			s.Save();
		SaveTxt(saveFileName, data);
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/DANGER/DELETE ALL SAVE FILES")]
	private static void DeleteAllSaveFiles() {
		SetPath();
		foreach (string s in Directory.GetFiles(path)) {
			File.Delete(s);
		}
	}
	#endif

	/// <summary>
	/// Gets the names of the files saved in the save folder.
	/// </summary>
	public static string[] GetSaveNames() {
		SetPath();
		string[] files = Directory.GetFiles(path);
		for (int i = 0; i < files.Length; i++)
			files[i] = Path.GetFileNameWithoutExtension(files[i]);
		return files;
	}

	[System.Serializable]
	public class SaveData {
		public float randomSaveValue = Random.value;
		public YarnVariables.YarnVariable[] yarnVariables = new YarnVariables.YarnVariable[0];
		public Vector2 playerPos;
		public int[] playerCards;
	}
}

public interface ICanBeSaved {
	void Save();
	void Load();
	void Initialize();
}