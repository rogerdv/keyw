using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	void Start() {

	}


	public void StartGame()
	{
		Application.LoadLevel("char-creation");
	}

	public void LoadGame() {
		//Application.LoadLevel("Calesoni_Castle");
		LevelManager.Load("Calesoni_Castle");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
