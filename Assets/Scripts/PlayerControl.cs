using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour { 

	//public Vector3 position;
	public GameObject t;
	
    private NavMeshAgent agent;
	private Animator anim;

	//Transform myTransform;

	public Transform LabelTransform;
	public GameObject LabelPrefab;

	void Awake() {
		DontDestroyOnLoad (this);
	}

		
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		anim.SetInteger ("CharacterState", (int)CharacterState.Idle);
		//myTransform = transform;

	}

	void OnLevelWasLoaded(int level) {
		anim.SetInteger ("CharacterState", (int)CharacterState.Idle);
	}
	
	void OnCollisionEnter()
	{
		Debug.Log("Hit something");
	}

	public void MoveTo(Vector3 coord) {
		anim.SetInteger ("CharacterState", (int)CharacterState.Running);
		agent.SetDestination (coord);
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameInstance.pause) {
			if (!anim.enabled)
				anim.enabled = true;
			if (!agent.hasPath)
				anim.SetInteger ("CharacterState", (int)CharacterState.Idle);

			/*if (Input.GetKey (KeyCode.Mouse0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 1000)) {
					Vector3 position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
					//transform.LookAt(position);
					//myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(position,myTransform.position), 2 * Time.deltaTime);
					anim.SetInteger ("CharacterState", (int)CharacterState.Walking);
					agent.SetDestination (position);
					MsgList.SetText ("Moving to");
				}
			}	*/
			if (Input.GetKey (KeyCode.Mouse1)) {
				anim.SetInteger ("CharacterState", (int)CharacterState.AttackMelee1h);

			}
			if (Input.GetKey (KeyCode.C)) {
				anim.SetInteger ("CharacterState", (int)CharacterState.Combat1h);
			} 
			if (Input.GetKey (KeyCode.T)) {
				//attach tests
				GameObject wprefab = Resources.Load ("Weapons/ancient-grtsword") as GameObject;
				GameObject weapon = Instantiate (wprefab) as GameObject; 
				weapon.transform.SetParent (t.transform);
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = Quaternion.Euler (Vector3.zero);
				//CreateDamagePopup (200);
			}
		} else {
			anim.enabled =false;
		}
	}



	public void CreateDamagePopup(int damage){
		GameObject LabelGO = (GameObject)Instantiate(LabelPrefab,
		                                                      LabelTransform.position,
		                                                      LabelTransform.rotation);
		LabelGO.GetComponentInChildren<Text>().text = damage.ToString();
	}

}
