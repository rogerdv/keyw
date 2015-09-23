#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

public class DialogEditor : EditorWindow {
	
	public static DialogEditor window;
	public static Dialog dlg;
	public GameObject ButtonPref;
	private GameObject obj;
	private NPC script; 
	string status;		//status message, in the bottom of the window
	
	[MenuItem ("Tools/DialogEditor")]
	public static void OpenWindow(){

		window = (DialogEditor)EditorWindow.GetWindow(typeof(DialogEditor)); //create a window
		window.title = "Dialog Editor"; //set a window title

	}

	void OnGUI(){
		if(window == null)
			OpenWindow();
		window.maximized = true;
		if(GUI.Button(new Rect(0, 0, 45, 20), "Load")){

			Load();
		}
		if(GUI.Button(new Rect(50, 0, 45, 20), "Save")){
			Debug.Log("Save dialog");
		}
		if (Selection.activeGameObject != null) {
			//gets the object you currently have selected in the scene view
			obj = Selection.activeGameObject;
			script = obj.GetComponent<NPC>();
			if (script.dialog=="") 
				GUI.Label(new Rect(5, 22,160,15), "The entity has no dialog. Please set a dialog name and reopen this editor");

		} else	{
			GUI.Label(new Rect(5, 22,160,15), "No entity selected!");
		}

	}

	void Load(){
		Debug.Log("Load dialog "+script.dialog);
	}

	void Save() {

	}
}
#endif