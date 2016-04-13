 using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryWindow : MonoBehaviour {
	public GameObject invGridPrefab;		//list element
	public GameObject panelPrefab;		//info panel prefab
	GameObject player;
	GameObject[] buttons; 

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		var inv = gameObject;
			
		//create the list
		var scrollArea = GameObject.Find("content");		
			
		var InvList = player.GetComponent<BaseCharacter> ().inventory;
		buttons = new GameObject[InvList.Count];
		int row = 0;
		foreach (var item in InvList) {
			Debug.Log("Item "+item.Name);
			buttons[row] = Instantiate(invGridPrefab);
			buttons[row].name =  "Button_"+item.Name;
				
			var rt = buttons[row].GetComponent<RectTransform>();
			rt.SetParent(scrollArea.transform);
			//rt.anchoredPosition = new Vector2(-150,200-row*31);
			//Debug.Log("Button not null");
			foreach (Transform t in buttons[row].GetComponentsInChildren<Transform>()){
					if (t.name ==  "icon") {							
						var icon = t.GetComponent<Image>();
						icon.sprite = Resources.Load<Sprite>(item.portrait);
					} else if (t.name ==  "Text") {
						var text = t.GetComponent<Text>();
						text.text = item.Name;
					}
				}
			//setup events
			/*var b = buttons[row].GetComponent<Button>();
			b.onClick.AddListener(() => MouseClick(buttons[row]));*/
			//SetupEvents(buttons[row]);	
			row++;						
			}
	}
	
	// Update is called once per frame
	void Update () {
			
		

	}

	void SetupEvents(GameObject widget){


		//Get the event trigger attached to the UI object
		EventTrigger eventTrigger = widget.AddComponent<EventTrigger>();
		
		//Create a new entry. This entry will describe the kind of event we're looking for
		// and how to respond to it
		EventTrigger.Entry ButtonDown = new EventTrigger.Entry();
		
		//This event will respond to a drop event
		ButtonDown.eventID = EventTriggerType.PointerDown;
		
		//Create a new trigger to hold our callback methods
		ButtonDown.callback = new EventTrigger.TriggerEvent();
		
		//Create a new UnityAction, it contains our DropEventMethod delegate to respond to events
		UnityEngine.Events.UnityAction<BaseEventData> callback =
			new UnityEngine.Events.UnityAction<BaseEventData>(MouseClick);
		
		//Add our callback to the listeners
		ButtonDown.callback.AddListener(callback);
		
		//Add the EventTrigger entry to the event trigger component
		eventTrigger.triggers.Add(ButtonDown);
		

		//enter event
		EventTrigger.Entry enter = new EventTrigger.Entry();
		
		//This event will respond to a drop event
		enter.eventID = EventTriggerType.PointerEnter;
		
		//Create a new trigger to hold our callback methods
		enter.callback = new EventTrigger.TriggerEvent();
		
		//Create a new UnityAction, it contains our DropEventMethod delegate to respond to events
		UnityEngine.Events.UnityAction<BaseEventData> enterCallback =
			new UnityEngine.Events.UnityAction<BaseEventData>(MouseEnter);
		
		//Add our callback to the listeners
		enter.callback.AddListener(enterCallback);
		
		//Add the EventTrigger entry to the event trigger component
		eventTrigger.triggers.Add(enter);
	}

	//handles mouse clicks (left and right)
	void MouseClick(BaseEventData data){
		Debug.Log("Button '" + data.selectedObject.name + "' pressed!");
		//Debug.Log("Equip item '" + i.Name);
	}

	//mouse enters element
	public void MouseEnter(UnityEngine.EventSystems.BaseEventData baseEvent){
		Debug.Log(baseEvent.selectedObject.name + " triggered an enter event!");
	}

	//Create an event delegate that will be used for creating methods that respond to events
	public delegate void EventDelegate(UnityEngine.EventSystems.BaseEventData baseEvent);


}
