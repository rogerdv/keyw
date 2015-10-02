using UnityEngine;
using System.Collections;

/**
 * Basic scene laod trigger
*/

public class LoadScene : MonoBehaviour {

	public Texture2D crNormal;	//< Normal cursor
	public Texture2D crHover;	//< Hover cursor
	public string level;	//Scene to load
	public string location;	//Destination coordinates. If none, player marker will be used

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		//restore cursor
		Cursor.SetCursor (crNormal, Vector2.zero, CursorMode.Auto);
		//deactivate scene NPCs
		GameInstance.DeactivateEntities ();
		//load next scene
		Application.LoadLevel(level);	
		var playerGO = GameObject.FindGameObjectWithTag ("Player");
		var destGO = GameObject.Find (location); 
		playerGO.transform.position = destGO.transform.position;
	}

	void OnMouseEnter() {
		Cursor.SetCursor (crHover, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseExit() {
		Cursor.SetCursor (crNormal, Vector2.zero, CursorMode.Auto);
	}
}
