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
public class Modifier {
	public string type;
	public int value;
}

[Serializable]
public class BaseItem  {
	public string Name;
	public string Desc;
	public int weight;
	public int quality;
	public string mesh;
	public string portrait;	//icon image
	public string attach;	//where to attach this item
	public bool swap;		//does it swap a body section, like chest, hands, etc?
	public ItemSlot slot;		//slot
	public Attributes ParentAttr;		//attribute this item depends on
	public string ParentSkill;		//skill this item depends on
	public float UseTime;
	public float cooldown;
	List<Modifier> mods;		//bonus and penalizations of this item

	public BaseItem() {
		mods = new List<Modifier> ();
	}

	public void Use(GameObject owner, GameObject target) {
	}

	public void Equip(GameObject owner){
		var OwnSc = owner.GetComponent<BaseCharacter> ();
		foreach (var m in mods) {
			if (m.type == "strength") {
				int currentMod = OwnSc.GetBaseAttribute((int)Attributes.Str).modifier;
			}
		}
	}


}
