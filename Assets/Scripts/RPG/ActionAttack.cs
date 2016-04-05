using UnityEngine;
using System.Collections;

/**
 * An attack action checks equipped weapon and tries to use it on target
 * */
public class ActionAttack : CharacterAction {

	public override void Execute() {
		OriginItem.Use(OriginCharacter, TargetCharacter);
		TargetCharacter.GetComponent<BaseCharacter> ().HitPoints [0]-= 5;
	}

}
