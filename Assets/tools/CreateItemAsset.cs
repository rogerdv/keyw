#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class ItemAsset
{
	[MenuItem("Assets/Create/Item")]
	public static void CreateAsset ()
	{
		CustomAssetUtility.CreateAsset<BaseItem>();
	}
}
#endif