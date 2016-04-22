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
			GameObject.Find("ProfHelp").GetComponent<Text>().text = "Warriors depend on strength, big weapons and heavy armor. They use incapacitating attacks followed by killer moves to liquidate their opponents.";
			GameInstance.player.GetComponent<BaseCharacter>().profession = "warrior";
		} else if (obj.name == "mage") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a mage";
			GameObject.Find("ProfHelp").GetComponent<Text>().text = "Mages slow or paralyze their foes to keep them at distance. Their most important attribute is Intelligence.";
			GameInstance.player.GetComponent<BaseCharacter>().profession = "mage";
		} else if (obj.name == "cleric") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a cleric";
			GameObject.Find("ProfHelp").GetComponent<Text>().text = "";
		} else if (obj.name == "tracker") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a tracker";
		} else if (obj.name == "shenzu") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a shenzu";
			GameInstance.player.GetComponent<BaseCharacter>().profession = "shenzu";
			GameObject.Find("ProfHelp").GetComponent<Text>().text = "Shenzus are lethal assassins, with some magical abilities. Their fast and precise attacks benefits from dexterity, and their magic tricks require some intelligence.";
		} else if (obj.name == "thief") {
			GameObject.Find("Profession").GetComponent<Text>().text = "You are a thief";
		}

	}

	public void StartGame() {
		LevelManager.Load("Calesoni_Castle");
	}
}
