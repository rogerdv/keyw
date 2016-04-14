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
			BaseItem temp;
			if (node.Attributes.GetNamedItem("type").Value=="weapon")
				temp = new ItemWeapon();
			else if (node.Attributes.GetNamedItem("type").Value=="potion")
				temp = new ItemPotion();
			else 
				temp = new BaseItem();

			temp.Name = node.Attributes.GetNamedItem("name").Value;
			temp.Desc = node.Attributes.GetNamedItem("description").Value;
			temp.type = node.Attributes.GetNamedItem("type").Value;
			temp.range = float.Parse(node.Attributes.GetNamedItem("range").Value);
			temp.prefab = node.Attributes.GetNamedItem("prefab").Value;
			temp.portrait = node.Attributes.GetNamedItem("portrait").Value;
			temp.weight = int.Parse(node.Attributes.GetNamedItem("weight").Value);
			temp.ParentSkill = node.Attributes.GetNamedItem("skill").Value;
			var slot = node.Attributes.GetNamedItem("slot").Value;
			if(slot=="weapon") 
				temp.slot = ItemSlot.Weapon;
			else if (slot=="shield") 
				temp.slot = ItemSlot.Shield;
			temp.attach = node.Attributes.GetNamedItem("bone").Value;
			string[] vals; 
			vals = node.Attributes.GetNamedItem("offset").Value.Split(' ');
			float.TryParse(vals[0], out temp.offset.x);
			float.TryParse(vals[1], out temp.offset.y );
			float.TryParse(vals[2], out temp.offset.z);
			vals = node.Attributes.GetNamedItem("rotation").Value.Split(' ');
			float.TryParse(vals[0], out temp.rot.x);
			float.TryParse(vals[1], out temp.rot.y);
			float.TryParse(vals[2], out temp.rot.z);
			vals = node.Attributes.GetNamedItem("scale").Value.Split(' ');
			float.TryParse(vals[0], out temp.scale.x);
			float.TryParse(vals[1], out temp.scale.y);
			float.TryParse(vals[2], out temp.scale.z);
			XmlNodeList properties = node.ChildNodes;
			foreach (XmlNode pn in properties) {
				var p = new Property();
				p.name = pn.Attributes.GetNamedItem("name").Value;
				p.type = pn.Attributes.GetNamedItem("type").Value;
				p.value = float.Parse(pn.Attributes.GetNamedItem("value").Value);
				temp.props.Add(p);
			}
			ItemDefs.Add(node.Attributes.GetNamedItem("id").Value, temp);
		}
	}

	public BaseItem CreateItem(string type) {
		if (ItemDefs.ContainsKey (type))
			return ItemDefs [type];
		else
			return null;
	}

	public BaseItem RandomItem(){
		int r; 
		var temp = new BaseItem ();
		r = Random.Range (1, ItemDefs.Count);
		Debug.Log (r);
		int c = 1;
		foreach (var value in ItemDefs.Values)
		{
			if (c==r) 
				return value;
			c++;
		}

		return null;
	}

}
