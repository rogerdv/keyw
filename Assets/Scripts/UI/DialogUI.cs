using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogUI : MonoBehaviour {
	static private string dlgfile;
	private Dialog dlg;
	public GameObject ButtonPrefab;
	NPCLine line; 	//the npc text
	Dictionary<string, PlayerLine> options;	//player answers
	GameObject NPCText;
	List<GameObject> buttons;		//used to keep the buttons for destroying them
	BaseCharacter player;

	// Use this for initialization
	void Start () {
		dlg = new Dialog ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<BaseCharacter>(); 
		string npcdlg = GameInstance.Selected.GetComponent<NPC> ().dialog;
		foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
			if (t.name ==  "Text") {
				NPCText = t.gameObject;
			}
		}
		dlg.Load (player, "Dialogs/"+npcdlg);
		Display ("root");
	}

	void Display(string node) {
		//Debug.Log("display dialog");

		line = dlg.GetNode (node);
		//Debug.Log (line.text);
		NPCText.GetComponent<Text> ().text = line.text;
		options = dlg.GetAnswers (line.answers);
		//now create the list of choices
		int count = 0;
		buttons = new List<GameObject> ();
		foreach (KeyValuePair<string, PlayerLine> pl in options) {
			var go = Instantiate(ButtonPrefab);
			go.name =  pl.Key;
			var rt = go.GetComponent<RectTransform>();
			rt.SetParent(gameObject.transform);
			rt.anchoredPosition = new Vector2(10,-80-count*32);
			count++;
			foreach (Transform bText in go.GetComponentsInChildren<Transform>()) {
				if (bText.name ==  "Text") {
					bText.GetComponent<Text>().text = pl.Value.text;
				}
			}
			//setup the event handler
			var b = go.GetComponent<Button>();
			b.onClick.AddListener(() => MouseClick(go));
			buttons.Add(go);
		}
	}

	void MouseClick(GameObject obj){
		//Debug.Log("Button '" + obj.name + "' pressed!");
		string id = obj.name;
		foreach (var b in buttons)
			Destroy (b);
		buttons.Clear ();
		//Debug.Log("Displaying " + options[id].link);
		//execute actions
		if (options [id].actions.Count > 0) {
			foreach (Action a in options [id].actions) {
				if (a.ActionType == "AssignQuest") {
					Quest q = new Quest();
					q.Name = a.id;
					player.quests.Add(q);
				} else if (a.ActionType == "SetQuest") {
					player.SetQuestStatus(a.id, a.value);

				} else	if (a.ActionType == "GiveItem") {
					int idx = UnityEngine.Random.Range(100,90000);
					if (player.inventory.ContainsKey(idx))
						idx = UnityEngine.Random.Range(90000,200000);
					player.inventory[idx]=GameInstance.ItFactory.CreateItem(a.id);
				} else if (a.ActionType == "RemoveItem") {

				}
			}//foreach
		} //if actions>0
		if (options [id].link != "end")
			Display (options [id].link);
		else 
			Destroy (gameObject);

	}
}
