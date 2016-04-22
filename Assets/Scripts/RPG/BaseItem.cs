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
public class BaseItem {
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
	public Vector3 offset;
	public Vector3 scale;
	public Vector3 rot;
	public ItemSlot slot;		//slot
	//public float speed;		//Time to use this item
	public Attributes ParentAttr;		//attribute this item depends on
	public string ParentSkill;		//skill this item depends on
	public float UseTime;
	public float cooldown;
	public List<Modifier> mods;		//bonus and penalizations of this item
	public List<Property> props;

	public BaseItem() {
		mods = new List<Modifier> ();
		props = new List<Property> ();
	}

	public virtual void Use (GameObject owner, GameObject target) {

	}//Use

	/**
	 * For items with Area of Effect
	 * */
	public virtual void Use (GameObject owner, Vector3 position) {
		var ps = owner.GetComponent<BaseCharacter>().GetSkill(ParentSkill);
	}

	public virtual void Equip(GameObject owner){
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
