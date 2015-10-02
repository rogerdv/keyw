using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateUI : MonoBehaviour {
	// Available characterpieces, for characvter customization
	public string[] heads;
	public string[] mtorsos;
	public string[] ftorsos;
	public string[] legs;
	int torsocount;
	int legcount;
	public string[] parts; //current selected body parts
	public string prefab;	//prefab to use to build player
	GameObject player;
	int[] baseStats;
	int statPoints = 5;

	//labels 
	public GameObject strLab;
	public GameObject dexLab;
	public GameObject intLab;
	public GameObject conLab;
	public GameObject charLab;
	public GameObject generalLabel;

	// Use this for initialization
	void Start () {
		parts = new string[5];
		mtorsos = new string[2];
		ftorsos = new string[2];
		baseStats = new int[5];
		for (int i=0; i<5; i++)
			baseStats [i] = 10;
		mtorsos[0] = "Characters/vmale-torso";
		mtorsos[1] = "Characters/vmale-torso-fat";
		ftorsos[0] = "Characters/fmale-torso";
		ftorsos[1] = "Characters/vmale-torso-fat";
		legs  = new string[2];	
		legs[0] = "Characters/vmale-legs";
		legs[1] = "Armors/iron_legs";
		parts[0] = "Characters/vmale-head";
		parts [1] = mtorsos[0];
		parts [2] = "Characters/vmale-hands";
		parts [3] = legs[0];
		parts [4] = "Characters/vmale-feet";
		torsocount = 0;
		legcount = 0;
		var playerPos = GameObject.Find ("player");
		prefab = "Characters/male-player-pref";
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		GameInstance.BuildCharacter (player, parts); 
		player.transform.position = playerPos.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			if(Input.GetAxis("Mouse X")>0) {
				player.transform.Rotate(0,-320*Time.deltaTime,0);
			}
			if(Input.GetAxis("Mouse X")<0) {
				player.transform.Rotate(0,320*Time.deltaTime,0);
			}
		}	//if 
		
	}
	
	public void SetGenreMale() {
		parts[0] = "Characters/vmale-head";
		parts [1] = mtorsos[torsocount];
		parts [2] = "Characters/vmale-hands";
		parts [3] = legs[legcount];
		parts [4] = "Characters/vmale-feet";
		prefab = "Characters/male-player-pref";
		//reset
		Destroy(player);
		var playerPos = GameObject.Find ("player");
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		GameInstance.BuildCharacter (player, parts); 
		player.transform.position = playerPos.transform.position;
	}

	public void SetGenreFemale() {
		parts[0] = "Characters/vfemale-head";
		parts [1] = "Characters/vfemale-torso";
		parts [2] = "Characters/vfemale-hands";
		parts [3] = "Characters/vfemale-legs";
		parts [4] = "Characters/vfemale-feet";
		prefab = "Characters/female-player-pref";
		//reset
		Destroy(player);
		var playerPos = GameObject.Find ("player");
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		GameInstance.BuildCharacter (player, parts); 
		player.transform.position = playerPos.transform.position;
	}

	public void StartGame()
	{
		GameInstance.meshes = parts;
		GameInstance.prefab = prefab;
		GameInstance.stats = baseStats;
		Destroy (player);
		Application.LoadLevel("Calesoni_Castle");
	}

	public void NextTorso() {
		if (torsocount == 0)
			torsocount = 1;
			else torsocount =0;
		parts [1] = mtorsos[torsocount];
		//rebuild character
		Destroy(player);
		var playerPos = GameObject.Find ("player");
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		GameInstance.BuildCharacter (player, parts); 
		player.transform.position = playerPos.transform.position;
	}

	public void NextLegs() {
		if (legcount == 0)
			legcount = 1;
		else legcount =0;
		parts [3] = legs[legcount];
		//rebuild character
		Destroy(player);
		var playerPos = GameObject.Find ("player");
		GameObject playerPrefab = Resources.Load(prefab) as GameObject;
		player = Instantiate (playerPrefab) as GameObject;
		GameInstance.BuildCharacter (player, parts); 
		player.transform.position = playerPos.transform.position;
	}

	public void PrevRace(){
	}

	public void NextRace() { 
	}

	//stat buttons
	public void IncStr(){
		if (statPoints > 0) {
			statPoints--;
			baseStats[(int)Attributes.Str]++;
			//update label
			strLab.GetComponent<Text>().text = baseStats[(int)Attributes.Str].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void DecStr(){
		if (baseStats[(int)Attributes.Str] > 0) {
			statPoints++;
			baseStats[(int)Attributes.Str]--;
			//update label
			strLab.GetComponent<Text>().text = baseStats[(int)Attributes.Str].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void DecDext(){
		if (baseStats[(int)Attributes.Dext] > 0) {
			statPoints++;
			baseStats[(int)Attributes.Dext]--;
			//update label
			dexLab.GetComponent<Text>().text = baseStats[(int)Attributes.Dext].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void IncDext(){
		if (statPoints > 0) {
			statPoints--;
			baseStats[(int)Attributes.Dext]++;
			//update label
			dexLab.GetComponent<Text>().text = baseStats[(int)Attributes.Dext].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void DecInt(){
		if (baseStats[(int)Attributes.Int] > 0) {
			statPoints++;
			baseStats[(int)Attributes.Int]--;
			//update label
			intLab.GetComponent<Text>().text = baseStats[(int)Attributes.Int].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}
	
	public void IncInt(){
		if (statPoints > 0) {
			statPoints--;
			baseStats[(int)Attributes.Int]++;
			//update label
			intLab.GetComponent<Text>().text = baseStats[(int)Attributes.Int].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void DecConst(){
		if (baseStats[(int)Attributes.Const] > 0) {
			statPoints++;
			baseStats[(int)Attributes.Const]--;
			//update label
			conLab.GetComponent<Text>().text = baseStats[(int)Attributes.Const].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}
	
	public void IncConst(){
		if (statPoints > 0) {
			statPoints--;
			baseStats[(int)Attributes.Const]++;
			//update label
			conLab.GetComponent<Text>().text = baseStats[(int)Attributes.Const].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}

	public void DecChar(){
		if (baseStats[(int)Attributes.Char] > 0) {
			statPoints++;
			baseStats[(int)Attributes.Char]--;
			//update label
			charLab.GetComponent<Text>().text = baseStats[(int)Attributes.Char].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}
	
	public void IncChar(){
		if (statPoints > 0) {
			statPoints--;
			baseStats[(int)Attributes.Char]++;
			//update label
			charLab.GetComponent<Text>().text = baseStats[(int)Attributes.Char].ToString();
			generalLabel.GetComponent<Text>().text = "Attribute points available: "+statPoints.ToString();
		}
	}
}
