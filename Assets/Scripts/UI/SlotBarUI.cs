using UnityEngine;
using System.Collections;

public class SlotBarUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SlotClick(GameObject obj) {
		Debug.Log (obj.name);
		if (obj.name == "01") {
			//temp hack: slot1 == attack
			var PlayerSc =  GameInstance.player.GetComponent<BaseCharacter>();
			if (PlayerSc.target!=null) {
				PlayerSc.AnimState = (int)CharacterState.Combat1h;
				var npcsc = PlayerSc.target.GetComponent<NPC>();
				var ai = PlayerSc.target.GetComponent<NPC>();
				npcsc.AIstate = (int)AIStates.Combat;
				npcsc.target = GameInstance.player;
				var prefab = Resources.Load("FloatingText") as GameObject;
				var t = Instantiate (prefab) as GameObject;
				RectTransform r = t.GetComponent<RectTransform> ();
				t.transform.SetParent (PlayerSc.target.transform.FindChild ("MyCanvas"));
				r.transform.localPosition = prefab.transform.localPosition;
				r.transform.localScale = prefab.transform.localScale;
				r.transform.localRotation = prefab.transform.localRotation;
				Destroy (t, 2);
			} else {//no target
				GameObject.Find("MessageBox").GetComponent<MsgList>().SetText("You swing your weapon in the air and people looks at you as if you were crazy");
			}
		} 
		else if (obj.name == "02") {
		} 
		else if (obj.name == "03") {
		}
		else if (obj.name == "04") {
		}
	}
}
