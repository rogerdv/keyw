using UnityEngine;
using System.Collections;

// Destroy the gameObject or component after a timer
public class SetLifeSpawn : MonoBehaviour {
	
	// Object can be a GameObject or a component
	public Object me;
	public float timer;
	
	void Start(){
		// Default is the gameObject
		if (me == null)
			me = gameObject;
		
		// Destroy works with GameObjects and Components
		Destroy(me, timer);
	}
}
