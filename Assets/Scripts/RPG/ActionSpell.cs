using UnityEngine;
using System.Collections;

public class ActionSpell : CharacterAction {

	public override void Execute() {
		if (OriginAbility.ttype == TargetType.Single) {
			//we only need the target
		} else {	//it is an area spell
		}
	}

}
