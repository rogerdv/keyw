using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemWeapon : BaseItem {

	public override void Use (GameObject owner, GameObject target) {
		//TODO: get weapon damages, etc
		//get item parent skill level
		var OwnSc = owner.GetComponent<BaseCharacter> ();
		var TargetSc = target.GetComponent<BaseCharacter> ();
		var ps = OwnSc.GetSkill(ParentSkill);
		/*Debug.Log("Parent skill  is "+ps.Name);
		Debug.Log(ps.baseValue);*/
		//calculate hit roll: determines if target is actually hit or evades attack
		int AttackRoll = ps + OwnSc.GetAttribute( (int)Attributes.Str) + OwnSc.GetAttribute((int)Attributes.Dext);
		int DodgeRoll = TargetSc.GetSkill("dodge") + TargetSc.GetAttribute((int)Attributes.Dext);
		int roll = Random.Range (0, AttackRoll + DodgeRoll + 1);
		if (roll > AttackRoll) {
			//Debug.Log("Attack missed!!");
			GameObject.Find("MessageBox").GetComponent<MsgList>().SetText(OwnSc.Name+" hits the air causing a lot of damege to nobody");
		}
		///weapon does damage: get all damage properties
		foreach (Property p in props) {
			if (p.type=="damage") {
				Debug.Log("Dmg type: "+p.name);
				Debug.Log("Dmg value: "+p.value);
				float dmg = p.value+OwnSc.GetAttribute((int)Attributes.Str)*ps;
				Debug.Log("Effective damage is "+dmg);
				target.GetComponent<BaseCharacter> ().HitPoints [0]-= dmg;
				var bar = GameObject.Find("blood-bar").GetComponent<Image>();
				float percent = TargetSc.HitPoints [0]/TargetSc.HitPoints [1];
				bar.transform.localScale = new Vector3(percent, 1.0f,1.0f);
				//Debug.Log("Dmg value: "+p.value);
				GameObject.Find("MessageBox").GetComponent<MsgList>().SetText(OwnSc.Name+" hits "+TargetSc.Name+" inflicting "+dmg.ToString() +" damage");
			}
		}		 
	}//Use

	public override void Equip(GameObject owner){
		var OwnSc = owner.GetComponent<BaseCharacter> ();
		foreach (var m in mods) {
			if (m.type == "strength") {
				int currentMod = OwnSc.GetBaseAttribute((int)Attributes.Str).modifier;
			}
		}
	}

}

