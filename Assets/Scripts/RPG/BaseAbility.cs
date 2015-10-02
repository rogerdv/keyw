using UnityEngine;
using System.Collections;

enum TargetType {
}

public class BaseAbility {
	public int ttype;		//area effect, single target
	public string ParentSkill; 

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
