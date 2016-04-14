using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Combat1h = 4,
	Combat2h = 5,
	AttackMelee1h = 6,
	RunMelee1h = 7
}

enum BodySections {
	bsHair = 0,
	bsHead = 1,
	bsTorso = 2,
	bsArms = 3,
	bsHands = 4,
	bsLegs = 5,
	bsFeet = 6
}

/**
 * This is the base class for all game entities, containing common
 * information like inventory, attributes, skills, etc.
 * */
[Serializable]
public class BaseCharacter : MonoBehaviour {
	public string Name;
	public string faction;
	public BaseAttrib[] attrib;
	public float[] HitPoints;		//[0] is current hitpoints [1] is max points
	public float[] EnergyPoints;

	public int level;
	protected int group;
	public string profession;

	public int state;		//Animation state: combat, idle, etc
	public Dictionary<int, BaseItem> inventory;
	public List<BaseSkill> skills;
	public List<BaseAbility> abilities;
	public List<Quest> quests;
	public ActionQueue actions;
	Transform AttachPoint;
	public BaseItem[] equip;		//equipped items
	
	public NavMeshAgent agent;
	public Animator anim;
	public GameObject target;		/// selected entity

	void Awake() {
		DontDestroyOnLoad (this);
		attrib = new BaseAttrib[Enum.GetValues (typeof(Attributes)).Length];
		for (int i=0;i<5;i++)
			attrib [i] = new BaseAttrib();
		equip = new BaseItem[Enum.GetValues (typeof(ItemSlot)).Length]; 
		HitPoints = new float[2];
		HitPoints [0] = 30;
		EnergyPoints = new float[2];
		inventory = new Dictionary<int, BaseItem>();
		quests = new List<Quest> ();
		skills = new List<BaseSkill> ();
		abilities = new List<BaseAbility> ();

		equip = new BaseItem[9];
		actions = new ActionQueue ();
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		anim.SetInteger ("CharacterState", (int)CharacterState.Idle);
		state = (int)CharacterState.Idle;	
	}

	void OnLevelWasLoaded(int level) {
		state = (int)CharacterState.Idle;
		if (anim)
			anim.SetInteger ("CharacterState", (int)CharacterState.Idle);
	}

	public virtual void SaveToFile(FileStream SaveFile) {
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(SaveFile, name);
		bf.Serialize(SaveFile, attrib);
		bf.Serialize(SaveFile, HitPoints);
		bf.Serialize(SaveFile, EnergyPoints);
	}

	public void MoveTo(Vector3 coord) {
		//moving clears the action queue
		actions.Clear ();
		if (state == (int)CharacterState.Combat1h) {
			anim.SetInteger ("CharacterState", (int)CharacterState.RunMelee1h);
			state = (int)CharacterState.RunMelee1h;
		} else {
			anim.SetInteger ("CharacterState", (int)CharacterState.Walking);
			state = (int)CharacterState.Walking;
		}
		agent.SetDestination (coord);
	}

	/**
	 *  Set the entity attributes
	 */
	public void SetAttributes(int[] attr) {
		for (int i=0;i<4;i++){
			attrib[i].baseValue = attr[i];
		}
		HitPoints [1] = 5 * attrib [(int)Attributes.Const].getValue() + 3 * attrib [(int)Attributes.Str].getValue();	//set max hp
		HitPoints [0] =  HitPoints [1];
		EnergyPoints[1]= 2 * attrib [(int)Attributes.Const].getValue() + 3 * attrib [(int)Attributes.Str].getValue();	//set max energy
		EnergyPoints [0] = EnergyPoints [1];
	}

	void ExecuteActions() {
		//execute queued actions
		if (!actions.isEmpty()) {
			Debug.Log("Not empty, executign action");
			var a = actions.getAction();
			if (a.cooldown == 0) {
				Debug.Log("Cooldown is zero");
				//not used yet
				a.cooldown += Time.deltaTime;
				a.Execute();
			} else {
				//action is in cooldown
				Debug.Log("Action in cooldown");
				a.cooldown += Time.deltaTime; 
				if (a.cooldown >= a.time) {
					if (a.loop) 
						a.cooldown = 0; //reset cooldown, so action can be used again
					else 
						actions.popAction();	//remove action
				}
			} //if cooldown			
		} //if actions
	}

	// Update is called once per frame
	void Update () {

		if (!GameInstance.pause) {
			if (!anim.enabled)
				anim.enabled = true;
			if (!agent.hasPath && state==(int)CharacterState.Walking) {
				anim.SetInteger ("CharacterState", (int)CharacterState.Idle);			
				state = (int)CharacterState.Idle;
			}
			if (state == (int)CharacterState.Combat1h) {
				anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);	
			}
			ExecuteActions();
			//Check countdown timers for bonus or buffs

			//Regenerate life and energy

			/*if (Input.GetKey (KeyCode.Mouse1)) {
				anim.SetInteger ("CharacterState", (int)CharacterState.AttackMelee1h);
				
			}*/
			/*if (Input.GetKey (KeyCode.C)) {
				anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
			} */
			/*if (Input.GetKeyDown (KeyCode.T)) {
				//attach tests
				//automatyically set combat stance
				anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
				GameObject wprefab = Resources.Load ("Weapons/ancient-grtsword") as GameObject;
				GameObject weapon = Instantiate (wprefab) as GameObject; 

				foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
					if (t.name ==  "weapon_target_side.R_end")
						AttachPoint = t;
				}
				weapon.transform.SetParent (AttachPoint.transform);
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = Quaternion.Euler (Vector3.zero);
				weapon.transform.localScale = new Vector3(7,7,7);
				//CreateDamagePopup (200);
			}*/
		} else {
			anim.enabled =false;
		}
	
	}

	//Getters
	/**
	 * Returns the attribute class
	*/
	public BaseAttrib GetBaseAttribute(int attr) {
		return attrib [attr];
	}

	public int GetSkill(string skill) {
		foreach (var sk in skills) {
			if (sk.Name == skill)
				return sk.baseValue;
		}
		return 0;
	}
	
	/**
	 * Returns the total value of the attribute
	*/
	public int GetAttribute(int attr) {
		return attrib [attr].getValue();
	}

	public string GetQuestStatus(string name){
		foreach (Quest q in quests) {
			if (q.Name==name) {
				return q.status;				
			}
		}
		return "no";	//the quest is not assigned yet
	}

	public void SetQuestStatus(string id, string status){
		foreach (Quest q in quests) {
			if (q.Name==id) {
				q.status = status;
			}
		}
	}

}
