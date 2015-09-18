using UnityEngine;
using System.Collections;

public class BaseAbility {
	public int TargetType;		//area effect, single target
	public string ParentSkill; 

	/**
	 * Applies the ability to a target
	 * @param owner The caster entity
	 * @param target the receiving entity
	 * */
	public void Apply(BaseCharacter owner, BaseCharacter target) {
		//get skill level from owner
	}
}
