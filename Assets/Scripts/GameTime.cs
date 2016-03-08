using UnityEngine;
using System.Collections;

public class GameTime {
	public int day=3, hour=18, minute=37;
	public float SecsPerMinute = 1.0f;		//Game minute duration, in seconds
	float counter = 0;
	bool lightsOn = false;
	float sunAngle = 0;
	float moonAngle = 0;

	public void Adjust() {
		var sun = GameObject.Find("Sun");
		Debug.Log("Sun angle");
		sunAngle = 0.25f*(hour*60+minute)-90;
		if (hour > 19 || hour < 7) {
			//it is night
			sun.GetComponent<Light> ().enabled = false;
			RenderSettings.ambientLight = new Color(0.5f, 0.55f, 0.6f); 
		}
		moonAngle = sunAngle - 180;
		//Debug.Log(sunAngle);
		sun.transform.rotation = Quaternion.Euler(sunAngle,0,0);
		var moon = GameObject.Find("Moon");
		moon.transform.rotation = Quaternion.Euler(moonAngle,0,0);
		if (hour > 6 && hour < 20) {
			moon.GetComponent<Light> ().enabled = false;
			RenderSettings.ambientLight = new Color(0.7f, 0.75f, 0.8f); 
		}
	}

	// Update is called once per frame
	public void Update (float elapsed) {
		GameObject sun;
		GameObject moon;
		counter += elapsed;
		if (counter > SecsPerMinute) {
			counter = 0;
			minute++;
			if (minute==60) { //hour change
				minute = 0;
				hour++;
				if (hour==24) {
					hour = 0;
					day++;
				}
				Debug.Log("hour change");
				Debug.Log(hour);
				if (hour == 7) {
					sun = GameObject.Find("Sun");
					sun.GetComponent<Light>().enabled = true;
					sun.GetComponent<Light>().color = new Color(0.1f, 0.1f, 0.2f);
					RenderSettings.ambientLight = new Color(0.7f, 0.75f, 0.8f); 
					moon = GameObject.Find("Moon");
					moon.GetComponent<Light>().enabled = false;
				}
				if (hour == 20) {
					sun = GameObject.Find("Sun");
					sun.GetComponent<Light>().enabled = false;
					RenderSettings.ambientLight = new Color(0.5f, 0.55f, 0.6f); 
					moon = GameObject.Find("Moon");
					moon.GetComponent<Light>().enabled = true;
				}

			}
			//adjust sun
			sun = GameObject.Find("Sun");
			moon = GameObject.Find("Moon");
			//Debug.Log("Sun angle");
			sunAngle = 0.25f*(hour*60+minute)-180;
			moonAngle = sunAngle - 180;
			//Debug.Log(sunAngle);
			sun.transform.rotation = Quaternion.Euler(sunAngle,0,0);
			moon.transform.rotation = Quaternion.Euler(moonAngle,0,0);
		}
		//if (hour > 18 || hour <7 && !lightsOn)
		//	toggleNightLights (true);
		if (hour > 6 && hour < 20) {
			toggleNightLights (false);

		} else toggleNightLights (true);
	}

	/*
	 * Advance time X minutes
	 * param m minutes to increase
	 */
	public void AdvanceMinutes(int m) {
	}

	/*
	 * Advance time X hours
	 * param h hours to increase
	 */
	public void AdvanceHours(int h) {
		hour += h;
		Adjust ();
	}

	public void toggleNightLights(bool toggle){
		/*if (lightsOn == toggle)
			return;
		else 
			lightsOn = toggle;*/

		var lights = GameObject.FindGameObjectsWithTag ("NightLight");

		foreach (var lightObj in lights) {
			//set lights on/off
			lightObj.GetComponentInChildren<Light>().enabled = toggle;
		}

		var fires = GameObject.FindGameObjectsWithTag ("Fire");
		
		foreach (var firePart in fires) {
			//set lights on/off
			//fires.GetComponentInChildren<>().enabled = toggle;
			//Debug.Log(firePart.name);
			//firePart.SetActive(toggle);
			firePart.GetComponent<ParticleSystem>().enableEmission = toggle;
		}
		/*if (toggle == true) {
			//set sun to black
			var sun = GameObject.Find("Sun");
			if (sun) {
				sun.GetComponent<Light>().color = new Color(0.1f, 0.1f, 0.2f);
				//RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.3f);
				RenderSettings.ambientLight = new Color(0.5f, 0.55f, 0.6f); 
			}
		} else {
			//set sun to yellow
			var sun = GameObject.Find("Sun");
			if (sun) {
				sun.GetComponent<Light>().color = new Color(1.0f, 0.95f, 0.85f);
				RenderSettings.ambientLight = new Color(0.7f, 0.75f, 0.8f); 
			}
		}*/
	}
}
