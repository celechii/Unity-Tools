using System.Collections.Generic;
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

	private static string path;

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
		path = Application.persistentDataPath + "/Saves";
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

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Load Game")]
	#endif
	public static void Load() {
		if (SaveExists("SaveData.txt")) {
			data = LoadTxt<SaveData>("SaveData");
			if (data == null)
				data = new SaveData();
			else
				foreach (ICanBeSaved s in savables)
					s.Load();
		} else
			data = new SaveData();
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Save Shit/Save Game")]
	#endif
	public static void Save() {
		foreach (ICanBeSaved s in savables)
			s.Save();
		SaveTxt("SaveData", data);
	}

	[System.Serializable]
	public class SaveData {
		// save fields
	}
}

public interface ICanBeSaved {
	void SubscribeToSaveSystem();
	void Save();
	void Load();
}