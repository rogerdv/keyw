using UnityEngine;
using System.Collections;

public class GameSettings {
	int DificultLevel;
	public bool shadows;
	public bool ssao;
	public int aa;

	/**
	 * Load from user saved preferences
	 * 
	*/	 
	bool Load() {
		return true;
	}

}
