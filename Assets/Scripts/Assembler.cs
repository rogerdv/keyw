using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#region TransformCatalog
public class TransformCatalog : Dictionary<string, Transform>
{
	#region Constructors
	public TransformCatalog(Transform transform)
	{

		Catalog(transform);
	}
	#endregion
	
	#region Catalog
	private void Catalog(Transform transform)
	{
		if (ContainsKey (transform.name))
			Remove(transform.name);
		
		Add(transform.name, transform);
		foreach (Transform child in transform)
			Catalog(child);
	}
	#endregion
}
#endregion

#region DictionaryExtensions
public class DictionaryExtensions
{
	public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
	{
		TValue value;
		source.TryGetValue(key, out value);
		return value;
	}
}
#endregion

public class Assembler {

	string[] BodyPartNames = {"Head", "Torso", "Hands", "Legs", "Feet"};

	public void BuildCharacter(GameObject parentGO, GameObject[] BodyParts, out GameObject[] ActiveParts) { 
		GameObject temp;		//temporary game object to hold pieces
		ActiveParts = new GameObject[5];
		var boneCatalog = new TransformCatalog(parentGO.transform);
		for (int i=0; i<=4; i++) {
			temp = GameObject.Instantiate(BodyParts[i]) as GameObject;
			ActiveParts[i] = new GameObject();
			ActiveParts[i].transform.SetParent(parentGO.transform);
			ActiveParts[i].transform.localPosition = Vector3.zero;
			ActiveParts[i].transform.localScale = parentGO.transform.localScale;
			ActiveParts[i].name = BodyPartNames[i];

			//copy skinned mesh renderer components
			CopySkinnedMeshRenderer(temp, ActiveParts[i], boneCatalog);		
			GameObject.DestroyImmediate(temp);
		}
	}
	
	public void CopySkinnedMeshRenderer(GameObject source, GameObject target, TransformCatalog bones) {
		var skinnedMeshRenderers = source.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (var sourceRenderer in skinnedMeshRenderers)
		{
			var comp = target.AddComponent<SkinnedMeshRenderer>();
			comp.sharedMesh = sourceRenderer.sharedMesh;
			comp.materials = sourceRenderer.materials;
			comp.bones = TranslateTransforms(sourceRenderer.bones, bones);
		}
	}
	
	public Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog)
	{
		var targets = new Transform[sources.Length];
		for (var index = 0; index < sources.Length; index++)
			targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
		return targets;
	}

}
