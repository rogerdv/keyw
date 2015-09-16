using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryWindow : MonoBehaviour {
	public GameObject invGridPrefab;
	GameObject player;
	GameObject ghost;		//clone of the widget being dragged

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		var inv = gameObject;
			
			//create the list
			var scrollArea = GameObject.Find("ScrollArea");		
			
			var InvList = player.GetComponent<BaseCharacter> ().inventory;
			int row = 0;
			foreach (var item in InvList) {
				Debug.Log("Item "+item.Name);
				var go = Instantiate(invGridPrefab);
				go.name =  "Button_"+item.Name;
				
				var rt = go.GetComponent<RectTransform>();
				rt.SetParent(scrollArea.transform);
				rt.anchoredPosition = new Vector2(-100,140-row*31);
				//Debug.Log("Button not null");
				foreach (Transform t in go.GetComponentsInChildren<Transform>()){
					if (t.name ==  "icon") {							
						var icon = t.GetComponent<Image>();
						icon.sprite = Resources.Load<Sprite>(item.portrait);
					} else if (t.name ==  "name") {
						var text = t.GetComponent<Text>();
						text.text = item.Name;
					}
				}
				row++;
				//setup events
				/*var b = go.GetComponent<Button>();
					b.onClick.AddListener(() => MouseClick(go));*/
				SetupEvents(go);
				
			}
			

	}
	
	// Update is called once per frame
	void Update () {
			
		

	}

	void SetupEvents(GameObject widget){

		//Get the event trigger attached to the UI object
		EventTrigger eventTrigger = widget.GetComponent<EventTrigger>();
		
		//Create a new entry. This entry will describe the kind of event we're looking for
		// and how to respond to it
		EventTrigger.Entry drag = new EventTrigger.Entry();
		
		//This event will respond to a drop event
		drag.eventID = EventTriggerType.Drag;
		
		//Create a new trigger to hold our callback methods
		drag.callback = new EventTrigger.TriggerEvent();
		
		//Create a new UnityAction, it contains our DropEventMethod delegate to respond to events
		UnityEngine.Events.UnityAction<BaseEventData> callback =
			new UnityEngine.Events.UnityAction<BaseEventData>(DragEventMethod);
		
		//Add our callback to the listeners
		drag.callback.AddListener(callback);
		
		//Add the EventTrigger entry to the event trigger component
		eventTrigger.triggers.Add(drag);
		
		//Begin drag
		EventTrigger.Entry begindrag = new EventTrigger.Entry();
		
		//This event will respond to a drop event
		begindrag.eventID = EventTriggerType.BeginDrag;
		
		//Create a new trigger to hold our callback methods
		begindrag.callback = new EventTrigger.TriggerEvent();
		
		//Create a new UnityAction, it contains our DropEventMethod delegate to respond to events
		UnityEngine.Events.UnityAction<BaseEventData> bd_callback =
			new UnityEngine.Events.UnityAction<BaseEventData>(BeginDragEventMethod);
		
		//Add our callback to the listeners
		begindrag.callback.AddListener(bd_callback);
		
		//Add the EventTrigger entry to the event trigger component
		eventTrigger.triggers.Add(begindrag);
	}

	void MouseClick(GameObject obj){
		Debug.Log("Button '" + obj.name + "' pressed!");
		//Debug.Log("Equip item '" + i.Name);
	}

	public void MouseEnter(UnityEngine.EventSystems.BaseEventData baseEvent){
		Debug.Log(baseEvent.selectedObject.name + " triggered an enter event!");
	}

	//Create an event delegate that will be used for creating methods that respond to events
	public delegate void EventDelegate(UnityEngine.EventSystems.BaseEventData baseEvent);

	public void BeginDragEventMethod(UnityEngine.EventSystems.BaseEventData baseEvent) {
		Debug.Log(baseEvent.selectedObject.name + " triggered a bgin drag event!");
		//create a copy
		ghost = Instantiate (baseEvent.selectedObject);
	}

	public void DragEventMethod(UnityEngine.EventSystems.BaseEventData baseEvent) {
		Debug.Log(baseEvent.selectedObject.name + " triggered a drag event!");

		//fade it
		//ghost.GetComponent<Image> ().C = 0.5f;
		//baseEvent.selectedObject is the GameObject that triggered the event,
		// so we can access its components, destroy it, or do whatever.
	}


}
