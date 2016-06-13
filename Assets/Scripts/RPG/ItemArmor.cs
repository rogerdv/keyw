using UnityEngine;
using System.Collections;

public class ItemArmor : BaseItem {

	public override void Use (GameObject owner, GameObject target) {
			 
	}//Use
	
	public override void Equip(GameObject owner){
		var OwnSc = owner.GetComponent<BaseCharacter> ();
		if (swap) {

		}
		foreach (var m in mods) {
			if (m.type == "strength") {
				int currentMod = OwnSc.GetBaseAttribute((int)Attributes.Str).modifier;
			}
		}
	}
}
