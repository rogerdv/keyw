using UnityEngine;
using System.Collections;

public enum EffectType {
	None = 0,
	Poison = 1,
	Stun = 2,
	Disease = 3

}

/**
 * An effect is any bonus or buff affecting a character
 * 
 */
public class Effect  {
	public EffectType type;
	public float time;			//total time
	public float elapsed;		//elapsed time
	public int amount;
	public BaseCharacter target;

	// applies the effect to the owner, once per second
	void Apply () {
	
	}
	
}
