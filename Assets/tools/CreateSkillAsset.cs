#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class SkillAsset
{
	[MenuItem("Assets/Create/Base Skill")]
	public static void CreateAsset ()
	{
		CustomAssetUtility.CreateAsset<BaseSkill>();
	}
}
#endif