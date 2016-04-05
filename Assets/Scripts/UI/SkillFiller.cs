using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillFiller : MonoBehaviour {

	public GameObject panel;		//list container
	public GameObject elem;			//list element prefab
	List<GameObject> skills;

	string[] SkNames = {"Melee","Dodge","Evasion", "Swords", "Hammers", "Shields",
		"Physical training", "Combat", "Daggers", "Wands", "Elemental Mastery: fire", 
		"Elemental Mastery: lightning", "Elemental Mastery: earth", "Elemental Mastery: water",
		"Elemental Mastery: air", "Subterfuge", "Lore"};

	// Use this for initialization
	void Start () {
		RectTransform rt;
		skills = new List<GameObject>();
		int i = 0;
		foreach (string n in SkNames) {
			var skill = Instantiate (elem);

			skill.GetComponentInChildren<Text> ().text = n;
			rt = skill.GetComponent<RectTransform> ();
			rt.SetParent (panel.transform);
			skills.Add(skill);
		}

		/*skills [1] = Instantiate (elem);
		skills [1].GetComponentInChildren<Text> ().text = "Dodge";
		rt = skills[1].GetComponent<RectTransform>();
		rt.SetParent(panel.transform);*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
