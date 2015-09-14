using System;
using UnityEngine;

public enum Attributes {
	Str = 0,
	Dext = 1,
	Int = 2,
	Const = 3,
	Char = 4
}

/**
 * Basic character attribute, like Strength, Dexterity, etc
 * 
 */
[Serializable]
public class BaseAttrib  {
	public int baseValue;
	public int modifier;		//modifier

	public BaseAttrib (){
		baseValue = 10;
	}

	public int getValue() {		//return total value
		return baseValue + modifier;
	}
}
