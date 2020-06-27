using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	[Header("only need to fill in 1 of these!")]
	public string sceneName;
	public int sceneBuildIndex;

	public void SwitchScene() {
		if (sceneName != "")
			SwitchScene(sceneName);
		else
			SwitchScene(sceneBuildIndex);
	}

	public void SwitchScene(string name) {
		SceneManager.LoadScene(name);
	}

	public void SwitchScene(int sceneNum) {
		SceneManager.LoadScene(sceneNum);
	}
}