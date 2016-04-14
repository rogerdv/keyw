using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum AIStates {
	Idle = 0,
	Combat = 1
}

public class NPC : BaseCharacter {
	public string dialog;	//Dialog file
	public string definition;	//xml file containing entity description
	public int AIstate;	//FSm state: idle, combat
	public Sprite portrait;

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

	public override void SaveToFile(FileStream SaveFile) {
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(SaveFile, name);
		bf.Serialize(SaveFile, attrib);
		bf.Serialize(SaveFile, HitPoints);
		bf.Serialize(SaveFile, EnergyPoints);
		bf.Serialize(SaveFile, definition);
		bf.Serialize(SaveFile, AIstate);
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
			//Debug.Log("skill "+node.Attributes.GetNamedItem("id").Value);
			BaseSkill s = new BaseSkill();
			s.Name = node.Attributes.GetNamedItem("id").Value;
			s.baseValue = int.Parse(node.Attributes.GetNamedItem("level").Value);
			skills.Add(s);
		}

		XmlNodeList myabilities = doc.SelectNodes ("entity/skills/abilities");	
		foreach (XmlNode node in myabilities) {
			//Debug.Log("skill "+node.Attributes.GetNamedItem("id").Value);
			BaseAbility a = new BaseAbility();
			a.Name = node.Attributes.GetNamedItem("id").Value;
			abilities.Add(a);
		}

		XmlNodeList myitems = doc.SelectNodes ("entity/inventory/item");	
		foreach (XmlNode node in myitems) {
			//Debug.Log("item "+node.Attributes.GetNamedItem("id").Value);
			var item = GameInstance.ItFactory.CreateItem(node.Attributes.GetNamedItem("id").Value);
			int idx = UnityEngine.Random.Range(100,90000);
			if (inventory.ContainsKey(idx))
				idx = UnityEngine.Random.Range(90000,200000);
			inventory[idx] = item;
		}
	}

}
