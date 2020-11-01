using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	private static ObjectPool control;

	private Dictionary<GameObject, List<GameObject>> objects = new Dictionary<GameObject, List<GameObject>>();

	private void Awake() {
		control = this;
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position) => control.SpawnObject(prefab, position);

	private GameObject SpawnObject(GameObject prefab, Vector3 position) {

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

			GameObject spawn = Instantiate(prefab, position, Quaternion.identity, transform);
			instances.Add(spawn);
			return spawn;

		} else {
			objects.Add(prefab, new List<GameObject>() { Instantiate(prefab, position, Quaternion.identity, transform) });
			return objects[prefab][0];
		}
	}
}