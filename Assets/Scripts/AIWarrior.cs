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
			if (Vector3.Distance (owner.gameObject.transform.position, owner.target.transform.position) > owner.equip[(int)ItemSlot.Weapon].range) {
				//move towards enemy
				var dest = Vector3.zero;
				if (gameObject.transform.position.x > owner.target.transform.position.x)
					dest.x = owner.target.transform.position.x+owner.equip[(int)ItemSlot.Weapon].range;
				else 
					dest.x = owner.target.transform.position.x-owner.equip[(int)ItemSlot.Weapon].range;
				if (gameObject.transform.position.z > owner.target.transform.position.z)
					dest.z = owner.target.transform.position.z+owner.equip[(int)ItemSlot.Weapon].range;
				else dest.z = owner.target.transform.position.z-owner.equip[(int)ItemSlot.Weapon].range;
				dest.y = owner.target.transform.position.y;
				owner.MoveTo(owner.target.transform.position);
			} 
		} else {
			//swing
			owner.anim.SetInteger ("CharacterState", (int)CharacterState.AttackMelee1h);
			owner.state = (int)CharacterState.AttackMelee1h;
		}
	}
}
