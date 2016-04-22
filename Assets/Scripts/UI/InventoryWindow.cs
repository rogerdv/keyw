 using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventoryWindow : MonoBehaviour {
	public GameObject invGridPrefab;		//list element
	public GameObject panelPrefab;		//info panel prefab
	GameObject player;
	public GameObject[] buttons; 

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		//create the list
		var scrollArea = GameObject.Find("content");		
			
		var InvList = player.GetComponent<BaseCharacter> ().inventory;
		buttons = new GameObject[InvList.Count];
		//buttons = new GameObject[100];
		int row = 0;
		foreach (KeyValuePair<int,BaseItem> item in InvList) {
			//Debug.Log("Item "+item.Value.Name);
			buttons[row] = Instantiate(invGridPrefab);
			//buttons[row].name =  "Button_"+item.Key.ToString();
				
			var rt = buttons[row].GetComponent<RectTransform>();
			rt.SetParent(scrollArea.transform);

			//Debug.Log("Button not null");
			foreach (Transform t in buttons[row].GetComponentsInChildren<Transform>()){
					if (t.name ==  "icon") {							
						var icon = t.GetComponent<Image>();
						icon.sprite = Resources.Load<Sprite>(item.Value.portrait);
						Debug.Log("Icon "+item.Value.portrait);
					t.name = "Icon_"+item.Key.ToString();
					} else if (t.name ==  "Text") {
						var text = t.GetComponent<Text>();
						text.text = item.Value.Name;
					}
				}
			row++;						
			}
	}
	
	// Update is called once per frame
	void Update () {			
		

	}

	//handles mouse clicks (left and right)
	public void OnPointerDown(PointerEventData data){
		Debug.Log("Button '" + data.pointerPress.name + "' pressed!");
		//Debug.Log("Equip item '" + i.Name);
	}
}
