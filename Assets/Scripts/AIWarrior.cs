using UnityEngine;
using System.Collections;

public class AIWarrior : BaseAI {

	// Update is called once per frame
	void Update () {
		if (owner.AIstate == (int)AIStates.Idle) {
			State_Idle();
		} else if (owner.AIstate == (int)AIStates.Combat) {
			State_Combat();
		}
	}

	void State_Idle() {
		//Debug.Log("Warrior idle");
	}
	
	void State_Combat() {
		//Debug.Log("Warrior combat");
		if (owner.equip[(int)ItemSlot.Weapon]==null) {
			Debug.Log("I have no weapon equipped!");
			foreach (var i in owner.inventory) {	//TODO: choose the best weapon
				if (i.type=="weapon") {
					owner.equip[(int)ItemSlot.Weapon] = i;
					/* Test */
					GameObject wprefab = Resources.Load ("Weapons/"+i.prefab) as GameObject;
					GameObject weapon = Instantiate (wprefab) as GameObject; 
					
					foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
						if (t.name == i.attach) { //"weapon_target_side.R_end"
							weapon.transform.SetParent (t);
							weapon.transform.localPosition = new Vector3(9.2f, -9.9f,-12.4f);
							weapon.transform.localRotation = Quaternion.Euler (new Vector3(35.56f, 358.5f, 358.9f));
							weapon.transform.localScale = new Vector3(1,7f,7);
						}
					}
					/* Test**/
					break;
				}
			}
			owner.anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
			owner.state = (int)CharacterState.Combat1h;

		} else {
			if (Vector3.Distance (owner.gameObject.transform.position, owner.target.transform.position) > owner.equip[(int)ItemSlot.Weapon].range && !owner.agent.hasPath) {
				//move towards enemy
				owner.MoveTo(owner.target.transform.position);
			} else if (owner.agent.hasPath) {
				if (Vector3.Distance (owner.gameObject.transform.position, owner.target.transform.position)<owner.equip[(int)ItemSlot.Weapon].range) {
					//swing
					Debug.Log("We are close. Attacking");
					owner.agent.ResetPath();
					CharacterAction a = new CharacterAction();
					a.type = ActionType.UseItem;
					a.loop = true;
					a.time = 1.0f;
					a.cooldown = 0;
					a.OriginCharacter = gameObject;
					a.TargetCharacter = owner.target;
					a.OriginItem = owner.equip[(int)ItemSlot.Weapon];
					owner.actions.addAction(a);
					//owner.anim.SetInteger ("CharacterState", (int)CharacterState.AttackMelee1h);
					//owner.state = (int)CharacterState.AttackMelee1h;
					owner.anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
					owner.state = (int)CharacterState.Combat1h;
				} 
			}
		}
		//check if target is dead
		if (owner.target.GetComponent<BaseCharacter> ().HitPoints [0] < 0) {
			Debug.Log("Target dead!");
			//TODO: check if group fight and get another target
			owner.AIstate = (int)AIStates.Idle;
			owner.actions.Clear();
			owner.anim.SetInteger ("CharacterState", (int)CharacterState.Idle);			
			owner.state = (int)CharacterState.Idle;
		}
	}
}
