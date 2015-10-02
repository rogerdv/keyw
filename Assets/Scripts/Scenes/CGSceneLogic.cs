using UnityEngine;
using System.Collections;


public class CGSceneLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if defend quest is in final phase, spawn demons a the door
		var player = GameObject.FindGameObjectWithTag ("Player");
		if (player.GetComponent<BaseCharacter> ().GetQuestStatus ("defend-getgear") == "combat") {
			//send everybody to the gates

			//spawn the attackers
		} else if (player.GetComponent<BaseCharacter> ().GetQuestStatus ("kill-raiders") == "active") {
			//spawn the bandits
		}
	}
}
