using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityStandardAssets.ImageEffects;

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
public class GameInstance : Singleton<GameInstance> {

	public static bool pause;
	public Texture2D crNormal;	//< Normal cursor

	public static GameObject player;
	public static BaseCharacter pcScript;

	static ItemFactory itFactory;
	GameObject SceneInf;
	public static string[] BodyParts;
	public static string[] meshes;	//mesh files for body parts
	public static string prefab = "male-player-pref";	//prefab to use to build player
	public static int[] stats = {11,11,11,11,11};				//the base stats to create the character
	GameTime clock;		//game clock
	//Dialog dlgWin;
	//bool displayPortrait=false;	//true if entity selected, to display the portrait

	static Dictionary<string, List<GameObject>> entities; 	//list of scene entities

	static GameObject selected; 
	static public GameObject Selected {
		get { return selected; }
		set { selected = value; }
	}
	public static bool clicks = true;		//false if dialog or inventory windows are displayed
	bool areaMode = false;		//Area target select mode

	static public GameSettings options;


	//ui elements
	public GameObject dlgPrefab;
	GameObject dlgWindow;
	public GameObject invPrefab;
	GameObject inv;
	public GameObject optPrefab;
	GameObject opts;		//options window
	public GameObject listPrefab;
	GameObject MsgBox;
	public GameObject portraitPrefab;
	GameObject portrait;

	//other prefabs required
	public GameObject areaProjector;		//Area marker projector
	GameObject marker;


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

		entities = new Dictionary<string, List<GameObject>> ();

