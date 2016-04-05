using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BaseSkill {
	public string Name;
	public int baseValue;
	int modifier;
	Attributes parent;

	public void Use(GameObject owner, GameObject target) {
		var OwnSc = owner.GetComponent<BaseCharacter>();
		var TargetSc = target.GetComponent<BaseCharacter>();
	}

	/*
	 * Effective value is skill real level after considering owner attribs and such
	 * */
	float GetEffectiveValue() {
		return baseValue;
	}

}
