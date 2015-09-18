using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Combat1h = 4,
	Combat2h = 5,
	AttackMelee1h = 6,
	WalkMelee1h = 7
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
	public string VisibleName;
	public string faction;
	public BaseAttrib[] attrib;
	public int[] HitPoints;		//[0] is current hitpoints [1] is max points
	public int[] EnergyPoints;

	protected int level;
	protected int group;
	public string profession;

	protected int state;		//Animation state: combat, idle, etc
	public List<BaseItem> inventory;
	public List<BaseSkill> skills;
	public List<Quest> quests;
	Transform AttachPoint;
	public BaseItem[] equip;		//equipped items

	
	protected NavMeshAgent agent;
	protected Animator anim;
	public GameObject target;		/// selected entity

	void Awake() {
		DontDestroyOnLoad (this);
		attrib = new BaseAttrib[Enum.GetValues (typeof(Attributes)).Length];
		attrib [(int)Attributes.Str] = new BaseAttrib();
		equip = new BaseItem[Enum.GetValues (typeof(ItemSlot)).Length]; 
		HitPoints = new int[2];
		EnergyPoints = new int[2];
		inventory = new List<BaseItem>();
		quests = new List<Quest> ();
		skills = new List<BaseSkill> ();

		equip = new BaseItem[9];
		//add two test items
		/*BaseItem t = new BaseItem();
		t.Name = "Basic sword of the player";
		t.portrait = "Icons/great-sword";
		inventory.Add(t);
		t = new BaseItem();
		t.Name = "Iron shield";
		t.portrait = "Icons/iron-shield";
		inventory.Add(t);*/
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

	public void MoveTo(Vector3 coord) {
		if (state == (int)CharacterState.Combat1h) {
			anim.SetInteger ("CharacterState", (int)CharacterState.WalkMelee1h);
			state = (int)CharacterState.WalkMelee1h;
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
	
	// Update is called once per frame
	void Update () {

		if (!GameInstance.pause) {
			if (!anim.enabled)
				anim.enabled = true;
			if (!agent.hasPath && state==(int)CharacterState.Walking) {
				anim.SetInteger ("CharacterState", (int)CharacterState.Idle);			
				state = (int)CharacterState.Idle;
			}

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

	public BaseSkill GetSkill(string skill) {
		foreach (var sk in skills) {
			if (sk.Name == skill)
				return sk;
		}
		return null;
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
