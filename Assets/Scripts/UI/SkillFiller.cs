using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

class SkillDef {
	public string name;
	public string desc;
}
public class SkillFiller : MonoBehaviour {

	public GameObject panel;		//list container
	public GameObject elem;			//list element prefab
	List<GameObject> skills;
	Dictionary<string,SkillDef> SkList;

	// Use this for initialization
	void Start () {
		RectTransform rt;

		TextAsset textAsset = (TextAsset) Resources.Load("skill-list");
		if (!textAsset) Debug.Log("failed to load xml resource");
		var doc = new XmlDocument();
		
		doc.LoadXml (textAsset.text);
		SkList = new Dictionary<string, SkillDef> ();
		XmlNodeList root = doc.SelectNodes ("skills/skill");
		string id;		//for parsing
		foreach (XmlNode node in root) {
			id = node.Attributes.GetNamedItem("id").Value;
			SkList[id] = new SkillDef();
			SkList[id].name = node.Attributes.GetNamedItem("name").Value;
			SkList[id].desc = node.Attributes.GetNamedItem("description").Value;
		}//foreach node
		skills = new List<GameObject>();
		int i = 0;
		foreach (KeyValuePair<string, SkillDef> pair in SkList) {
			var skill = Instantiate (elem);

			skill.GetComponentInChildren<Text> ().text = pair.Value.name;
			rt = skill.GetComponent<RectTransform> ();
			rt.SetParent (panel.transform);

			var evt = skill.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;

			entry.callback = new EventTrigger.TriggerEvent();
			UnityEngine.Events.UnityAction<BaseEventData> callback =
				new UnityEngine.Events.UnityAction<BaseEventData>(elementClick);

			entry.callback.AddListener(callback);
			evt.triggers.Add(entry);
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

	public void elementClick(BaseEventData eventData) {

	}
}
