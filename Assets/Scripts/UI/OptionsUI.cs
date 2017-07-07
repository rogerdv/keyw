using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsUI : MonoBehaviour {
	public GameObject ssao;
	public GameObject shadow;
	int i;		//resolution index
	Resolution[] resolutions;
	string[] DiffLevel;

	GameObject VideoPanel;
	GameObject GamePanel;

	// Use this for initialization
	void Start () {
		//get all the panels
		foreach (var t in gameObject.GetComponentsInChildren<Transform>()) {
		}
		resolutions = Screen.resolutions;
		Debug.Log (resolutions.Length);
		Resolution current = Screen.currentResolution;
		i = 0;
		/*foreach (Resolution res in resolutions) {
			if (res.ToString() == current.ToString()) {
				Debug.Log(res.ToString());
				break;
			}				
			i++;
		}*/
		//GameObject.Find ("ResolText").GetComponent<Text> ().text = resolutions [i].ToString ();

		//set toggles to current values
		foreach (var c in gameObject.GetComponentsInChildren<Toggle>()) {
			if (c.name == "shadow") {
				c.isOn = GameInstance.options.shadows;
			} else if (c.name == "ssao") {
				c.isOn = GameInstance.options.ssao;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void VideoClick() {

	}

	public void GameplayClick() {
		//disable the other panels
	}

	public void SSAOToggle() {
		foreach (var c in gameObject.GetComponentsInChildren<Toggle>()) {
			if (c.name == "ssao") {
				if (c.isOn) {
					GameInstance.options.ssao = true;
					var cam = GameObject.FindGameObjectWithTag("MainCamera");
					//var ssaocomp = cam.GetComponent<ScreenSpaceAmbientOcclusion>();
					//if (ssaocomp !=null) ssaocomp.enabled = true;
				} else {
					GameInstance.options.ssao = false;
					var cam = GameObject.FindGameObjectWithTag("MainCamera");
					//var ssaocomp = cam.GetComponent<ScreenSpaceAmbientOcclusion>();
					//if (ssaocomp !=null) ssaocomp.enabled = false;
				}
			}
		}
	}

	public void ShadowToggle() {
		foreach (var c in gameObject.GetComponentsInChildren<Toggle>()) {
			if (c.name == "shadow") {
				if (c.isOn) {
					GameInstance.options.shadows = true;
					var SceneInf = GameObject.Find ("SceneInfo");
					if (SceneInf != null) { //reset all lights shadow to on
						foreach( var light in FindObjectsOfType<Light>()) {
							light.shadows = LightShadows.Hard;
						}//foreach
					}
				} else {
					GameInstance.options.shadows = false;
					var SceneInf = GameObject.Find ("SceneInfo");
					if (SceneInf != null) { //reset all lights shadow to off
						foreach( var light in FindObjectsOfType<Light>()) {
							light.shadows = LightShadows.None;
						}//foreach
					}
				}
			}
		}
	}

	public void FXAAToggle() {
		foreach (var c in gameObject.GetComponentsInChildren<Toggle>()) {
			if (c.name == "fxaa") {
				if (c.isOn) {
					GameInstance.options.fxaa = true;
					var cam = GameObject.FindGameObjectWithTag("MainCamera");
					//var aacomp = cam.GetComponent<Antialiasing>();
					//if (aacomp !=null) aacomp.enabled = true;
				} else {
					GameInstance.options.fxaa = false;
					var cam = GameObject.FindGameObjectWithTag("MainCamera");
					//var aacomp = cam.GetComponent<Antialiasing>();
					//if (aacomp !=null) aacomp.enabled = false;
				}
			}
		}
	}

	public void PrevResolution() {
		if (i > 0) {
			i--;
			GameObject.Find ("ResolText").GetComponent<Text> ().text = resolutions [i].ToString ();
		}
	}

	public void NextResolution() {
		if (i < resolutions.Length) {
			i++;
			GameObject.Find ("ResolText").GetComponent<Text> ().text = resolutions [i].ToString ();
		}
	}

	/*
	 * Close and save config
	 */
	public void CloseWindow() {
		Debug.Log("Closing...");
		GameInstance.options.Save ();
		GameInstance.pause = false;
		Destroy (gameObject);
	}
}
