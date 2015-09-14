﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

enum AIStates {
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
		AIstate = (int)AIStates.Idle;
		Debug.Log("xml "+definition);
		attrib = new BaseAttrib[Enum.GetValues (typeof(Attributes)).Length];
		skills = new List<BaseSkill> ();

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

		XmlNodeList myskills = doc.SelectNodes ("entity/skills/skill");	
		foreach (XmlNode node in myskills) {
			Debug.Log("skill "+node.Attributes.GetNamedItem("id").Value);
		}

		XmlNodeList myitems = doc.SelectNodes ("entity/inventory/item");	
		foreach (XmlNode node in myitems) {
			Debug.Log("item "+node.Attributes.GetNamedItem("id").Value);
		}

	}

	
	// Update is called once per frame
	void Update () {
		//Actual AI goes here
		if (AIstate == (int)AIStates.Idle) {
			//wander

		}
	}
}