		clock = new GameTime ();
		Cursor.SetCursor (crNormal, Vector2.zero, CursorMode.Auto);
		options = new GameSettings ();
		//options.Load ();		//init config
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
				player.transform.position = new Vector3(playerPos.transform.position.x, playerPos.transform.position.y,playerPos.transform.position.z);

			}

			MsgBox = Instantiate(listPrefab);
			MsgBox.name = "MessageBox";
			var canvas = GameObject.Find("Canvas");
			MsgBox.transform.SetParent(canvas.transform, false);
			if (!options.shadows) {	//disable shadows, if needed
				Debug.Log("Shadows are off!!!");
				foreach( var light in FindObjectsOfType<Light>()) {
					light.shadows = LightShadows.None;
				}//foreach
			} //if shadows
			if (!options.ssao) { //disable ssao component
				var cam = GameObject.FindGameObjectWithTag("MainCamera");
				cam.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = false;
			}
			if (!options.fxaa) { //disable ssao component
				var cam = GameObject.FindGameObjectWithTag("MainCamera");
				cam.GetComponent<Antialiasing>().enabled = false;
			}
			if (!options.bloom) { //disable ssao component
				var cam = GameObject.FindGameObjectWithTag("MainCamera");
				cam.GetComponent<Bloom>().enabled = false;
			}
			ReactivateEntities();
			clock.Adjust();
		} //if SceneInfo

	}

	public static void DeactivateEntities() {
		var ents = GameObject.FindGameObjectsWithTag("NPC");
		string level = Application.loadedLevelName;
		if (!entities.ContainsKey(level)) {
			entities[level] = new List<GameObject>();
		}
		foreach (GameObject e in ents) {
			e.SetActive(false);
			entities[level].Add(e);
		}
	}
	
	public void ReactivateEntities() {
		string level = Application.loadedLevelName;
		if (entities.ContainsKey (level)) {
			foreach (GameObject g in entities[level]) {
				g.SetActive (true);
			}
		} //else, the level have never been loaded, nothing to reactivate
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		var UserDir = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		FileStream file = File.Open(UserDir + "/keyw/quicksave.save", FileMode.Create);

		//todo: save level, game time, etc
		bf.Serialize(file, Application.loadedLevelName);
		pcScript.SaveToFile (file);
		int NumNPC = entities.Count;
		//get total number of 
		foreach (KeyValuePair<string, List<GameObject>> l in entities) {
			NumNPC += l.Value.Count;
		}
		bf.Serialize(file, NumNPC);
		file.Close();
	}

	public void Load() {
		BinaryFormatter bf = new BinaryFormatter();
		var UserDir = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		FileStream file = File.Open(UserDir + "/keyw/quicksave.save", FileMode.Open);
		string level = (string)bf.Deserialize (file);
		Debug.Log (level);

	}
	
	// Update is called once per frame
	void Update () {


		if (SceneInf != null) {
			if (!pause) {
				clock.Update (Time.deltaTime);
			}		
			if (Input.GetKeyDown (KeyCode.Space)) {
				pause = !pause;
			} else if (Input.GetKeyDown(KeyCode.Escape)) {
				if (inv)
					Destroy(inv);
				if (opts) {
					pause = false;
					Destroy(opts);
				}
				if (areaMode) {
					Destroy(marker);
					areaMode = false;
				}
			} else if (Input.GetKeyDown (KeyCode.Mouse0) && clicks) { //left  click
				if (areaMode) {
					RaycastHit hit;					
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000)) {
						int layerMask = 1 << 9;
						// marker.GetComponent<Projector>().orthographicSize*2
						var colliders = Physics.OverlapSphere (marker.transform.position,marker.GetComponent<Projector>().orthographicSize, layerMask);
						foreach (var c in colliders) {
							Debug.Log(c.name);
						}
					}
					Destroy(marker);
					areaMode = false;
				} else if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {				
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
							MsgBox.GetComponent<MsgList>().SetText("Clicked on " + npcsc.Name);
							if (portrait!=null)
								Destroy(portrait);
							portrait = Instantiate(portraitPrefab);
							var canvas = GameObject.Find("Canvas");
							portrait.transform.SetParent(canvas.transform, false);
							portrait.GetComponent<Image>().sprite = npcsc.portrait;

							pcScript.target = selected;
							//enable projector to display the selection marker
							selected.GetComponentInChildren<Projector>().enabled  = true;
							//calculate hp bar size
							float percent = npcsc.HitPoints[0]/npcsc.HitPoints[1];
							foreach (Transform t in portrait.GetComponentsInChildren<Transform>()) {
								if (t.name == "HP") {
									t.localScale = new Vector3(percent, 1.0f,1.0f);
								} else if (t.name == "level") {
									t.GetComponent<Text>().text = npcsc.level.ToString();
									if (npcsc.level>pcScript.level+2) {//set color red
										t.GetComponent<Text>().color = Color.red;
									} else if (npcsc.level>pcScript.level-2) {
										t.GetComponent<Text>().color = Color.white;
									} else
										t.GetComponent<Text>().color = Color.blue;
								}
							}
							if (Vector3.Distance (selected.transform.position, player.transform.position) < 3.0f) { //close enough to talk?
								//instantiate dialog
								dlgWindow = Instantiate(dlgPrefab);
								dlgWindow.transform.SetParent(canvas.transform, false);
							} else 
								MsgBox.GetComponent<MsgList>().SetText ("You are too far to talk.");
						} else {
							pcScript.MoveTo (new Vector3 (hit.point.x, hit.point.y, hit.point.z));
						}
					}
				}
			} else if (Input.GetKeyDown(KeyCode.Keypad5)) { //reset camera to player position
				var camObj = GameObject.Find("target");
				camObj.transform.position = player.transform.position;
			} else if (Input.GetKeyDown(KeyCode.I)) { //show inventory
				if (inv) {
					Destroy(inv);
				} else {
					inv = Instantiate(invPrefab);
					var canvas = GameObject.Find("Canvas");
					inv.transform.SetParent(canvas.transform, false);
				}
			}  else if (Input.GetKey (KeyCode.Mouse1) && clicks) {		//right click unselects
				if (selected) {
					selected.GetComponentInChildren<Projector>().enabled  = false;		//disable projector, turn off selection marker
					selected = null;
					pcScript.target = null;
					Destroy(portrait);
				}
				if (areaMode) {
					Destroy(marker);
					areaMode = false;
				}
			} else if (Input.GetKeyDown(KeyCode.Alpha1)) { //action slot 1
				//temporary hack: attack
				if (pcScript.target!=null) {
					pcScript.state = (int)CharacterState.Combat1h;
					var npcsc = pcScript.target.GetComponent<NPC>();
					var ai = pcScript.target.GetComponent<NPC>();
					npcsc.AIstate = (int)AIStates.Combat;
					npcsc.target = player;
					var prefab = Resources.Load("FloatingText") as GameObject;
					var t = Instantiate (prefab) as GameObject;
					RectTransform r = t.GetComponent<RectTransform> ();
					t.transform.SetParent (pcScript.target.transform.FindChild ("MyCanvas"));
					r.transform.localPosition = prefab.transform.localPosition;
					r.transform.localScale = prefab.transform.localScale;
					r.transform.localRotation = prefab.transform.localRotation;
					Destroy (t, 2);
				} else {//no target
					MsgBox.GetComponent<MsgList>().SetText("You swing your weapon in the air and people looks at you as if you were crazy");
				}
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) { //action slot 1
				//temporary hack: area test
				areaMode = true;
				marker = Instantiate(areaProjector);
				RaycastHit hit;					
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 1000)) {
					marker.transform.position = new Vector3(hit.point.x, 1,hit.point.z);
				}
			} //if Input events
		}

		if (areaMode) {
			if (Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y")!=0) {
				RaycastHit hit;					
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 1000)) {
					marker.transform.position = new Vector3(hit.point.x, 1,hit.point.z);
				}
			} 
			Vector3 end = new Vector3(marker.transform.position.x+marker.GetComponent<Projector>().orthographicSize,1, marker.transform.position.z);
			Debug.DrawLine(marker.transform.position, end, Color.red);
			/*if(Input.GetAxis("Mouse X")>0) {
				marker.transform.Translate(8*Time.deltaTime,0,0);
			}
			if(Input.GetAxis("Mouse X")<0) {
				marker.transform.Translate(-8*Time.deltaTime,0,0);
			}
			if(Input.GetAxis("Mouse Y")>0) {
				marker.transform.Translate(0,8*Time.deltaTime, 0);
			}
			if(Input.GetAxis("Mouse Y")<0) {
				marker.transform.Translate(0,-8*Time.deltaTime, 0);
			}*/
			//Vector3 end = new Vector3(marker.transform.position.x+4,marker.transform.position.y, marker.transform.position.z);
			//Debug.DrawLine(marker.transform.position, end, Color.red, 1);
		}
		if (Input.GetKeyDown (KeyCode.O)) { //show options window
			if (opts) { 
				pause = false;	
				Destroy (opts);
			} else {
				pause = true;
				opts = Instantiate (optPrefab);
				var canvas = GameObject.Find ("Canvas");
				opts.transform.SetParent (canvas.transform, false);
			}
		} else if (Input.GetKeyDown (KeyCode.F5)) {
			//quicksave
			Save();
		} else if (Input.GetKeyDown (KeyCode.F9)) {
			//quickload
			Load();
		}
			
	}

	public static void InstancePlayer() {
		Debug.Log("prefab!!!! "+prefab);
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		pcScript = player.GetComponent<BaseCharacter> ();
		pcScript.SetAttributes (stats);
		//for (int i=0; i<5; i++)
		//	pcScript.attrib [i].baseValue = stats [i];
		//player.transform.localPosition = Vector3.zero;
		player.transform.localRotation = Quaternion.identity;
		var playerPos = GameObject.Find ("player");
		player.transform.localPosition = playerPos.transform.position;
		//player.transform.rotation = playerPos.transform.rotation;
		BuildCharacter (player, meshes);
		BaseSkill s = new BaseSkill();
		s.Name = "dodge";
		s.baseValue = 18;
		player.GetComponent<BaseCharacter>().skills.Add(s);
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
