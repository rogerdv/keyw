using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public enum AIStates {
	Idle = 0,
	Combat
}

public class NPC : BaseCharacter {
	public string dialog;	//Dialog file
	public string definition;	//xml file containing entity description
	public uint AIstate;	//FSm state: idle, combat
	public Texture2D portrait;

	// Use this for initialization
	void Start () {       
		anim = GetComponentInChildren<Animator>();
		agent = GetComponent<NavMeshAgent>();
		AIstate = (int)AIStates.Idle;

		//attrib = new BaseAttrib[Enum.GetValues (typeof(Attributes)).Length];
		//skills = new List<BaseSkill> ();

		Load ();
		//once loaded, init all basic attributes
		HitPoints [1] = attrib [(int)Attributes.Str].baseValue * 3 + attrib [(int)Attributes.Const].baseValue * 4;
		HitPoints [0] = HitPoints [1];
		EnergyPoints[1] = attrib [(int)Attributes.Int].baseValue * 3 + attrib [(int)Attributes.Const].baseValue * 2;
		EnergyPoints [0] = EnergyPoints [1];
	}

	/**
	 * Load from Xml
	 * */
	void Load(){
		TextAsset textAsset = (TextAsset) Resources.Load("Entities/"+definition);
		if (!textAsset) Debug.Log("failed to load xml resource");
		var doc = new XmlDocument();		
		doc.LoadXml (textAsset.text);

		XmlNodeList entity = doc.SelectNodes ("entity");	
		var atlist = entity.Item(0).Attributes.GetNamedItem("attrib").Value.Split(' ');
		int count = 0;
		foreach (string atr in atlist) {
			attrib[count] = new BaseAttrib();
			attrib[count].baseValue = int.Parse(atr);
			count++;
		}
		level = int.Parse (entity.Item (0).Attributes.GetNamedItem ("level").Value);
		profession = entity.Item (0).Attributes.GetNamedItem ("class").Value;

		XmlNodeList myskills = doc.SelectNodes ("entity/skills/skill");	
		foreach (XmlNode node in myskills) {
			Debug.Log("skill "+node.Attributes.GetNamedItem("id").Value);
		}

		XmlNodeList myitems = doc.SelectNodes ("entity/inventory/item");	
		foreach (XmlNode node in myitems) {
			Debug.Log("item "+node.Attributes.GetNamedItem("id").Value);
			var item = GameInstance.ItFactory.CreateItem(node.Attributes.GetNamedItem("id").Value);
			inventory.Add(item);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Actual AI goes here
		if (AIstate == (int)AIStates.Idle) {
			//wander

		} else if (AIstate == (int)AIStates.Combat) {
			//check if I have a weapon equipped
			if (equip[(int)ItemSlot.Weapon]==null) {
				Debug.Log("I have no weapon equipped!");
				foreach (var i in inventory) {	//TODO: choose the best weapon
					if (i.type=="weapon") {
						equip[(int)ItemSlot.Weapon] = i;
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
				anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
				state = (int)CharacterState.Combat1h;
				if (Vector3.Distance (gameObject.transform.position, target.transform.position) > equip[(int)ItemSlot.Weapon].range) {
					//move towards enemy
					var dest = Vector3.zero;
					if (gameObject.transform.position.x > target.transform.position.x)
						dest.x = target.transform.position.x+equip[(int)ItemSlot.Weapon].range;
					else 
						dest.x = target.transform.position.x-equip[(int)ItemSlot.Weapon].range;
					if (gameObject.transform.position.z > target.transform.position.z)
						dest.z = target.transform.position.z+equip[(int)ItemSlot.Weapon].range;
					else dest.z = target.transform.position.z-equip[(int)ItemSlot.Weapon].range;
					dest.y = target.transform.position.y;
					MoveTo(target.transform.position);
				} 
			} else {
				//swing
				anim.SetInteger ("CharacterState", (int)CharacterState.AttackMelee1h);
				state = (int)CharacterState.AttackMelee1h;
			}
		}
	}
}
