using UnityEngine;
using System.Collections;

public class CamaraWoW : MonoBehaviour
{
	public Transform target;
	public float targetHeight = 1;
	public float distance = 3;
	public float maxDistance = 10;
	public float minDistance = 0.5f;
	public float xSpeed = 250;
	public float ySpeed = 120;
	public float yMinLimit = -40;
	public float yMaxLimit = 80;
	public float zoomRate = 50;
	public float rotationDampening = 15;
	private float x = 0f;
	private float y = 0f;
	//    bool isTalking = false;
	public float count;
	//AddComponentMenu("Camera-Control/WoW Camera")
	// Use this for initialization
	void Start ()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the Rigid Body not change rotation.
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	void LateUpdate()
	{
		if (!target)
		{
			return;
		}
		
		//If either mouse buttons are down, let them govern camera position
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}
		//Otherwise, ease behind the target if any of the directional keys are pressed.
		else if ((Input.GetAxis("Vertical") != 0f )|| (Input.GetAxis("Horizontal") != 0f))
		{
			float targetRotationAngle = target.eulerAngles.y;
			float currentRotationAngle = transform.eulerAngles.y;
			x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
		}
		
		distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		//Rotate Camera
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		transform.rotation = rotation;
		
		//Position Camera
		var position = target.position - (rotation * Vector3.forward * distance + new Vector3(0, -targetHeight, 0));
		transform.position = position;
		
		//Is view blocked?
		RaycastHit hit;
		Vector3 trueTargetPosition = target.transform.position - new Vector3(0, -targetHeight, 0);
		
		//Cast the line to check:
		//HAL 2011-jan-23 / make sure its not colliding with the original target
		if (Physics.Linecast(trueTargetPosition, transform.position,out hit) && hit.transform != target)
		{
			count += Time.deltaTime;
			
			//If so, shorten distance so camera is in front of object:
			if (count > 0.9)
			{
				var tempDistance = Vector3.Distance(trueTargetPosition, hit.point) - 0.28f;
				
				// Finally reposition the camera:
				position = target.position - (rotation * Vector3.forward * tempDistance + new Vector3(0, -targetHeight, 0));
				transform.position = position;
			}
		}
		else
		{
			count = 0;
		}
		
	}
	
	static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
		{
			angle += 360;
		}
		if (angle > 360)
		{
			angle -= 360;
		}
		return Mathf.Clamp(angle, min, max);
	}


}