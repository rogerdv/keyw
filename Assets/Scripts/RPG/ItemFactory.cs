using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


public class ItemFactory  {
	Dictionary<string, BaseItem> ItemDefs;	//list of item definitions

	public ItemFactory(){
		ItemDefs = new Dictionary<string, BaseItem> ();
	}

	public void Init(string file){
		TextAsset textAsset = (TextAsset) Resources.Load(file);
		if (!textAsset) Debug.Log("failed to load xml resource");
		var doc = new XmlDocument();
		
		doc.LoadXml (textAsset.text);

		//parse the item definition XML file
		XmlNodeList root = doc.SelectNodes ("items/item");
		foreach (XmlNode node in root) {
			var temp = new BaseItem ();
			temp.Name = node.Attributes.GetNamedItem("name").Value;
			temp.Desc = node.Attributes.GetNamedItem("description").Value;
			temp.weight = int.Parse(node.Attributes.GetNamedItem("weight").Value);
			var slot = node.Attributes.GetNamedItem("slot").Value;
			if(slot=="weapon") 
				temp.slot = ItemSlot.Weapon;
			else if (slot=="shield") 
				temp.slot = ItemSlot.Shield;
			temp.attach = node.Attributes.GetNamedItem("bone").Value;
			ItemDefs.Add(node.Attributes.GetNamedItem("id").Value, temp);
		}
	}

	public BaseItem CreateItem(string type) {
		return ItemDefs[type];
	}

	public BaseItem RandomItem(){
		int r; 
		var temp = new BaseItem ();
		r = Random.Range (1, 100);
		return null;
	}

}
