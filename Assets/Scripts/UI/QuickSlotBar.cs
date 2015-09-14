using UnityEngine;
using System.Collections;

public class QuickSlotBar : MonoBehaviour {
	public Texture2D backg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI () {
		GUIContent bg = new GUIContent();
		bg.image = backg;
		//GUI.BeginGroup(new Rect(Screen.width/2-210, Screen.height-45, 800, 43), bg);
		/*for (int i=0;i<=9;i++) {
			GUI.Button(new Rect(0+i*42,0,42,42),"slot");
		}*/
		//GUI.EndGroup ();
	}
}
