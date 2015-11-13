using UnityEngine;
using System;
using System.Collections;

public enum TargetType {
	Single,
	Area
}

[Serializable]
public class BaseAbility: ScriptableObject {
	public TargetType ttype;		//area effect, single target
	public string ParentSkill;
	public GameObject particle;		//particle system prefab
	public int baseValue;
	int modifier;

	/**
	 * Applies the ability to a target
	 * @param owner The caster entity
	 * @param target the receiving entity
	 * */
	public void Apply(GameObject owner, GameObject target) {
		//get skill level from owner
		var ps = owner.GetComponent<BaseCharacter>().GetSkill(ParentSkill);
	}

	public void Apply(GameObject owner, Vector3 position) {
		//get skill level from owner
		var ps = owner.GetComponent<BaseCharacter>().GetSkill(ParentSkill);
		var colliders = Physics.OverlapSphere (position, 30.0f);
		foreach (var c in colliders) {
			//Debug.Log(c.name);
		}

	}
}
