using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AIWarrior : BaseAI {

	// Use this for initialization
	void Start () {
		owner = gameObject.GetComponent<NPC> ();
		StartCoroutine(AIFSM());
	}

	IEnumerator AIFSM()
	{
		while (true) {
			if (owner.AIstate == (int)AIStates.Idle) {
				yield return StartCoroutine ("Idle");
			} else if (owner.AIstate == (int)AIStates.Combat) {
				yield return StartCoroutine ("Combat");
			}
		}
	}

	IEnumerator Idle() {
		//Debug.Log("Warrior idle");
		while(owner.AIstate == (int)AIStates.Idle)
		{	
			/*var prob = Random.Range(0,100);
			if (prob>95) {
				var prefab = Resources.Load("FloatingText") as GameObject;
				var t = Instantiate (prefab) as GameObject;
				RectTransform r = t.GetComponent<RectTransform> ();
				t.transform.SetParent (gameObject.transform.FindChild ("MyCanvas"));
				r.transform.localPosition = prefab.transform.localPosition;
				r.transform.localScale = prefab.transform.localScale;
				r.transform.localRotation = prefab.transform.localRotation;
				//t.transform.LookAt(Camera.main.transform.position);
				t.GetComponent<Text>().text = "I have a cast for a real game next week";
				Destroy (t, 3);
			}*/
			yield return null;
		}
	}
	
	IEnumerator Combat() {
		//Debug.Log("Warrior combat");
		if (owner.equip[(int)ItemSlot.Weapon]==null) {
			Debug.Log("AI: I have no weapon equipped!");
			foreach (KeyValuePair<int,BaseItem> i in owner.inventory) {	//TODO: choose the best weapon
				if (i.Value.type=="weapon") {
					owner.equip[(int)ItemSlot.Weapon] = i.Value;
					/* Test */
					GameObject wprefab = Resources.Load (i.Value.prefab) as GameObject;
					GameObject weapon = Instantiate (wprefab) as GameObject; 
					
					foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
						if (t.name == i.Value.attach) { 
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
			owner.AnimState = (int)CharacterState.Combat1h;
			yield return null;
		} else {
			if (Vector3.Distance (owner.gameObject.transform.position, owner.target.transform.position) > owner.equip[(int)ItemSlot.Weapon].range && !owner.agent.hasPath) {
				//move towards enemy
				owner.MoveTo(owner.target.transform.position);
				yield return null;
			} else if (owner.agent.hasPath) {
				if (Vector3.Distance (owner.gameObject.transform.position, owner.target.transform.position)<owner.equip[(int)ItemSlot.Weapon].range) {
					//swing
					Debug.Log("We are close. Attacking");
					owner.agent.ResetPath();
					ActionAttack a = new ActionAttack();
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
					owner.AnimState = (int)CharacterState.Combat1h;
				} 
				yield return null;
			}
		}
		//check if target is dead
		if (owner.target.GetComponent<BaseCharacter> ().HitPoints [0] < 0) {
			Debug.Log("Target dead!");
			//TODO: check if group fight and get another target
			owner.AIstate = (int)AIStates.Idle;
			owner.actions.Clear();
			owner.anim.SetInteger ("CharacterState", (int)CharacterState.Idle);			
			owner.AnimState = (int)CharacterState.Idle;
			yield return null;
		}
	}
}
