using UnityEngine;
using System.Collections;

public class ItemPotion : BaseItem {

	public virtual void Use (GameObject owner, GameObject target) {

		foreach (Property p in props) {
			if (p.type == "restore") {
				if (p.name == "hp") {
					target.GetComponent<BaseCharacter>().HitPoints[0]+= p.value;
					//check if max health was exceeded 
					if (target.GetComponent<BaseCharacter>().HitPoints[0]>target.GetComponent<BaseCharacter>().HitPoints[1])
						target.GetComponent<BaseCharacter>().HitPoints[0]=target.GetComponent<BaseCharacter>().HitPoints[1];
				} else if (p.name == "energy") {
					target.GetComponent<BaseCharacter>().EnergyPoints[0]+= p.value;
					//check if max health was exceeded 
					if (target.GetComponent<BaseCharacter>().EnergyPoints[0]>target.GetComponent<BaseCharacter>().EnergyPoints[1])
						target.GetComponent<BaseCharacter>().EnergyPoints[0]=target.GetComponent<BaseCharacter>().EnergyPoints[1];
				}
			}
		}
	}	

}
