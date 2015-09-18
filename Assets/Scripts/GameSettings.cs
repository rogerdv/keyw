using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;

public class GameSettings {
	int DificultLevel;
	public bool shadows;
	public bool ssao;
	public int aa;

	/**
	 * Load from user saved preferences
	 * 
	*/	 
	public bool Load() {
		var UserDir = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		Debug.Log (UserDir);
		if (!System.IO.Directory.Exists (UserDir + "/keyw")) { 
			System.IO.Directory.CreateDirectory(UserDir + "/keyw");
		}
		//config file doesnt exists, create one and fill
		//it with some default (conservative) values
		if (!File.Exists (UserDir + "/keyw/keyw.config")) {
			System.IO.Directory.CreateDirectory (UserDir + "/keyw");
			var cfg = File.CreateText (UserDir + "/keyw/keyw.config");
			cfg.WriteLine ("<?xml version=\"1.0\" ?>");
			cfg.WriteLine ("<config>");
			cfg.WriteLine (" <option name=\"aa\" value=\"2\" />"); 
			cfg.WriteLine (" <option name=\"bloom\" value=\"no\" />");
			cfg.WriteLine (" <option name=\"ssao\" value=\"no\" />");
			cfg.WriteLine (" <option name=\"shadows\" value=\"yes\" />");
			cfg.WriteLine (" <option name=\"difficulty\" value=\"easy\" />");
			cfg.WriteLine ("</config>");
			cfg.Flush ();
			cfg.Close();
			ssao = false;
			aa = 2;
			shadows = false;
			DificultLevel = 1;
		} else {
			//var cfg = File.OpenRead(UserDir + "/keyw/keyw.config");
			var t = File.ReadAllText(UserDir + "/keyw/keyw.config");
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(t);
			XmlNodeList optNodes = doc.SelectNodes ("config/option");
			foreach (XmlNode n in optNodes){
				if (n.Attributes.GetNamedItem("name").Value=="aa") {//antialias
					var v = n.Attributes.GetNamedItem("value").Value;
					aa = int.Parse(v);
				} else if (n.Attributes.GetNamedItem("name").Value=="ssao") {//ssao
					var v = n.Attributes.GetNamedItem("value").Value;
					if (v=="yes") ssao = true;
						else ssao = false;
				} else if (n.Attributes.GetNamedItem("name").Value=="shadows") {
					var v = n.Attributes.GetNamedItem("value").Value;
					if (v=="yes") shadows = true;
						else shadows = false;
				}
			}
		}
		return true;
	}

	public bool Save(){
		StreamWriter cfg;
		var UserDir = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		if (!File.Exists (UserDir + "/keyw/keyw.config")) {
			System.IO.Directory.CreateDirectory (UserDir + "/keyw");
			cfg = File.CreateText (UserDir + "/keyw/keyw.config");
			cfg.WriteLine ("<?xml version=\"1.0\" ?>");
			cfg.WriteLine ("<config>");

		} else {
			//open and reset
			File.Delete(UserDir + "/keyw/keyw.config");
			cfg = File.CreateText (UserDir + "/keyw/keyw.config");
			cfg.WriteLine ("<?xml version=\"1.0\" ?>");
			cfg.WriteLine ("<config>");			

		}
		cfg.WriteLine (" <option name=\"aa\" value=\"2\" />"); 
		cfg.WriteLine (" <option name=\"bloom\" value=\"no\" />");
		if (!ssao)
			cfg.WriteLine (" <option name=\"ssao\" value=\"no\" />");
		else
			cfg.WriteLine (" <option name=\"ssao\" value=\"yes\" />");
		if (!shadows)
			cfg.WriteLine (" <option name=\"shadows\" value=\"no\" />");
		else
			cfg.WriteLine (" <option name=\"shadows\" value=\"yes\" />"); 

		cfg.WriteLine (" <option name=\"difficulty\" value=\"easy\" />");
		cfg.WriteLine ("</config>");
		cfg.Flush ();
		cfg.Close ();
		return true;	
	}

}
