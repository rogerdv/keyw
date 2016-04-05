using UnityEngine;
using System.Collections;


public class CGSceneLogic : MonoBehaviour {
	GameInstance gm;
	// Use this for initialization
	void Start () {
		gm = GameInstance.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		//if defend quest is in final phase, spawn demons a the door
		var player = GameInstance.player;// gm.player; //GameObject.FindGameObjectWithTag ("Player");
		if (player == null) {
			GameInstance.InstancePlayer();
		}
		if (player.GetComponent<BaseCharacter> ().GetQuestStatus ("defend-getgear") == "combat") {
			//send everybody to the gates

			//spawn the attackers
		} else if (player.GetComponent<BaseCharacter> ().GetQuestStatus ("kill-raiders") == "active") {
			//spawn the bandits
		}
	}
}
