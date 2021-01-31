using System.Collections.Generic;
using UnityEngine;

public class SORefer : MonoBehaviour {

	private static SORefer control;

	[SerializeField]
	private ScriptableObject[] allObjects;

	private Dictionary<int, ScriptableObject> objects;

	private void Awake() {
		control = this;

		#if UNITY_EDITOR
		UpdateList();
		#endif

		BuildDictionary();
	}

	#if UNITY_EDITOR
	[ContextMenu("Update List")]
	private void UpdateList() {
		allObjects = null;
		string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:ScriptableObject", new string[] { "Assets/Scriptable Objects" });
		allObjects = new ScriptableObject[guids.Length];
		for (int i = 0; i < guids.Length; i++)
			allObjects[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]));
	}

	[UnityEditor.MenuItem("NOÉ'S FUCK SHIT/Update SORefer")]
	private static void UpdateSORefer() => GameObject.FindObjectOfType<SORefer>().UpdateList();

	#endif

	public static int GetCode<T>(T scriptableObject)where T : ScriptableObject {
		return HashFromNameAndType(scriptableObject.name, typeof(T));
	}

	public static int[] GetCodes<T>(T[] scriptableObjects)where T : ScriptableObject {
		if (scriptableObjects == null)
			return null;
		int[] results = new int[scriptableObjects.Length];
		for (int i = 0; i < results.Length; i++)
			results[i] = GetCode<T>(scriptableObjects[i]);
		return results;
	}

	public static T GetObject<T>(int code)where T : ScriptableObject {
		return (T)control.objects[code];
	}

	public static T[] GetObjects<T>(int[] codes)where T : ScriptableObject {
		if (codes == null)
			return null;
		T[] results = new T[codes.Length];
		for (int i = 0; i < results.Length; i++)
			results[i] = GetObject<T>(codes[i]);
		return results;
	}

	private void BuildDictionary() {
		objects = new Dictionary<int, ScriptableObject>();
		foreach (ScriptableObject so in allObjects)
			objects.Add(HashFromNameAndType(so.name, so.GetType()), so);
	}

	private static int HashFromNameAndType(string name, System.Type type) {
		return (name + type.ToString()).GetHashCode();
	}
}