using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Private-Data.
	private static T PrivateInstance;
	private static object PrivateLock = new object();
	private static bool ApplicationIsQuitting = false;
	
	// Get the singleton instance.
	public static T Instance
	{
		get
		{
			// Check if the application is quitting.
			if(ApplicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Application quitting.");
				return null;
			}
			
			// Lock to be safe about threading.
			lock(PrivateLock)
			{
				// Check if the private instance is null.
				if(PrivateInstance == null)
				{
					// Check if we have more than one instance found.
					if(FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogError("[Singleton] More than one instance found !");
						return PrivateInstance;
					}
					
					// Find the object.
					PrivateInstance = (T)FindObjectOfType(typeof(T));
					
					// Check if the private instance is null.
					if(PrivateInstance == null)
					{
						GameObject Singleton = new GameObject();
						PrivateInstance = Singleton.AddComponent<T>();
						Singleton.name = "[Singleton]"+ typeof(T).ToString();
						DontDestroyOnLoad(Singleton);
						Debug.Log("[Singleton]" + typeof(T) + " created.");
					}
				}
				
				// Return the private instance.
				return PrivateInstance;
			}
		}
	}
	
	// Called when the application is quitting.
	public void OnDestroy()
	{
		ApplicationIsQuitting = true;
		Debug.Log("[Singleton]" + typeof(T) + " destroyed.");
	}
}