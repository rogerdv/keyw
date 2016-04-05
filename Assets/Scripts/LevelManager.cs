using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static void Load(string name) {
		GameObject go = new GameObject("LevelManager");
		LevelManager instance = go.AddComponent<LevelManager>();
		instance.StartCoroutine(instance.InnerLoad(name));
	}
	
	IEnumerator InnerLoad(string name) {
		//load transition scene
		Object.DontDestroyOnLoad(this.gameObject);
		Application.LoadLevel("LoadingScreen");
		
		//wait one frame (for rendering, etc.)
		yield return null;
		//yield return new WaitForSeconds(3);
		
		//load the target scene
		AsyncOperation async = Application.LoadLevelAsync(name);
		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
		Destroy(this.gameObject);
	}
}
