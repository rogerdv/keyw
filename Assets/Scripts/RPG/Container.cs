using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour {
	public int LockLevel;		//0 for unlocked

	void OnMouseDown(){
		//create an item
		var item = GameInstance.ItFactory.RandomItem ();
		Debug.Log (item.Name);
	}
}
