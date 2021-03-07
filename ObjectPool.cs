using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	private static ObjectPool control;

	private Dictionary<GameObject, List<GameObject>> objects = new Dictionary<GameObject, List<GameObject>>();

	private void Awake() {
		control = this;
	}

	public static int NumObjects => control.transform.childCount;

	public static int NumActiveObjects {
		get {
			Transform trans = control.transform;
			int num = 0;
			for (int i = 0; i < trans.childCount; i++)
				if (trans.GetChild(i).gameObject.activeSelf)
					num++;
			return num;
		}
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position) => control.SpawnObject(prefab, position);

	public GameObject SpawnObject(GameObject prefab, Vector3 position) => SpawnObject(prefab, position, transform);
	public GameObject SpawnObject(GameObject prefab, Vector3 position, Transform parent) {

		if (objects.ContainsKey(prefab)) {
			List<GameObject> instances = objects[prefab];
			for (int i = 0; i < instances.Count; i++) {

				if (instances[i] == null) {
					instances.RemoveAt(i);
					i--;
				} else if (!instances[i].activeSelf) {
					instances[i].SetActive(true);
					instances[i].transform.localPosition = position;
					return instances[i];
				}
			}

			GameObject spawn = Spawn();
			instances.Add(spawn);
			return spawn;

		} else {
			objects.Add(prefab, new List<GameObject>() { Spawn() });
			return objects[prefab][0];
		}

		GameObject Spawn() => Instantiate(prefab, position, Quaternion.identity, parent);
	}

}