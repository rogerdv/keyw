using UnityEngine;
using System.Collections;


public class BaseAI : MonoBehaviour {

	protected NPC owner;

	// Use this for initialization
	void Start () {

		owner = gameObject.GetComponent<NPC> ();
	}



	// Update is called once per frame
	void Update () {
	
	}

	void State_Idle() {}

	void State_Combat() {}
}
