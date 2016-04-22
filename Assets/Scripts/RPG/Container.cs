using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour {
	public int LockLevel;		//0 for unlocked
	bool used = false;

	void OnMouseDown(){
		//if (used) 
			//return;
		//create an item
		var item = GameInstance.ItFactory.RandomItem ();
		var player = GameInstance.player.GetComponent<BaseCharacter> ();
		bool IndexExists = true;
		while(IndexExists) {
			int idx = UnityEngine.Random.Range(100,10000);
			if (!player.inventory.ContainsKey(idx)) {
				player.inventory[idx]=item;
				IndexExists = false;
			}
		} //while
		used = true;
		Debug.Log (item.Name);
	}
}
