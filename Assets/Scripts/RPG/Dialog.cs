﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/**
 * Condition that must be met for the line to be displayed
 * for example, Int greater than can be expressed
 * CheckType = Int, value = greater, eval = 15
 * for a Quest
 * CheckType = HasQuest, value = quest-name, eval = active
 * */
public class Check {
	string CheckType;		//stat, quest
	string value;
	string eval;
}

/**
 * An Action is something triggered by a player line
 * Could be a quest assignment, receiving a reward, etc
 * */
public class Action {
	public string ActionType;
	public string id;		//quest or item id
	public string value;	//value to set, amount of items
}

/**
 * The NPC text
 * */
public class NPCLine {	
	public string text;
	public string answers;
	public List<Check> checks;
}

/**
 * Player line
 * */
public class PlayerLine {
	public string text;
	public string link;		//next NPC line
	public List<Check> checks;
	public List<Action> actions;
	public PlayerLine() {
		actions = new List<Action> ();
	}
}

/**
 * this is the dialog container
 * */
public class Dialog  {

	private static XmlDocument doc;
	public Texture2D backg;

	private static BaseCharacter player;

	public Dialog() {

	}

	/**
	 * Load the dialog from Xml
	 * */
	public void Load(BaseCharacter PlayerClass, string file) {
		player = PlayerClass; 
		TextAsset textAsset = (TextAsset) Resources.Load(file);
		if (!textAsset) Debug.Log("failed to load xml resource "+file);
		doc = new XmlDocument();

		doc.LoadXml (textAsset.text);
	}

	/**
	 * Receives a node and starts parsing from there, returning
	 */
	public NPCLine GetNode (string id){
		NPCLine retval = new NPCLine(); 
		XmlNodeList root = doc.SelectNodes ("dialog/node");
		foreach (XmlNode node in root) {
			if (node.Attributes.GetNamedItem("id").Value==id) {
				//parse the lines
				XmlNodeList lines = node.ChildNodes;
				foreach (XmlNode line in lines) {
					//Debug.Log("NPC line"+line.InnerText);
					//validate checks

					if (line.Attributes.GetNamedItem("check")!=null) {
						string check = line.Attributes.GetNamedItem("check").Value;
						string value = line.Attributes.GetNamedItem("value").Value;
						string eval = line.Attributes.GetNamedItem("eval").Value;
						if (check=="HasQuest") {
							//check if player has quest
							if (player.GetQuestStatus(value)==eval) {
								//this is the line
								retval.text = line.InnerText;
								retval.answers = line.Attributes.GetNamedItem("answers").Value;
								return retval;
							}
							//break;
						} else if (check=="HasItem") {
						} //if checks
					} else { //has no checks
						Debug.Log("NPC line has no checks "+line.InnerText);
						retval.text = line.InnerText;
						retval.answers = line.Attributes.GetNamedItem("answers").Value;
						return retval;
					}//if check
				} //for each line

				//Debug.Log("NPC line"+NPCline);
			}
		}//foeach node 
		return null;
	}

	public Dictionary<string, PlayerLine> GetAnswers(string alist) {
		Dictionary<string, PlayerLine> choices;	// Player lines
		choices = new Dictionary<string, PlayerLine> ();

		var answers= alist.Split(' ');
		XmlNodeList answlist = doc.SelectNodes ("dialog/answer");
		int count = 0;
		choices.Clear ();
		foreach (XmlNode answnode in answlist) {
			//Debug.Log("Checking "+answnode.Attributes.GetNamedItem("id").Value);
			foreach (string answer in answers){ 
				//is this one of the answers Im loking for?
				if (answnode.Attributes.GetNamedItem("id").Value==answer) {
					//Debug.Log("Found answer "+answer);
					PlayerLine l = new PlayerLine();
					var anschoices = answnode.ChildNodes;
					foreach (XmlNode cnode in anschoices) {
						if (cnode.Name== "option") { //it is text
							l.text = cnode.InnerText;
							l.link = cnode.Attributes.GetNamedItem("link").Value;
						} else if (cnode.Name== "action") { //action description
							Action a = new Action();
							a.ActionType = cnode.Attributes.GetNamedItem("type").Value;
							a.id = cnode.Attributes.GetNamedItem("id").Value;
							a.value = cnode.Attributes.GetNamedItem("value").Value;
							l.actions.Add(a);
						}
					}
					//Debug.Log("answer text is "+anschoices.Item(0).InnerText);

					choices.Add(answnode.Attributes.GetNamedItem("id").Value, l);
					//choicelinks.Add(count,anschoices.Item(0).Attributes.GetNamedItem("link").Value);
					//count++;
					break;
				}//if
			}//for each answer
		} //foreach answer list
		return choices;
	}	

}
