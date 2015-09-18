using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MsgList : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetText(string text){
		foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
			if (t.name == "Text") {
				t.GetComponent<Text>().text = t.GetComponent<Text>().text+text + "\n";
			}
		}	
	}
}

