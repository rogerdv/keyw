using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Create2UI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetProfession(GameObject obj) {
		if (obj.name == "warrior") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a warrior";
			GameInstance.player.GetComponent<BaseCharacter>().profession = "warrior";
		} else if (obj.name == "mage") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a mage";
			GameInstance.player.GetComponent<BaseCharacter>().profession = "mage";
		} else if (obj.name == "cleric") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a cleric";
		} else if (obj.name == "tracker") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a tracker";
		} else if (obj.name == "shenzu") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a shenzu";
		} else if (obj.name == "thief") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a thief";
		}

	}

	public void StartGame() {
		LevelManager.Load("Calesoni_Castle");
	}
}
