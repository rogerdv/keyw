using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ItemSlot {
	Head = 0,
	Chest = 1,
	Weapon = 2,
	Shield = 3,
	Legs = 4,
	Feet = 5,
	LeftRing = 6,
	RightRing = 7,
	Amulet = 8
}

/**
 * Modifiers add/decrease stuff like attributes or skills
 * */
[Serializable]
public class Modifier {
	public string type;		//is it an stat or skill?
	public string target;	//what skill or stat is affected?
	public int value;
}

[Serializable]
public class Property {
	public string name;		//property (slash, fire, etc), sort of a subtype
	public string type;		//type: damage, protection, stat
	public float value;
}

[Serializable]
public class BaseItem  {
	public string Name;
	public string Desc;
	public string type;		//item type
	public int weight;
	public float range;
	public int quality;
	public string prefab;
	public string portrait;	//icon image
	public string attach;	//where to attach this item
	public bool swap;		//does it swap a body section, like chest, hands, etc?
	public ItemSlot slot;		//slot
	public Attributes ParentAttr;		//attribute this item depends on
	public string ParentSkill;		//skill this item depends on
	public float UseTime;
	public float cooldown;
	List<Modifier> mods;		//bonus and penalizations of this item
	public List<Property> props;

	public BaseItem() {
		mods = new List<Modifier> ();
		props = new List<Property> ();
	}

	public void Use (GameObject owner, GameObject target) {
		//TODO: get weapon damages, etc
		//get item parent skill level
		var ps = owner.GetComponent<BaseCharacter>().GetSkill(ParentSkill);
		Debug.Log("Parent skill  is "+ps.Name);
		Debug.Log(ps.baseValue);
		if (type=="weapon") {
			///weapon does damage: get all damage properties
			foreach (Property p in props) {
				if (p.type=="damage")
					Debug.Log("Dmg "+p.name);				
			}			
		} else if (type=="potion") {
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
	}//Use

	/**
	 * For items with Area of Effect
	 * */
	public void Use (GameObject owner, Vector3 position) {
		var ps = owner.GetComponent<BaseCharacter>().GetSkill(ParentSkill);
	}

	public void Equip(GameObject owner){
		var OwnSc = owner.GetComponent<BaseCharacter> ();
		foreach (var m in mods) {
			if (m.type == "strength") { 		//redo!!!
				int currentMod = OwnSc.GetBaseAttribute((int)Attributes.Str).modifier;
			}
		}
	}

	/*
	 * Returns the value of a given property
	 */
	public float GetPropertyValue(string pname, string type) {
		foreach (var p in props) {
			if (p.name == pname && p.type==type) 
				return p.value;
		}
		return 0;
	}

	/*
	 * Returns the list of properties of a given type
	 * 
	 */
	public List<Property> GetProperties(string type) {
		return null;
	}
}
