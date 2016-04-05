using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum ActionType {
	CastSpell = 0,
	UseItem = 1
}

/*
* @brief An action is anything executed by a character: swing, cast, etc .
 * It is influenced by some parent skill and it is handled by an script.
 * For example: player attacks NPC X with a sword
 * A loopable action is created with Sword fighting script
 * time required to swing is calculated by the script based on equiped weapon,
 * dexterity, etc. Such time must be elapsed to perform this (or any queued)
 * action again
*/
[Serializable]
public class CharacterAction {
	string name;
	public ActionType type;
	public bool loop;          // if true, action will be repeated if there is no other queued
	public float time;           //< required time
	public float cooldown;		//elapsed time since last use
	//origin are the item or skill being used/casted by the caster
	public BaseItem OriginItem;
	public GameObject OriginCharacter;
	public BaseAbility OriginAbility;
	//targets are the destination of the skill or item being used
	public GameObject TargetCharacter;
	public Vector3 TargetPosition;
	public virtual void Execute() {
		/*if (type == ActionType.UseItem) {
			OriginItem.Use(OriginCharacter, TargetCharacter);
			TargetCharacter.GetComponent<BaseCharacter> ().HitPoints [0]-= 5;
		} else if (type == ActionType.CastSpell) {	//execute spell or ability on target/spot
			//get level from spell parent skill
			if (OriginAbility.ttype == TargetType.Single) {
				//we only need the target
			} else {	//it is an area spell
			}
		}*/
	}		
}

/**
 * Entity action queue 
 */
[Serializable]
public class ActionQueue {
	public ActionQueue() {
		actions = new List<CharacterAction> ();
	}
	public bool isEmpty() {
		if (actions.Count == 0) 
			return true;
		else
			return false;
	}

	public CharacterAction getAction() {
		if (!isEmpty ()) 
			return actions [0];
		else
			return null;
	}

	public void popAction() {
		if (!isEmpty ()) 
			actions.Remove(actions[0]); 
	}

	public bool isQueued(string name) {
		return true;
	}

	public void addAction(CharacterAction a) {
		actions.Add (a);
	}

	public void Clear() {
		actions.Clear ();
	}
	List<CharacterAction> actions;
}