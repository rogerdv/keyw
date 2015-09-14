using UnityEngine;
using System.Collections;

public class MsgList : MonoBehaviour {
	private static Vector2 scrollPosition;
	private static string text;

	// Use this for initialization
	void Start () {
		text = "";
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		GUILayout.BeginArea (new Rect(5, Screen.height-100, Screen.width-500, Screen.height-90));
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (200), GUILayout.Height (50));
		GUILayout.Label (text);
		GUILayout.EndScrollView ();
		GUILayout.EndArea();
	}

	public static void SetText(string Text){
		text+= Text + "\n";;
		// setting the "y" value of scrollPosition puts the scrollbar at the bottom
		scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
	}
}

