using System;
using System.Security.Cryptography;
using System.Text;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_ENGINE
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

[ExecuteInEditMode]
public class UniqueID : MonoBehaviour {

	public string group;
	[ReadOnly]
	public int ID;

	[ContextMenu("Regenerate")]
	private void Reset() {
		MD5 md5 = MD5.Create();

		ID = BitConverter.ToInt32(md5.ComputeHash(Encoding.UTF8.GetBytes(group + GetInstanceID())), 0);
		md5.Clear();

		#if UNITY_ENGINE
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		#endif
	}
}