using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

enum RewardType {
	Gold = 0,
	Item = 1,
	Exp = 2
}

[Serializable]
public class Reward {
	RewardType type;
	int amount;			//this could be exp/gold amount or item id
}

[Serializable]
public class Quest {
	public string Name;
	public string Description;
	public string Notes;		//additional info related to this quest
	public string status;
	public List<Reward> rewards;			

	public Quest(){
		rewards = new List<Reward> ();
		status = "active";
	}

}
