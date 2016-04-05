using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int r = Random.Range (1, 4);
		Debug.Log (r);
		if (r==1) 
			gameObject.GetComponent<Text> ().text = "If you are a talker, try to make others do the dirty work for you";
		else if (r==2) 
			gameObject.GetComponent<Text> ().text = "We ran out of tips";
		else if (r==3) 
			gameObject.GetComponent<Text> ().text = "A wise (or less dumb) warrior once said: it is better to give than receive";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
