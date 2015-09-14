using UnityEngine;
using System.Collections;

public class InventoryUIEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * TRiggered on mouse over.
	 * Displays item info
	 * */
	public void MouseOver(){
		Debug.Log("mouse is over me!");
	}

	/**
	 * trigger on drop
	 * */
	public void Drop(){
		Debug.Log("I was dragged and dropped");

	}
}
