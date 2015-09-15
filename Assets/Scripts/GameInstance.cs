using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#region TransformCatalog
public class TransformCatalog : Dictionary<string, Transform>
{
	#region Constructors
	public TransformCatalog(Transform transform)
	{
		Catalog(transform);
	}
	#endregion
	
	#region Catalog
	private void Catalog(Transform transform)
	{
		Add(transform.name, transform);
		foreach (Transform child in transform)
			Catalog(child);
	}
	#endregion
}
#endregion


#region DictionaryExtensions
public class DictionaryExtensions
{
	public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
	{
		TValue value;
		source.TryGetValue(key, out value);
		return value;
	}
}
#endregion

/**
 * Manages all game info, instancing character, NPCs, etc
 * */
public class GameInstance : MonoBehaviour {

	public static bool pause;
	public Texture2D crNormal;	//< Normal cursor

	public static GameObject player;
	public static BaseCharacter pcScript;

	static ItemFactory itFactory;
	GameObject SceneInf;
	public static string[] BodyParts;
	public static string[] meshes;	//mesh files for body parts
	public static string prefab;	//prefab to use to build player
	GameTime clock;		//game clock
	//Dialog dlgWin;
	bool displayPortrait=false;	//true if entity selected, to display the portrait
	Texture2D portrait;
	static GameObject selected; 
	static public GameObject Selected {
		get { return selected; }
		set { selected = value; }
	}
	public static bool clicks = true;		//false if dialog or inventory windows are displayed

	//ui elements
	public GameObject dlgPrefab;


	void Awake() {
		DontDestroyOnLoad (this);

	}

	// Use this for initialization
	void Start () {
		pause = false;
		BodyParts = new string[5];
		BodyParts[0] = "head";
		BodyParts[1] = "torso";
		BodyParts[2] = "hands";
		BodyParts[3] = "legs";
		BodyParts[4] = "feet";
		meshes = new string[5];


		clock = new GameTime ();
		Cursor.SetCursor (crNormal, Vector2.zero, CursorMode.Auto);
		itFactory = new ItemFactory (); 
		itFactory.Init("Items/items");
	}


	void OnLevelWasLoaded(int level){
		pause = false;
		SceneInf = GameObject.Find ("SceneInfo");
		if (SceneInf != null) {
			if (player==null)
				InstancePlayer ();
			else {
				//find player marker
				var playerPos = GameObject.Find ("player");
				player.transform.position = playerPos.transform.position;
			}
		} 

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			pause = !pause;
		}
		if (SceneInf != null) {
			if (!pause) {
				clock.Update (Time.deltaTime);
			}		

			if (Input.GetKey (KeyCode.Mouse0) && clicks) {
				if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {				
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000)) {
						if (hit.collider.gameObject.tag == "NPC") {	//an NPC was clicked
							//is some NPC selected?
							if (selected) {
								selected.GetComponentInChildren<Projector>().enabled  = false;
							}
							selected = hit.collider.gameObject;
							var npcsc = selected.GetComponent<NPC> ();
							//display NPC information
							///@TODO: Migrate OnGUI based stuff to new UI system
							MsgList.SetText ("Clicked on " + npcsc.VisibleName);
							displayPortrait = true;
							portrait = npcsc.portrait;

							pcScript.target = selected;
							//enable projector to display the selection marker
							selected.GetComponentInChildren<Projector>().enabled  = true;
							if (Vector3.Distance (selected.transform.position, player.transform.position) < 5.0f) { //close enough to talk?
								/*Dialog.Load (pcScript, "Dialogs/" + npcsc.dialog);
								Dialog.ToggleDialog ();*/
								//instantiate dialog
								var dlgWindow = Instantiate(dlgPrefab);
								var canvas = GameObject.Find("Canvas");
								//var rt = dlgWindow.GetComponent<RectTransform>(); 
								//rt.SetParent(canvas.GetComponent<RectTransform>());
								dlgWindow.transform.SetParent(canvas.transform, false);
								//rt.anchoredPosition = new Vector2(10,-400);
								//clicks = false;
							} else 
								MsgList.SetText ("You are too far to talk.");
						} else {
							pcScript.MoveTo (new Vector3 (hit.point.x, hit.point.y, hit.point.z));
						}
					}
				}
			} else if (Input.GetKeyDown(KeyCode.Keypad5)) { //reset camera to player position
				var camObj = GameObject.Find("target");
				camObj.transform.position = player.transform.position;
			} else if (Input.GetKey (KeyCode.Mouse1) && clicks) {		//right click unselects
				if (selected) {
					selected.GetComponentInChildren<Projector>().enabled  = false;		//disable projector, turn off selection marker
					selected = null;
					pcScript.target = null;
				}
			} //if Input events
		}
			
	}

	public static void InstancePlayer() {
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		pcScript = player.GetComponent<BaseCharacter> ();
		//player.transform.localPosition = Vector3.zero;
		player.transform.localRotation = Quaternion.identity;
		var playerPos = GameObject.Find ("player");
		player.transform.localPosition = playerPos.transform.position;
		//player.transform.rotation = playerPos.transform.rotation;
		BuildCharacter (player, meshes);

	}

	public static void BuildCharacter(GameObject parentGO, string[] pieces) { 
		GameObject temp;		//temporary game object to hold pieces
		GameObject[] body = new GameObject[5];
		var boneCatalog = new TransformCatalog(parentGO.transform);
		for (int i=0; i<=4; i++) {
			var obj = Resources.Load(pieces[i]);
			temp = Instantiate(obj) as GameObject;
			body[i] = new GameObject(BodyParts[i]);
			body[i].transform.SetParent(parentGO.transform);
			body[i].transform.localPosition = Vector3.zero;
			//body[i].transform.localRotation =  Quaternion.Euler(Vector3.zero);
			body[i].transform.localScale = parentGO.transform.localScale;
			//copy skinned mesh renderer components
			CopySkinnedMeshRenderer(temp, body[i], boneCatalog);
			Destroy(temp);
		}
	}

	public static void CopySkinnedMeshRenderer(GameObject source, GameObject target, TransformCatalog bones) {
		var skinnedMeshRenderers = source.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (var sourceRenderer in skinnedMeshRenderers)
		{
			var comp = target.AddComponent<SkinnedMeshRenderer>();
			comp.sharedMesh = sourceRenderer.sharedMesh;
			comp.materials = sourceRenderer.materials;
			comp.bones = TranslateTransforms(sourceRenderer.bones, bones);
		}
	}

	public static Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog)
	{
		var targets = new Transform[sources.Length];
		for (var index = 0; index < sources.Length; index++)
			targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
		return targets;
	}

	void OnGUI() {
		if (displayPortrait) {
			//GUI.DrawTexture(new Rect(150, 5, 128,128), portrait);
			//GUI.Label(new Rect(150,135,128,25),selectedName);
		}
	}

	//// Getters/Setters

	static public ItemFactory ItFactory{
		get {return itFactory;}
	}


	//Events

	void MouseEnter(GameObject widget){
		Debug.Log("mouse is over me!");
	}

	void Drag(GameObject widget) {
		widget.transform.position = Input.mousePosition;
	}
}
